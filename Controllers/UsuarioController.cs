using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using LaFarmacia.Models.viewModel;

namespace LaFarmacia.Controllers
{
    public class UsuarioController : Controller
    {
        private LaFarmaciaDBEntities3 db = new LaFarmaciaDBEntities3();

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
            foreach(SP_CheckUser_Result user in db.T_User)
            {
                string contrasenna = contraseña(8);

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
            SP_CheckUser_Result user = db.T_User.Find(id);
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
                Password = contraseña(8)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,RolId,State,Password")] UserDTO userDTO)
        {
               if (ModelState.IsValid)
               {
                    db.SP_NewUser(userDTO.Name, userDTO.Email, userDTO.RolId, userDTO.State, contraseña(8));
                    db.SaveChanges();
                    return RedirectToAction("Index");
               }
                ViewBag.listaRoles = ListaRoles();
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
            SP_CheckUser_Result user = db.T_User.Find(id);
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
    }
}
