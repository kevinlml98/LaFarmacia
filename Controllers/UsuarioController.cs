using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
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
        private string contraseña(int largo)
        {
            const string palabras = "ABCDEFGHIJK1234567890#$%^&**!';";

            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < largo; i++)
            {
                int entrada = random.Next(palabras.Length);
                sb.Append(palabras[entrada]);
            }
            return sb.ToString();

        }
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
            
            var usr = db.SP_CheckUser(user.Email, user.Password).ToList();
            if (usr.Count() > 0)
            {
                T_Rol rol = db.T_Rol.Find(usr[0].RolId);
                UserDTO usuario = new UserDTO()
                {
                    Email = usr[0].Email,
                    Id = usr[0].Id,
                    Name = usr[0].Name,
                    RolId = usr[0].RolId,
                    State = usr[0].State,
                    RolDescription = rol.Description
                };
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Mostrar = 1;
                ViewBag.Mensaje = "Usuario o contraseña incorrectos";
                return View();
            }
            
        }
    }
}
