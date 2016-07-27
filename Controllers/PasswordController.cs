using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication.Data;

namespace WebApplication.Controllers
{
    //[Authorize]
    [Route("Password")]
    public class PasswordController : Controller
    {    
        
        private static byte[] CreatePasswordSalt()
        {
            var salt = new byte[16];
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();

            rng.GetBytes(salt);

            return salt;
        }

        private static byte[] CreateMasterKey()
        {
            var key = new byte[32];
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();

            rng.GetBytes(key);

            return key;
        }

        private static byte[] GetKey(string password, byte[] salt)
        {
            using (var pb = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, 10000))
            {
                return pb.GetBytes(32);
            }
        }

        private static byte[] Encrypt(byte[] key, string data)
        {
            var iv = CreatePasswordSalt();
            return Encrypt(key, iv, System.Text.UTF8Encoding.UTF8.GetBytes(data));
        }

        private static byte[] Encrypt(string password, byte[] data)
        {
            var salt = CreatePasswordSalt();
            var key = GetKey(password ,salt);

            return Encrypt(key, salt, data);
        }

        private static byte[] Encrypt(byte[] key, byte[] iv, byte[] data)
        {
            using(var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Mode = System.Security.Cryptography.CipherMode.CBC;

                using(var encryptor = aes.CreateEncryptor(key, iv))
                {
                    var encryptedBytes = encryptor.TransformFinalBlock(data, 0, data.Length);

                    return iv.Concat(encryptedBytes).ToArray();
                }
            }
        }

        private static byte[] Decrypt(string password, byte[] encryptedData)
        {
            var salt = new byte[16];

            Array.Copy(encryptedData, salt, salt.Length);

            var key = GetKey(password, salt);

            return Decrypt(key, encryptedData);  
        }

        private static byte[] Decrypt(byte[] key, byte[] encryptedData)
        {
            var iv = new byte[16];
            var data = new byte[encryptedData.Length-16];

            Array.Copy(encryptedData, iv, iv.Length);
            Array.Copy(encryptedData, 16, data, 0, data.Length);

            using(var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Mode = System.Security.Cryptography.CipherMode.CBC;

                using(var decryptor = aes.CreateDecryptor(key, iv))
                {
                    var plainBytes = decryptor.TransformFinalBlock(data, 0, data.Length);

                    return plainBytes;
                }
            }
        }

