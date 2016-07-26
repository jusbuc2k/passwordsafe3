using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Data
{

    [Table("Password")]
    public class Password
    {
        public int PasswordID { get; set; }

        public int VaultID { get; set;}

        public Vault Vault { get;set; }

        public string Name { get; set; }

        public string Description { get; set; }

        // The encrypted password + other data
        public byte[] Data { get; set; }
    }
}