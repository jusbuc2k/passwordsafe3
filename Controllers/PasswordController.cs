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
        public PasswordController(AppDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public AppDbContext DbContext { get; set; }

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

            var masterKeyBytes = Convert.FromBase64String(model.MasterKey);

            var userKey = this.DbContext.VaultUserKeys.Add(new VaultUserKey(){
                Vault = vault,
                VaultID = vault.VaultID,
                Username = "justin",
                MasterKey = masterKeyBytes
            }).Entity;

            this.DbContext.SaveChanges();

            return this.Ok(new {
                VaultID = vault.VaultID
            });
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

        [HttpGet]
        [Route("GetPassword/{passwordID}")]
        public IActionResult GetPassword(int passwordID)
        {
            return this.Ok(this.DbContext.Passwords
                .Where(x => x.PasswordID == passwordID)
                .Select(s => new {
                    PasswordID = s.PasswordID,
                    Name = s.Name, 
                    Description = s.Description,
                    Data = Convert.ToBase64String(s.Data)
                })
                .SingleOrDefault()
            );
        }
    }

}