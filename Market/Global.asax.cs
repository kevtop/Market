using Market.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Market
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<
                Models.MarketContext, 
                Migrations.Configuration>());
            ApplicationDbContext db = new ApplicationDbContext();
            CreateRoles(db);
            CreateSuperUser(db);
            AddPermisionsToSuperUser(db);
            db.Dispose();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void AddPermisionsToSuperUser(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var user = userManager.FindByName("kevinvelascodiaz@hotmail.com");
            if (!userManager.IsInRole(user.Id, "View"))
            {
                userManager.AddToRole(user.Id, "View");
            }
            if (!userManager.IsInRole(user.Id, "Edit"))
            {
                userManager.AddToRole(user.Id, "Edit");
            }
            if (!userManager.IsInRole(user.Id, "Create"))
            {
                userManager.AddToRole(user.Id, "Create");
            }
            if (!userManager.IsInRole(user.Id, "Delete"))
            {
                userManager.AddToRole(user.Id, "Delete");
            }

        }

        private void CreateSuperUser(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var user = userManager.FindByName("");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "kevinvelascodiaz@hotmail.com",
                    Email = "kevinvelascodiaz@hotmail.com"
                };
                userManager.Create(user, "Kevin123.");
            };
        }

        private void CreateRoles(ApplicationDbContext db)
        {
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            if (!RoleManager.RoleExists("View"))
            {
                RoleManager.Create(new IdentityRole("View"));
            }
            if (!RoleManager.RoleExists("Edit"))
            {
                RoleManager.Create(new IdentityRole("Edit"));
            }
            if (!RoleManager.RoleExists("Create"))
            {
                RoleManager.Create(new IdentityRole("Create"));
            }
            if (!RoleManager.RoleExists("Delete"))
            {
                RoleManager.Create(new IdentityRole("Delete"));
            }
        }
    }
}
