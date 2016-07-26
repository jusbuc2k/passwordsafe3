using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication.Data;

namespace WebApplication.Controllers
{
    public class CreateVaultModel
    {
        public string Name { get;set; }

        public string MasterKey { get;set; }

        public string MasterIV { get; set; }
    }
}