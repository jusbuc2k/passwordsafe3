using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Data
{
    [Table("VaultUserKey")]
    public class VaultUserKey
    {
        [Key]
        public int VaultID { get; set; }

        public Vault Vault { get;set; }

        public string Username { get;set; }

        // This is a hash of the user's password, used to validate it when entering
        public byte[] Hash { get;set; }

        // This is the MasterKey for the vault, encrypted by the user's pin/password
        public byte[] MasterKey { get; set; }
    }
}