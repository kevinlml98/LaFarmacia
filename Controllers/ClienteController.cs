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

namespace LaFarmacia.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            List<ClientDTO> clients = new List<ClientDTO>();
            using (LaFarmaciaDBEntities db = new LaFarmaciaDBEntities())
            {
                foreach (T_Client client in db.T_Client)
                {
                    clients.Add(new ClientDTO
                    {
                        Id = client.Id,
                        Name = client.Name,
                        Email = client.Email
                    });
                }
                return View(clients);
            }
        }

        // GET: Cliente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (LaFarmaciaDBEntities db = new LaFarmaciaDBEntities())
            {
                T_Client client = db.T_Client.Find(id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                ClientDTO clientDTO = new ClientDTO()
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email
                };
                return View(clientDTO);
            }

        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email")] ClientDTO clientDTO)
        {
            if (ModelState.IsValid)
            {
                using (LaFarmaciaDBEntities db = new LaFarmaciaDBEntities())
                {
                    T_Client client = new T_Client()
                    {
                        Id = clientDTO.Id,
                        Name = clientDTO.Name,
                        Email = clientDTO.Email
                    };
                    db.T_Client.Add(client);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(clientDTO);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (LaFarmaciaDBEntities db = new LaFarmaciaDBEntities())
            {
                T_Client client = db.T_Client.Find(id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                ClientDTO clientDTO = new ClientDTO()
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email
                };
                return View(clientDTO);
            }
        }

        // POST: Cliente/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email")] ClientDTO clientDTO)
        {
            if (ModelState.IsValid)
            {
                using (LaFarmaciaDBEntities db = new LaFarmaciaDBEntities())
                {
                    T_Client cliente = new T_Client()
                    {
                        Id = clientDTO.Id,
                        Name = clientDTO.Name,
                        Email = clientDTO.Email
                    };
                    db.Entry(cliente).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return View(clientDTO);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (LaFarmaciaDBEntities db = new LaFarmaciaDBEntities())
            {
                T_Client cliente = db.T_Client.Find(id);
                if (cliente == null)
                {
                    return HttpNotFound();
                }
                ClientDTO clientDTO = new ClientDTO()
                {
                    Id = cliente.Id,
                    Name = cliente.Name,
                    Email = cliente.Email
                };
                return View(clientDTO);
            }
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (LaFarmaciaDBEntities db = new LaFarmaciaDBEntities())
            {
                T_Client cliente = db.T_Client.Find(id);
                db.T_Client.Remove(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

    }
}