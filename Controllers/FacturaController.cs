﻿using System;
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
    public class FacturaController : Controller
    {
        private LaFarmaciaDBEntities db = new LaFarmaciaDBEntities();

        // GET: Factura
        public ActionResult Index()
        {
            var t_InvoiceHeader = db.T_InvoiceHeader.Include(t => t.T_MethodPay);
            return View(t_InvoiceHeader.ToList());
        }

        // GET: Factura/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_InvoiceHeader t_InvoiceHeader = db.T_InvoiceHeader.Find(id);
            if (t_InvoiceHeader == null)
            {
                return HttpNotFound();
            }
            return View(t_InvoiceHeader);
        }

        // GET: Factura/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.T_Client, "Id", "Name");
            ViewBag.PayMethodTypeId = new SelectList(db.T_MethodPay, "Id", "Description");
            ViewBag.ProductoId = new SelectList(db.T_Product, "Code", "Description");
            ViewBag.PrecioId = new SelectList(db.T_Product, "Price", "Description");

            string RolUsuario = "Administrador"; //Session["Rol"] as string;

            if (RolUsuario == "Administrador")
            {
                ViewBag.Rol = 1;
            }
            else if(RolUsuario == "Vendedor")
            {
                ViewBag.Rol = 2;
            }
            else
            {
                ViewBag.Rol = 0;
            }

                return View();
        }

        // POST: Factura/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_InvoiceHeader t_InvoiceHeader)
        {

                T_Client client = db.T_Client.Find(t_InvoiceHeader.ClientId);

                if (client == null)
                {

                    string RolUsuario = "Administrador"; //Session["Rol"] as string;

                    if (RolUsuario == "Administrador")
                    {

                        ViewBag.ValorMensaje = 1;
                        ViewBag.Rol = 1;
                        ViewBag.MensajeProceso = "Cliente no se encuentra registrado";
                        ViewBag.PayMethodTypeId = new SelectList(db.T_MethodPay, "Id", "Description", t_InvoiceHeader.PayMethodTypeId);
                        ViewBag.PrecioId = new SelectList(db.T_Product, "Price", "Description");
                        ViewBag.ProductoId = new SelectList(db.T_Product, "Code", "Description", t_InvoiceHeader.ProductoId);
                        return View(t_InvoiceHeader);

                }
                if (RolUsuario == "Vendedor")
                {

                    ViewBag.ValorMensaje = 1;
                    ViewBag.Rol = 2;
                    ViewBag.MensajeProceso = "Cliente no se encuentra registrado";
                    ViewBag.PayMethodTypeId = new SelectList(db.T_MethodPay, "Id", "Description", t_InvoiceHeader.PayMethodTypeId);
                    ViewBag.PrecioId = new SelectList(db.T_Product, "Price", "Description");
                    ViewBag.ProductoId = new SelectList(db.T_Product, "Code", "Description", t_InvoiceHeader.ProductoId);
                    return View(t_InvoiceHeader);

                }
                    else
                    {

                    ViewBag.ValorMensaje = 1;
                    ViewBag.Rol = 0;
                    ViewBag.MensajeProceso = "Cliente no se encuentra registrado, por favor contactar con un administrador.";
                    ViewBag.PayMethodTypeId = new SelectList(db.T_MethodPay, "Id", "Description", t_InvoiceHeader.PayMethodTypeId);
                    ViewBag.PrecioId = new SelectList(db.T_Product, "Price", "Description");
                    ViewBag.ProductoId = new SelectList(db.T_Product, "Code", "Description", t_InvoiceHeader.ProductoId);
                    return View(t_InvoiceHeader);

                    }

                }
                else
                {

                t_InvoiceHeader.Code = CalcularConsecutivo();

                if (ModelState.IsValid)
                    {

                        db.T_InvoiceHeader.Add(t_InvoiceHeader);
                        db.SaveChanges();
                        
                        foreach (var item in t_InvoiceHeader.T_InvoiceDetailList)
                        {

                        decimal precioUnitario = CalcularPrecioUnitario(item.ProductCode);

                        T_InvoiceDetail t_InvoiceDetail = new T_InvoiceDetail();
                        t_InvoiceDetail.InvoiceHeaderCode = t_InvoiceHeader.Code;
                        t_InvoiceDetail.ProductCode = item.ProductCode;
                        t_InvoiceDetail.Count = item.Count;
                        t_InvoiceDetail.UnitPrice = precioUnitario;
                        t_InvoiceDetail.Total = item.Total;
                        db.T_InvoiceDetail.Add(t_InvoiceDetail);

                        }

                    db.SaveChanges();

                }

                    ViewBag.ProductoId = new SelectList(db.T_Product, "Code", "Description", t_InvoiceHeader.ProductoId);
                    ViewBag.PayMethodTypeId = new SelectList(db.T_MethodPay, "Id", "Description", t_InvoiceHeader.PayMethodTypeId);
                    ViewBag.PrecioId = new SelectList(db.T_Product, "Price", "Description");
                    return View(t_InvoiceHeader);

                }
            
        }

        [HttpPost]
        public JsonResult ValidarCliente(int IdCliente)
        {

            Respuesta  respuesta = new Respuesta();

            T_Client client = db.T_Client.Find(IdCliente);

            if (client == null)
            {

                string RolUsuario = "Administrador"; //Session["Rol"] as string;

                if (RolUsuario == "Administrador")
                {

                    respuesta.Resultado = 1;
                    respuesta.Texto = "Cliente no se encuentra registrado, por favor registrar el cliente";

                }
                if (RolUsuario == "Vendedor")
                {

                    respuesta.Resultado = 2;
                    respuesta.Texto = "Cliente no se encuentra registrado, por favor registrar el cliente";

                }
                else
                {

                    respuesta.Resultado = 3;
                    respuesta.Texto = "Cliente no se encuentra registrado, por favor contactar con un administrador.";

                }

            }
            return Json(respuesta);
        }

        public string CalcularConsecutivo()
        {

            T_InvoiceHeader UltimoElemento = db.T_InvoiceHeader.OrderByDescending(x => x.Code).FirstOrDefault();

            string CodigoActual = UltimoElemento.Code;

            string[] Separador = CodigoActual.Split('0');

            string ConsecutivoS = Separador[Separador.Length - 1];

            int ConsecutivoI = Convert.ToInt32(ConsecutivoS);

            string FormatoConsecutivo = string.Format("FAC{0:D10}", ConsecutivoI + 1);

            return FormatoConsecutivo;
        }

        public decimal CalcularPrecioUnitario(string id)
        {

            T_Product t_Product = db.T_Product.Find(id);

           return t_Product.Price;
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