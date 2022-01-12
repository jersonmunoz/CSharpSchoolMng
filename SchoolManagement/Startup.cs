using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SchoolManagement.Models;

[assembly: OwinStartupAttribute(typeof(SchoolManagement.Startup))]
namespace SchoolManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            initializeRolesAndUsers();
        }
        public void initializeRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser { UserName = "admin", Email = "admin@gmail.com", BirthDate = DateTime.Now };
                var password = "password";
                var usr = userManager.Create(user, password);
                if (usr.Succeeded)
                {
                    var results = userManager.AddToRole(user.Id, "Admin");
                }
            }
            if (!roleManager.RoleExists("Supervisor"))
            {
                var role = new IdentityRole();
                role.Name = "Supervisor";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Teacher"))
            {
                var role = new IdentityRole();
                role.Name = "Teacher";
                roleManager.Create(role);
            }
        }
        }
}
