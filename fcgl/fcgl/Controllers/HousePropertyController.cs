using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using fcgl.Models;
using fcgl.App_Start;
using System.IO;
using fcgl.function;

namespace fcgl.Controllers
{
    public class HousePropertyController : Controller
    {
        private DBModels db = new DBModels();
        private UserFun userFun = new UserFun();
        private Menu menu = new Menu();
        public HousePropertyController()
        {
            List<MenuItem> menuList = menu.getUserMenu();
            foreach(MenuItem item in menuList)
            {
                if(item.url.Equals("/HouseProperty/userPropertyList"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            InfoModels infoModel = db.Info.Find(1);
            ViewBag.infoModel = infoModel;
        }
        [Login("user")]
        public ActionResult userPropertyList()
        {
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
            foreach(HousePropertyModels hm in userModels.houseProperty)
            {
                CitiesModels cm = db.Cities.Where(m => m.cityid.Equals(hm.area.cityid)).FirstOrDefault();
                ProvincesModels pm = db.Provinces.Where(m => m.provinceid.Equals(cm.provinceid)).FirstOrDefault();
                hm.adress = pm.province + cm.city + hm.area.area + hm.adress;
            }
            return View(userModels.houseProperty);
        }

        // GET: HouseProperty/Details/5
        public ActionResult Details(int? id)
        {
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (!userFun.isUsersHP(userModels, id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HousePropertyModels housePropertyModels = db.HousePropertyModels.Find(id);
            if (housePropertyModels == null)
            {
                return HttpNotFound();
            }
            CitiesModels citiesModels = db.Cities.Where(m => m.cityid == housePropertyModels.area.cityid).FirstOrDefault();
            ProvincesModels provincesModels = db.Provinces.Where(m => m.provinceid == citiesModels.provinceid).FirstOrDefault();
            housePropertyModels.adress = provincesModels.province + citiesModels.city + housePropertyModels.area.area + housePropertyModels.adress;
            return View(housePropertyModels);
        }

        // GET: HouseProperty/Create
        [Login("user")]
        public ActionResult Create()
        {
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
                List<ProvincesModels> provincesModelsList = db.Provinces.ToList();
                ViewBag.provincesModelsList = provincesModelsList;
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        // POST: HouseProperty/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [Login("user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,adress,prize,detial,propertyUrl")] HousePropertyModels housePropertyModels)
        {
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
                List<ProvincesModels> provincesModelsList = db.Provinces.ToList();
                ViewBag.provincesModelsList = provincesModelsList;
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            if (ModelState.IsValid)
            {
                string propertyUrl0 = Request["propertyUrl0"].Trim();
                string propertyUrl1 = Request["propertyUrl1"].Trim();
                string propertyUrl2 = Request["propertyUrl2"].Trim();
                string propertyUrl3 = Request["propertyUrl3"].Trim();
                if (Request["areaid"]!=null)
                {
                    int areaid = int.Parse(Request["areaid"]);
                    if (propertyUrl1 != null&&!propertyUrl1.Equals(""))
                    {
                        housePropertyModels.propertyUrl1 = propertyUrl1;
                        if (propertyUrl2 != null && !propertyUrl2.Equals(""))
                        {
                            housePropertyModels.propertyUrl2 = propertyUrl2;
                            if (propertyUrl3 != null && !propertyUrl3.Equals(""))
                            {
                                housePropertyModels.propertyUrl3 = propertyUrl3;
                                if (propertyUrl0 != null && !propertyUrl0.Equals(""))
                                {
                                    string[] urls = propertyUrl0.Split(',');
                                    foreach(string s in urls)
                                    {
                                        PropertyImageModels pm = new PropertyImageModels();
                                        pm.url = s;
                                        pm.houseProperty = housePropertyModels;
                                        db.propertyimg.Add(pm);
                                    }
                                    housePropertyModels.status = 0;
                                    AreasModels areasModels = db.Areas.Find(areaid);
                                    housePropertyModels.area = areasModels;
                                    housePropertyModels.owner = userModels;
                                    db.HousePropertyModels.Add(housePropertyModels);
                                    db.SaveChanges();
                                    return RedirectToAction("userPropertyList");
                                }
                                else
                                {
                                    ModelState.AddModelError("propertyUrl0", "文件未上传！");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("propertyUrl3", "文件未上传！");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("propertyUrl2", "文件未上传！");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("propertyUrl1", "文件未上传！");
                    }
                }
                else
                {
                    ModelState.AddModelError("areaid", "地址未选全！");
                }
            }

            return View(housePropertyModels);
        }
        [HttpPost]
        public ActionResult AjaxUploadFiles()
        {
            UserModels userModels = new UserModels();
            Dictionary<string, string> d = new Dictionary<string, string>();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
            }
            else
            {
                d.Add("ret", "-1");
                return Json(d);
            }
            HttpPostedFileBase propertyUrl0 = Request.Files["propertyUrl"];
            if (propertyUrl0 != null)
            {
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                string lfileDirectory = "/Uploads/u" + userModels.id + "/" + ts.TotalSeconds;
                string fileDirectory = HttpContext.Server.MapPath(lfileDirectory);
                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }
                string fileName = Path.GetFileName(propertyUrl0.FileName);
                string filePath = Path.Combine(fileDirectory, fileName);
                propertyUrl0.SaveAs(filePath);
                d.Add("ret", "1");
                d.Add("url", lfileDirectory + "/" + fileName);
                return Json(d);
            }
            else
            {
                d.Add("ret", "-2");
                return Json(d);
            }
        }

        [HttpPost]
        public ActionResult AjaxGetProvinceList()
        {
            List<ProvincesModels> provincesModelsList = db.Provinces.ToList();
            return Json(provincesModelsList);
        }

        [HttpPost]
        public ActionResult AjaxGetCityList()
        {
            string provinceid = Request["provinceid"];
            List<CitiesModels> citiesModelsList = db.Cities.Where(m => m.provinceid == provinceid).ToList();
            return Json(citiesModelsList);
        }

        [HttpPost]
        public ActionResult AjaxGetAreaList()
        {
            string cityid = Request["cityid"];
            List<AreasModels> areasModelsList = db.Areas.Where(m => m.cityid == cityid).ToList();
            var var = db.Areas.Where(m => m.cityid == cityid).Select(value => new { value.area, value.areaid, value.id });
            return Json(var);
        }

        [Login("user")]
        // GET: HouseProperty/Edit/5
        public ActionResult Edit(int? id)
        {
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if(!userFun.isUsersHP(userModels,id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HousePropertyModels housePropertyModels = db.HousePropertyModels.Find(id);
            if (housePropertyModels == null)
            {
                return HttpNotFound();
            }
            List<ProvincesModels> provincesModelsList = db.Provinces.ToList();
            ViewBag.provincesModelsList = provincesModelsList;
            AreasModels areasModels = db.Areas.Find(housePropertyModels.area.id);
            CitiesModels citiesModels = db.Cities.Where(m => m.cityid == areasModels.cityid).FirstOrDefault();
            ProvincesModels provincesModels = db.Provinces.Where(m => m.provinceid == citiesModels.provinceid).FirstOrDefault();
            List<CitiesModels> citiesModelsList = db.Cities.Where(m => m.provinceid == provincesModels.provinceid).ToList();
            List<AreasModels> areasModelsList = db.Areas.Where(m => m.cityid == citiesModels.cityid).ToList();
            ViewBag.citiesModelsList = citiesModelsList;
            ViewBag.areasModelsList = areasModelsList;
            ViewBag.citiesModels = citiesModels;
            ViewBag.provincesModels = provincesModels;
            return View(housePropertyModels);
        }

        // POST: HouseProperty/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [Login("user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,adress,prize,detial")] HousePropertyModels housePropertyModels)
        {
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            housePropertyModels.propertyUrl1 = Request["propertyUrl1"];
            housePropertyModels.propertyUrl2 = Request["propertyUrl2"];
            housePropertyModels.propertyUrl3 = Request["propertyUrl3"];
            int oareaid = int.Parse(Request["oareaid"]);
            if (ModelState.IsValid)
            {
                if (Request["areaid"] != null)
                {
                    int areaid = int.Parse(Request["areaid"]);
                    db.Entry(housePropertyModels).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand("update HousePropertyModels set area_id="+areaid+" where id="+housePropertyModels.id);
                    return RedirectToAction("userPropertyList");
                }
                else
                {
                    ModelState.AddModelError("areaid", "地址未选全！");
                }
                
            }
            int aid = 0;
            if(Request["areaid"]==null)
            {
                aid = int.Parse(Request["oareaid"]);
            }
            else
            {
                aid = int.Parse(Request["areaid"]);
            }
            List<ProvincesModels> provincesModelsList = db.Provinces.ToList();
            ViewBag.provincesModelsList = provincesModelsList;
            AreasModels areasModels = db.Areas.Find(aid);
            CitiesModels citiesModels = db.Cities.Where(m => m.cityid == areasModels.cityid).FirstOrDefault();
            ProvincesModels provincesModels = db.Provinces.Where(m => m.provinceid == citiesModels.provinceid).FirstOrDefault();
            List<CitiesModels> citiesModelsList = db.Cities.Where(m => m.provinceid == provincesModels.provinceid).ToList();
            List<AreasModels> areasModelsList = db.Areas.Where(m => m.cityid == citiesModels.cityid).ToList();
            ViewBag.citiesModelsList = citiesModelsList;
            ViewBag.areasModelsList = areasModelsList;
            ViewBag.citiesModels = citiesModels;
            ViewBag.provincesModels = provincesModels;
            housePropertyModels.area = areasModels;
            housePropertyModels.owner = userModels;
            return View(housePropertyModels);
        }

        [Login("user")]
        public ActionResult Sell(int? id)
        {
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (!userFun.isUsersHP(userModels, id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HousePropertyModels hm = db.HousePropertyModels.Find(id);
            if(hm.status!=1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hm.status = 2;
            db.HousePropertyModels.Attach(hm);
            db.Entry(hm).Property(m => m.status).IsModified = true;
            db.SaveChanges();
            return RedirectToAction("userPropertyList");
        }

        [Login("user")]
        // GET: HouseProperty/Delete/5
        public ActionResult Delete(int? id)
        {
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (!userFun.isUsersHP(userModels, id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HousePropertyModels housePropertyModels = db.HousePropertyModels.Find(id);
            if (housePropertyModels == null)
            {
                return HttpNotFound();
            }
            CitiesModels citiesModels = db.Cities.Where(m => m.cityid == housePropertyModels.area.cityid).FirstOrDefault();
            ProvincesModels provincesModels = db.Provinces.Where(m => m.provinceid == citiesModels.provinceid).FirstOrDefault();
            housePropertyModels.adress = provincesModels.province + citiesModels.city + housePropertyModels.area.area + housePropertyModels.adress;
            return View(housePropertyModels);
        }

        // POST: HouseProperty/Delete/5
        [Login("user")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HousePropertyModels housePropertyModels = db.HousePropertyModels.Find(id);
            db.HousePropertyModels.Remove(housePropertyModels);
            db.SaveChanges();
            return RedirectToAction("userPropertyList");
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
