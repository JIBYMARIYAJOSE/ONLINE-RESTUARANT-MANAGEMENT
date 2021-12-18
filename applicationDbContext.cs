using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;//step 2: install ms entity frameworkcore sql server..tools-> nuget package->manage nuget package for solution.
using Microsoft.EntityFrameworkCore;
using firstProjectWith_ASP.Models;


namespace firstProjectWith_ASP.Data
{   //step 3: create a class with Dbcontext
    public class applicationDbContext : IdentityDbContext
    {  //step 4 : define the below step as given for configuration
        public applicationDbContext(DbContextOptions<applicationDbContext> options) : base(options)
        {

        }
        //step 5 define the following to introduce entity
        //step 6 - inside the startup.cs->configure.services introduce this entity
        public DbSet<Category> category { get; set; }

        public DbSet <ApplicationType> ApplicationType { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; } 
    }
}
