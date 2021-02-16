using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021InternTemplate.Models
{
    public class MoneroUser : IdentityUser
    {
        public string Wallet { get;set; }
    }
}
