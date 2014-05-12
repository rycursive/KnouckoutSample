using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnockoutSample.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Pattern.Ef6;

namespace KnockoutSample.DAL
{
    public class ApplicationDbContext : IdentityDatacontext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<KnockoutSample.Model.Product> Products { get; set; }

        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}
    }
}
