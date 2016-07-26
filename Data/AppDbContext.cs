using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WebApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Vault> Vaults { get; set; }

        public DbSet<Password> Passwords { get; set; }

        public DbSet<VaultUserKey> VaultUserKeys { get; set;}
    }

}
