using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Web.Security;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LaFarmacia.Models.viewModel;
using System.Web.Services.Description;

//namespace LaFarmacia.Controllers
//{
    //public class LoginController : Controller
    //{
    //    private LaFarmaciaDBEntities3 db = new LaFarmaciaDBEntities3();

    //    [HttpGet]
    //    public ActionResult Login()
    //    {
    //        return View();
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Login(Login model)
    //    {

    //        System.Data.Entity.Core.Objects.ObjectResult<SP_CheckUser_Result> usr = db.SP_CheckUser(model.UserName, model.Password);

    //        if (ModelState.IsValid) 
    //        {
    //         if (model.UserName == "usuario" && model.Password == "Contraseña") 
    //         {
    //            FormsAuthentication.SetAuthCookie(model.UserName, false);
    //            return RedirectToAction("Index", "Home");   
    //         }
    //         else
    //         {
    //            ViewBag.Valor = 1;
    //            ViewBag.Mensaje = "Usuario o contraseña es incorrecto" ;
    //           return View(model);
    //         }    
    //       }

    //       return View(model);
          
    //    }
    //    public ActionResult Logout() 
    //    {
    //        FormsAuthentication.SignOut();
    //        return RedirectToAction("Index", "Home");
    //    }

//    }
//}
