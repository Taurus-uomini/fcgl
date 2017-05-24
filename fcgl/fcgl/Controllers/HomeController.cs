using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fcgl.Models;
using System.Security.Cryptography;
using System.Text;
using fcgl.App_Start;
using System.Net;
using System.Data.Entity;
using fcgl.function;

namespace fcgl.Controllers
{
    public class HomeController : Controller
    {
        private DBModels db = new DBModels();
        private UserFun userFun = new UserFun();
        public HomeController()
        {
            InfoModels infoModel = db.Info.Find(1);
            ViewBag.infoModel = infoModel;
        }
        public ActionResult Index()
        {
            List<HousePropertyModels> housePropertyList = null;
            if (Session["adminid"] != null)
            {
                AdminModels adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else if (Session["userid"] != null)
            {
                UserModels userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            housePropertyList = db.HousePropertyModels.Where(m => m.status == 2).ToList();
            foreach (HousePropertyModels hm in housePropertyList)
            {
                CitiesModels cm = db.Cities.Where(m => m.cityid.Equals(hm.area.cityid)).FirstOrDefault();
                ProvincesModels pm = db.Provinces.Where(m => m.provinceid.Equals(cm.provinceid)).FirstOrDefault();
                hm.adress = pm.province + cm.city + hm.area.area + hm.adress;
            }
            ViewBag.housePropertyList = housePropertyList;
            return View();
        }
        public ActionResult PropertyDetial(int? id)
        {
            if (Session["adminid"] != null)
            {
                AdminModels adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else if (Session["userid"] != null)
            {
                UserModels userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
                /*if(userFun.isUsersHP(userModels,id))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }*/
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HousePropertyModels hm = db.HousePropertyModels.Find(id);
            if(hm==null)
            {
                return HttpNotFound();
            }
            if(hm.status!=2)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CitiesModels cm = db.Cities.Where(m => m.cityid.Equals(hm.area.cityid)).FirstOrDefault();
            ProvincesModels pm = db.Provinces.Where(m => m.provinceid.Equals(cm.provinceid)).FirstOrDefault();
            hm.adress = pm.province + cm.city + hm.area.area + hm.adress;
            return View(hm);
        }
        public ActionResult Buy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            else
            {
                return RedirectToAction("Login","User");
            }
            if (userFun.isUsersHP(userModels, id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HousePropertyModels hp = db.HousePropertyModels.Find(id);
            OrderModels om = new OrderModels();
            om.status = 0;
            om.startTime = DateTime.Now;
            om.buyer = db.User.Find(Session["userid"]);
            om.seller = hp.owner;
            om.houseProperty = hp;
            db.Order.Add(om);
            hp.status = 3;
            db.HousePropertyModels.Attach(hp);
            db.Entry(hp).Property(m => m.status).IsModified = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}