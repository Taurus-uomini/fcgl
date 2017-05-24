using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fcgl.Models;
using System.Security.Cryptography;
using System.Text;
using fcgl.App_Start;
using fcgl.function;
using System.Net;
using System.Data.Entity;

namespace fcgl.Controllers
{
    public class AdminController : Controller
    {
        private DBModels db = new DBModels();
        private MD5fun md5fun = new MD5fun();
        private Menu menu = new Menu();
        public AdminController()
        {
            InfoModels infoModel = db.Info.AsNoTracking().First();
            ViewBag.infoModel = infoModel;
        }
        [Login("admin")]
        public ActionResult UserManage()
        {
            List<MenuItem> menuList = menu.getAdminMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/Admin/UserManage"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            if (Session["adminid"] != null)
            {
                AdminModels adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            List<UserModels> userList = db.User.ToList();
            ViewBag.userList = userList;
            return View();
        }
        [Login("admin")]
        public ActionResult UserEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<MenuItem> menuList = menu.getAdminMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/Admin/UserManage"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            if (Session["adminid"] != null)
            {
                AdminModels adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            UserModels userInfo = db.User.Find(id);
            List<ProvincesModels> provincesModelsList = db.Provinces.ToList();
            ViewBag.provincesModelsList = provincesModelsList;
            AreasModels areasModels = db.Areas.Find(userInfo.area.id);
            CitiesModels citiesModels = db.Cities.Where(m => m.cityid == areasModels.cityid).FirstOrDefault();
            ProvincesModels provincesModels = db.Provinces.Where(m => m.provinceid == citiesModels.provinceid).FirstOrDefault();
            List<CitiesModels> citiesModelsList = db.Cities.Where(m => m.provinceid == provincesModels.provinceid).ToList();
            List<AreasModels> areasModelsList = db.Areas.Where(m => m.cityid == citiesModels.cityid).ToList();
            ViewBag.citiesModelsList = citiesModelsList;
            ViewBag.areasModelsList = areasModelsList;
            ViewBag.citiesModels = citiesModels;
            ViewBag.provincesModels = provincesModels;
            return View(userInfo);
        }
        [HttpPost]
        [Login("admin")]
        public ActionResult UserEdit(UserModels userInfo)
        {
            List<MenuItem> menuList = menu.getAdminMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/Admin/UserManage"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            if (Session["adminid"] != null)
            {
                AdminModels adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            if (ModelState.IsValid)
            {
                if (Request["areaid"] != null)
                {
                    int areaid = int.Parse(Request["areaid"]);
                    db.Entry(userInfo).State = EntityState.Modified;
                    db.Database.ExecuteSqlCommand("update UserModels set area_id=" + areaid + " where id=" + userInfo.id);
                    db.SaveChanges();
                    return RedirectToAction("UserManage");
                }
            }
            int aid = 0;
            if (Request["areaid"] == null)
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
            userInfo.area = areasModels;
            return View(userInfo);
        }
        [Login("admin")]
        public ActionResult UserDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["adminid"] != null)
            {
                AdminModels adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            UserModels userInfo = db.User.Find(id);
            List<OrderModels> omlist = db.Order.Where(m => m.buyer.id == userInfo.id || m.seller.id == userInfo.id).ToList();
            foreach (OrderModels om in omlist)
            {
                db.Order.Remove(om);
            }
            foreach(HousePropertyModels hm in userInfo.houseProperty.ToList())
            {
                db.HousePropertyModels.Remove(hm);
            }
            db.User.Remove(userInfo);
            db.SaveChanges();
            return RedirectToAction("UserManage");
        }
        [Login("admin")]
        public ActionResult PropertyManage()
        {
            List<MenuItem> menuList = menu.getAdminMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/Admin/PropertyManage"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            if (Session["adminid"] != null)
            {
                AdminModels adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            List<HousePropertyModels> housePropertyList = db.HousePropertyModels.ToList();
            foreach(HousePropertyModels hm in housePropertyList)
            {
                CitiesModels cm = db.Cities.Where(m => m.cityid.Equals(hm.area.cityid)).FirstOrDefault();
                ProvincesModels pm = db.Provinces.Where(m => m.provinceid.Equals(cm.provinceid)).FirstOrDefault();
                hm.adress = pm.province + cm.city + hm.area.area + hm.adress;
            }
            ViewBag.housePropertyList = housePropertyList;
            return View();
        }
        [Login("admin")]
        public ActionResult PropertyDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<MenuItem> menuList = menu.getAdminMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/Admin/PropertyManage"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            if (Session["adminid"] != null)
            {
                AdminModels adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            HousePropertyModels housePropertyModels = db.HousePropertyModels.Find(id);
            CitiesModels citiesModels = db.Cities.Where(m => m.cityid == housePropertyModels.area.cityid).FirstOrDefault();
            ProvincesModels provincesModels = db.Provinces.Where(m => m.provinceid == citiesModels.provinceid).FirstOrDefault();
            housePropertyModels.adress = provincesModels.province + citiesModels.city + housePropertyModels.area.area + housePropertyModels.adress;
            return View(housePropertyModels);
        }
        [Login("admin")]
        public ActionResult PropertyDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HousePropertyModels hm = db.HousePropertyModels.Find(id);
            db.HousePropertyModels.Remove(hm);
            db.SaveChanges();
            return RedirectToAction("PropertyManage");
        }
        [Login("admin")]
        public ActionResult PropertyCheck(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HousePropertyModels hm = db.HousePropertyModels.Find(id);
            hm.status = 1;
            db.HousePropertyModels.Attach(hm);
            db.Entry(hm).Property(m => m.status).IsModified = true;
            db.SaveChanges();
            return RedirectToAction("PropertyManage");
        }
        [Login("admin")]
        public ActionResult OrderManage()
        {
            List<MenuItem> menuList = menu.getAdminMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/Admin/OrderManage"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            if (Session["adminid"] != null)
            {
                AdminModels adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            List<OrderModels> OrderList = db.Order.ToList();
            ViewBag.OrderList = OrderList;
            return View();
        }
        [Login("admin")]
        public ActionResult WebSet()
        {
            List<MenuItem> menuList = menu.getAdminMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/Admin/WebSet"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            if (Session["adminid"] != null)
            {
                AdminModels adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            InfoModels infoModels = db.Info.Find(1);
            return View(infoModels);
        }
        [Login("admin")]
        [HttpPost]
        public ActionResult WebSet(InfoModels infoModels)
        {
            List<MenuItem> menuList = menu.getAdminMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/Admin/WebSet"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            if (Session["adminid"] != null)
            {
                AdminModels adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            if(ModelState.IsValid)
            {
                db.Entry(infoModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Index");
            }
            return View(infoModels);
        }
        public ActionResult Login()
        {
            Session.Clear();
            return View();
        }
        [HttpPost]
        public ActionResult Login(AdminModels adminModels)
        {
            if (ModelState.IsValid)
            {
                adminModels.password = md5fun.getMD5(adminModels.password);
                AdminModels am = db.Admin.Where(m => m.username == adminModels.username).Where(m => m.password == adminModels.password).FirstOrDefault();
                if (am != null)
                {
                    am.ip = Request.UserHostAddress;
                    db.Admin.Attach(am);
                    db.Entry(am).Property(m => m.ip).IsModified = true;
                    db.SaveChanges();
                    Session["adminid"] = am.id;
                    return RedirectToAction("Index", "Index");
                }
                else
                {
                    ModelState.AddModelError("errorunameorpass", "用户名或密码错误！");
                }
            }       
            return View(adminModels);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        [Login("admin")]
        public ActionResult changePassword()
        {
            List<MenuItem> menuList = menu.getAdminMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/Admin/changePassword"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            if (Session["adminid"] != null)
            {
                AdminModels adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View(db.Admin.Find(Session["adminid"]));
        }
        [HttpPost, ActionName("changePassword")]
        [Login("admin")]
        public ActionResult doChangePassword()
        {
            List<MenuItem> menuList = menu.getAdminMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/Admin/changePassword"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            AdminModels adminModels;
            if (Session["adminid"] != null)
            {
                adminModels = db.Admin.Find(Session["adminid"]);
                ViewBag.adminModels = adminModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            string passwordold = Request["passwordold"];
            string passwordnew = Request["passwordnew"];
            string passwordnewconfg = Request["passwordnewconfg"];
            if (passwordold == null || passwordold.Equals(""))
            {
                ModelState.AddModelError("passwordold", "原密码不能为空");
            }
            else if (passwordnew == null || passwordnew.Equals(""))
            {
                ModelState.AddModelError("passwordnew", "新密码不能为空");
            }
            else if (passwordnewconfg == null || passwordnewconfg.Equals(""))
            {
                ModelState.AddModelError("passwordnewconfg", "确定密码不能为空");
            }
            else if (!passwordnew.Equals(passwordnewconfg))
            {
                ModelState.AddModelError("passwordnewconfg", "新密码和确定密码不一致");
            }
            else
            {

                passwordold = md5fun.getMD5(passwordold);
                if (!passwordold.Equals(adminModels.password))
                {
                    ModelState.AddModelError("passwordold", "原密码错误");
                }
                else
                {
                    adminModels.password = md5fun.getMD5(passwordnew);
                    db.Entry(adminModels).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Logout");
                }
            }
            return View(adminModels);
        }
    }
}