using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Data
{

    [Table("Vault")]
    public class Vault
    {
        public int VaultID { get; set;}

        public string Name { get; set; }

    }
}