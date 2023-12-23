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
using Newtonsoft.Json;

namespace LaFarmacia.Controllers
{
    public class FacturaController : Controller
    {
        private LaFarmaciaDBEntities db = new LaFarmaciaDBEntities();

        private List<T_InvoiceDetail> _InvoiceDetailGlobal
        {

            get
            {
                List<T_InvoiceDetail> data = Session["DetallesFactura"] as List<T_InvoiceDetail>;
                if(data == null)
                {
                    data = new List<T_InvoiceDetail>();
                    Session["DetallesFactura"] = data;
                }
                return data;
            }
            set
            {
                Session["DetallesFactura"] = value;
            }

        }

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


                string consecutivo = CalcularConsecutivo();
                t_InvoiceHeader.Code = consecutivo;
                t_InvoiceHeader.Tax = 13;
                t_InvoiceHeader.Discount = 0;
                t_InvoiceHeader.SubTotal = CalcularSubTotal();
                t_InvoiceHeader.Total = CalcularTotal(t_InvoiceHeader.PayMethodTypeId, t_InvoiceHeader.SubTotal);

                if (ModelState.IsValid)
                    {

                        db.T_InvoiceHeader.Add(t_InvoiceHeader);
                        db.SaveChanges();
                        
                        foreach (var item in _InvoiceDetailGlobal)
                        {

                        decimal precioUnitario = CalcularPrecioUnitario(item.ProductCode);

                        T_InvoiceDetail t_InvoiceDetail = new T_InvoiceDetail();
                        t_InvoiceDetail.InvoiceHeaderCode = consecutivo;
                        t_InvoiceDetail.ProductCode = item.ProductCode;
                        t_InvoiceDetail.Count = item.Count;
                        t_InvoiceDetail.UnitPrice = precioUnitario;
                        t_InvoiceDetail.Total = item.Count * precioUnitario;
                        db.T_InvoiceDetail.Add(t_InvoiceDetail);

                        var pro = db.T_Product.Find(item.ProductCode);
                        int cantidadnueva = pro.Count - item.Count;
                        pro.Count = cantidadnueva;

                        db.Entry(pro).State = System.Data.Entity.EntityState.Modified;

                    }

                    db.SaveChanges();


                }

                return RedirectToAction("Index", "Factura");

            }
            
        }

        [HttpPost]
        public ActionResult ValidarCliente(T_InvoiceHeader t_InvoiceHeader)
        {

            Respuesta  respuesta = new Respuesta();

            T_Client client = db.T_Client.Find(t_InvoiceHeader.ClientId);

            if (client == null)
            {

                string RolUsuario = "Administrador"; //Session["Rol"] as string;

                if (RolUsuario == "Administrador")
                {

                    respuesta.Resultado = false;
                    respuesta.Texto = "Cliente no se encuentra registrado, por favor registrar el cliente.";

                }
                else if (RolUsuario == "Vendedor")
                {

                    respuesta.Resultado = false;
                    respuesta.Texto = "Cliente no se encuentra registrado, por favor registrar el cliente.";

                }
                else
                {

                    respuesta.Resultado = false;
                    respuesta.Texto = "Cliente no se encuentra registrado, por favor contactar con un administrador.";

                }

            }
            else
            {

                respuesta.Resultado = true;
                respuesta.Texto = "Cliente si existe.";

            }
            return Json(respuesta);
        }

        [HttpPost]
        public ActionResult ValidarProducto(T_Product t_Product)
        {

            Respuesta respuesta = new Respuesta();

            T_Product t_Productseleccionado = db.T_Product.Find(t_Product.Code);

            if (t_Productseleccionado.State == false)
            {

                respuesta.Resultado = false;
                respuesta.Texto = "El Producto se encuenta agotado.";

            }
            else
            {

                if (t_Productseleccionado.Count >= t_Product.Count)
                {

                    bool ProductoFacturado = true;

                    foreach (var item in _InvoiceDetailGlobal)
                    {
                        if (item.ProductCode == t_Productseleccionado.Code)
                        {
                            ProductoFacturado = false;
                            break;
                        }

                    }

                    if (ProductoFacturado == false)
                    {
                        respuesta.Resultado = false;
                        respuesta.Texto = "El Producto ya se encuentra en la factura.";
                    }
                    else
                    {

                        respuesta.Resultado = true;
                        respuesta.Texto = "El Producto se añadio correctamente.";

                    }

                }
                else
                {

                    respuesta.Resultado = false;
                    respuesta.Texto = "El Producto se encuentra activo, pero se estan solicitando más de los que se encuentran en stock.";

                }

            }

            return Json(respuesta);
        }

        public ActionResult AgregarDetalle(T_InvoiceDetail t_InvoiceDetail)
        {

            _InvoiceDetailGlobal.Add(t_InvoiceDetail);

            List<ProductDTO> productDTOs = new List<ProductDTO>();

            foreach (var item in _InvoiceDetailGlobal)
            {
                T_Product t_Productseleccionado = db.T_Product.Find(item.ProductCode);

                productDTOs.Add(new ProductDTO()
                {
                    Code = t_Productseleccionado.Code,
                    Description = t_Productseleccionado.Description,
                    Count = item.Count,
                    Price = t_Productseleccionado.Price,
                    ProductTypeCode = t_Productseleccionado.ProductTypeCode,
                    State = t_Productseleccionado.State
                });

            }

            return Json(productDTOs);

        }

        public ActionResult CalcularStock(T_Product t_Product)
        {

            Respuesta respuesta = new Respuesta();

            T_Product t_Productseleccionado = db.T_Product.Find(t_Product.Code);

            respuesta.Resultado = true;
            respuesta.Valor = t_Productseleccionado.Count;

            return Json(respuesta);
        }

        public string CalcularConsecutivo()
        {

            string FormatoConsecutivo = "";

            T_InvoiceHeader UltimoElemento = db.T_InvoiceHeader.OrderByDescending(x => x.Code).FirstOrDefault();

            if (UltimoElemento == null)
            {

                FormatoConsecutivo = string.Format("FAC{0:D10}", 1);

            }
            else
            {

                string CodigoActual = UltimoElemento.Code;

                string[] Separador = CodigoActual.Split('0');

                string ConsecutivoS = Separador[Separador.Length - 1];

                int ConsecutivoI = Convert.ToInt32(ConsecutivoS);

                FormatoConsecutivo = string.Format("FAC{0:D10}", ConsecutivoI + 1);

            }

            return FormatoConsecutivo;
        }

        public decimal CalcularSubTotal()
        {

            decimal SubTotal = 0;

            foreach (var item in _InvoiceDetailGlobal)
            {
                T_Product t_Productseleccionado = db.T_Product.Find(item.ProductCode);

                SubTotal = SubTotal + (t_Productseleccionado.Price * item.Count);
            }

            return SubTotal;
        }

        public decimal CalcularTotal(int MetodoPago, decimal Subtotal)
        {

            decimal Total = 0;

            decimal Impuesto = Subtotal * 0.13m;

            Total = Subtotal + Impuesto;

            if (MetodoPago == 2)
            {
                Total = Total + 0.02m;


            }

            return Total;
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
