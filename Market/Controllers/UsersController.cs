using Market.Models;
using Market.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Market.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Gerente,Administrador")]
        // GET: Users
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var usersView = new List<UserView>();
            foreach (var user in users)
            {
                var userView = new UserView
                {
                    Email = user.Email,
                    Name = user.UserName,
                    UserID = user.Id
                };
                usersView.Add(userView);
            }
            return View(usersView);
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Roles(string userID)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var roles = RoleManager.Roles.ToList();
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userID);

            var rolesView = new List<RoleView>();
            
                foreach (var item in user.Roles)
                {
                    var role = roles.Find(r => r.Id == item.RoleId);

                    var roleView = new RoleView
                    {
                        RoleID = role.Id,
                        Name = role.Name
                    };
                    rolesView.Add(roleView);
                }
            
            
            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView
            };

            return View(userView);
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult AddRole(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userID);

            if (user==null)
            {
                return HttpNotFound();
            }
            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id
                
            };

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var list = RoleManager.Roles.ToList();
            list.Add(new IdentityRole { Id ="", Name = "[Seleccione un Rol]" });
            list = list.OrderBy(r => r.Name).ToList();
            ViewBag.RoleID = new SelectList(list, "Id", "Name");

            return View(userView);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult AddRole(string userID,FormCollection form)
        {
            var roleID = Request["RoleID"];
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userID);
            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id

            };
            if (string.IsNullOrEmpty(roleID))
            {
                ViewBag.Error = "Debes seleccionar un Role";
                

                

                var list = RoleManager.Roles.ToList();
                list.Add(new IdentityRole { Id = "", Name = "[Seleccione un Rol]" });
                list = list.OrderBy(r => r.Name).ToList();
                ViewBag.RoleID = new SelectList(list, "Id", "Name");

                return View(userView);
            }
            var roles = RoleManager.Roles.ToList();
            var role = roles.Find(r => r.Id == roleID);
            if (!userManager.IsInRole(user.Id,role.Name))
            {
                userManager.AddToRole(userID, role.Name);
            }

            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id == item.RoleId);

                var roleView = new RoleView
                {
                    RoleID = role.Id,
                    Name = role.Name
                };
                rolesView.Add(roleView);
            }


            userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView
            };

            return View("Roles",userView);
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(string userID, string roleID)
        {
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(roleID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var user = userManager.Users.ToList().Find(u => u.Id == userID);
            var role = RoleManager.Roles.ToList().Find(r => r.Id == roleID);

            if (userManager.IsInRole(user.Id, role.Name))
            {
                userManager.RemoveFromRole(user.Id, role.Name);
            }

            var users = userManager.Users.ToList();
            var roles = RoleManager.Roles.ToList();

            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id == item.RoleId);

                var roleView = new RoleView
                {
                    RoleID = role.Id,
                    Name = role.Name
                };
                rolesView.Add(roleView);
            }


            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView
            };

            return View("Roles",userView);

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}