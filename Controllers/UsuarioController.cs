using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaFarmacia.Models;
using LaFarmacia.Models.viewModel;
using Microsoft.Ajax.Utilities;

namespace LaFarmacia.Controllers
{
    public class UsuarioController : Controller
    {
        private LaFarmaciaDBEntities db = new LaFarmaciaDBEntities();

        // GET: Usuario
        public ActionResult Index()
        {
            List<UserDTO> usersDTO = new List<UserDTO>();
            foreach(T_User user in db.T_User)
            {
                usersDTO.Add(new UserDTO()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    RolId = user.RolId,
                    RolDescription = user.T_Rol.Description,
                    State = user.State,
                    Password = "******"
                });
            }
            return View(usersDTO);
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_User user = db.T_User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserDTO userDTO = new UserDTO()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                RolId = user.RolId,
                RolDescription = user.T_Rol.Description,
                State = user.State,
                Password = "******"
            };
            return View(userDTO);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            ViewBag.listaRoles = ListaRoles();
            return View();
        }

        // POST: Usuario/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,RolId,State,Password")] UserDTO userDTO)
        {
            
            if (ModelState.IsValid)
            {
                db.SP_NewUser(userDTO.Name,userDTO.Email,userDTO.RolId,userDTO.State,userDTO.Password);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userDTO);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.listaRoles = ListaRoles();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_User user = db.T_User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserDTO userDTO = new UserDTO()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                RolId = user.RolId,
                State = user.State,
                Password = ""
            };
            return View(userDTO);
        }

        // POST: Usuario/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,RolId,State,Password")] UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                db.SP_UpdateUser(userDTO.Id,userDTO.Name,userDTO.Email,userDTO.RolId,userDTO.State,userDTO.Password);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userDTO);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_User user = db.T_User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserDTO userDTO = new UserDTO()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                RolId = user.RolId,
                State = user.State,
                Password = "*****"
            };
            return View(userDTO);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            T_User user = db.T_User.Find(id);
            db.T_User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<SelectListItem> ListaRoles()
        {
            List<SelectListItem> roles = new List<SelectListItem>();
            foreach(T_Rol rol in db.T_Rol)
            {
                roles.Add(new SelectListItem()
                {
                    Value = rol.Id.ToString(),
                    Text = rol.Description
                });
            }
            return roles;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var usr = db.SP_CheckUser(user.Email, user.Password).ToList();
            if (usr.Count() > 0)
            {
                UserDTO usuario = new UserDTO()
                {
                    Email = usr[0].Email,
                    Id = usr[0].Id,
                    Name = usr[0].Name,
                    RolId = usr[0].RolId,
                    State = usr[0].State
                };
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.valor = 1;
                ViewBag.mensaje = "Usuario y contraseña incorrectas";

                return View();
            }
            

        }
    }
}
