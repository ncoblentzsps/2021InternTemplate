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
        public string Code { get; set; }
        //This is what you would use for multiple "Wallets"
        //public ICollection<Wallet> Wallets { get; set; }

    }
}
