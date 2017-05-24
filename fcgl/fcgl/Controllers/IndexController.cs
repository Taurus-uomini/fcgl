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
    public class IndexController : Controller
    {
        private DBModels db = new DBModels();
        private UserFun userFun = new UserFun();
        public IndexController()
        {
            InfoModels infoModel = db.Info.Find(1);
            ViewBag.infoModel = infoModel;
        }
        public ActionResult Index()
        {
            string id = Request["id"];
            int num = Request["num"]==null?0:int.Parse(Request["num"]);
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
            if(num==0)
            {
                housePropertyList = db.HousePropertyModels.Where(m => m.status == 2).ToList();
            }
            else if(num==1)
            {
                var array=db.Cities.Where(m => m.provinceid.Equals(id)).Select(value => value.cityid).ToArray();
                housePropertyList = db.HousePropertyModels.Where(m => m.status == 2).Where(m=> array.Contains(m.area.cityid)).ToList();
            }
            else if (num == 2)
            {
                housePropertyList = db.HousePropertyModels.Where(m => m.status == 2).Where(m=>m.area.cityid.Equals(id)).ToList();
            }
            else if (num == 3)
            {
                housePropertyList = db.HousePropertyModels.Where(m => m.status == 2).Where(m=>m.area.areaid.Equals(id)).ToList();
            }
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
            if (hm == null)
            {
                return HttpNotFound();
            }
            if (hm.status != 2)
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
            Dictionary<string, int> d = new Dictionary<string, int>();
            if (id == null)
            {
                d.Add("ret", -1);
                return Json(d,JsonRequestBehavior.AllowGet);
            }
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            else
            {
                d.Add("ret", -2);
                return Json(d, JsonRequestBehavior.AllowGet);
            }
            if (userFun.isUsersHP(userModels, id))
            {
                d.Add("ret", -3);
                return Json(d, JsonRequestBehavior.AllowGet);
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
            d.Add("ret", 1);
            return Json(d, JsonRequestBehavior.AllowGet);
        }
    }
}