        public PasswordController(AppDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public AppDbContext DbContext { get; set; }


        private string GetUnlockPassword(int vaultID)
        {
            byte[] data;
            if (this.HttpContext.Session.TryGetValue("Vault:" + vaultID.ToString(), out data))
            {
                return System.Text.UTF8Encoding.UTF8.GetString(data);
            }

            return null;
        }

        [HttpGet]
        [Route("ListVaults")]
        public IActionResult ListVaults()
        {
            return this.Ok(this.DbContext.Vaults.OrderBy(o => o.Name));
        }

        [HttpPost]
        [Route("AddVault")]
        public IActionResult AddVault(CreateVaultModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var vault = this.DbContext.Vaults.Add(new Vault(){
                Name = model.Name      
            }).Entity;

            var masterKey = CreateMasterKey();                      

            var userKey = this.DbContext.VaultUserKeys.Add(new VaultUserKey(){
                Vault = vault,
                VaultID = vault.VaultID,
                Username = "justin",
                MasterKey = Encrypt(model.Password, masterKey)
            }).Entity;

            this.DbContext.SaveChanges();

            return this.Ok(new {
                VaultID = vault.VaultID
            });
        }

        [HttpPost]
        [Route("UnlockVault/{vaultID}")]
        public IActionResult UnlockVault(int vaultID, [FromBody]DecryptPasswordModel model)
        {
            this.HttpContext.Session.Set("Vault:" + vaultID, System.Text.UTF8Encoding.UTF8.GetBytes(model.UnlockPassword));

            return this.Ok();
        }

        [HttpGet]
        [Route("ListPasswords/{vaultID}")]
        public IActionResult ListPasswords(int vaultID)
        {
            var vault = this.DbContext.Vaults
                .Where(x => x.VaultID == vaultID)
                .Select(s => new { 
                    Name = s.Name
                })
                .SingleOrDefault();

            var passwords = this.DbContext.Passwords
                .Where(x => x.VaultID == vaultID)
                .OrderBy(o => o.Name)
                .Select(s => new {
                    PasswordID = s.PasswordID,
                    Name = s.Name, 
                    Description = s.Description 
                });

            var userKey = this.DbContext.VaultUserKeys
                .Where(x => x.VaultID == vaultID && x.Username == "justin")
                .SingleOrDefault();
                
            return this.Ok(new {
                vault = vault,
                masterKey = Convert.ToBase64String(userKey.MasterKey)
            });
        }

        [HttpPost]
        [Route("AddPassword/{vaultID}")]
        public IActionResult AddPassword(int vaultID, [FromBody]CreatePasswordModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }                  

            var userKey = this.DbContext.VaultUserKeys
                .Where(x => x.Username == "justin" && x.VaultID == vaultID)
                .Select(s => s.MasterKey)
                .Single();
            
            var masterKey = Decrypt(GetUnlockPassword(vaultID), userKey);

            var password = this.DbContext.Passwords.Add(new Password(){
                Name = model.Name,
                Description = model.Description,
                Data = Encrypt(masterKey, model.Data)
            });

            this.DbContext.SaveChanges();

            return this.Ok(new {
                PasswordID = password.Entity.PasswordID
            });
        }

        [HttpPost]
        [Route("UpdatePassword/{passwordID}")]
        public IActionResult UpdatePassword(int passwordID, [FromBody]CreatePasswordModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }                  

            var password = this.DbContext.Passwords.Single(x => x.PasswordID == passwordID);

            var userKey = this.DbContext.VaultUserKeys
                .Where(x => x.Username == "justin" && x.VaultID == password.VaultID)
                .Select(s => s.MasterKey)
                .Single();
            
            var masterKey = Decrypt(GetUnlockPassword(password.VaultID), userKey);

            password.Name = model.Name;
            password.Description = model.Description;
            password.Data = Encrypt(masterKey, model.Data);           

            this.DbContext.SaveChanges();

            return this.Ok();
        }

        [HttpGet]
        [Route("GetPassword/{passwordID}")]
        public IActionResult GetPassword(int passwordID)
        {
            return this.Ok(this.DbContext.Passwords
                .Where(x => x.PasswordID == passwordID)
                .Select(s => new {
                    PasswordID = s.PasswordID,
                    Name = s.Name, 
                    Description = s.Description
                })
                .SingleOrDefault()
            );
        }

        [HttpPost]
        [Route("DecryptPassword/{passwordID}")]
        public IActionResult DecryptPassword(int passwordID)
        {
            var password = this.DbContext.Passwords
                .Where(x => x.PasswordID == passwordID)
                .Select(s => new {
                    PasswordID = s.PasswordID,
                    VaultID = s.VaultID,
                    Name = s.Name, 
                    Description = s.Description,
                    Data = s.Data
                })
                .SingleOrDefault();
        
             var userKey = this.DbContext.VaultUserKeys
                .Where(x => x.Username == "justin" && x.VaultID == password.VaultID)
                .Select(s => s.MasterKey)
                .Single();
            
            var masterKey = Decrypt(GetUnlockPassword(password.VaultID), userKey);
            
            var plainText = System.Text.UTF8Encoding.UTF8.GetString(Decrypt(masterKey, password.Data));

            return this.Ok(new {
                PasswordID = password.PasswordID,
                Name = password.Name,
                VaultID = password.VaultID,
                Description = password.Description,
                Data = plainText
            });
        }
    }

}