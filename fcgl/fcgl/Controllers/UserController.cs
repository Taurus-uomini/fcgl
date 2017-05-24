using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fcgl.Models;
using System.Security.Cryptography;
using System.Text;
using fcgl.App_Start;
using System.Data.Entity;
using fcgl.function;
using System.Net;

namespace fcgl.Controllers
{
    public class UserController : Controller
    {
        private DBModels db = new DBModels();
        private MD5fun md5fun = new MD5fun();
        private Menu menu = new Menu();
        public UserController()
        {
            InfoModels infoModel = db.Info.Find(1);
            ViewBag.infoModel = infoModel;
        }
        public ActionResult Login()
        {
            Session.Clear();
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserModels userModels)
        {
            if(ModelState.IsValid)
            {
                userModels.password = md5fun.getMD5(userModels.password);
                UserModels um = db.User.Where(m => m.username == userModels.username).Where(m => m.password == userModels.password).FirstOrDefault();
                if (um != null)
                {
                    Session["userid"] = um.id;
                    if(Session["buyitem"]!=null)
                    {
                        return RedirectToAction("Index", "PropertyDetial", Session["buyitem"]);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Index");
                    }       
                }
                else
                {
                    ModelState.AddModelError("errorunameorpass", "用户名或密码错误！");
                }
            }
            return View(userModels);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        public ActionResult Register()
        {
            List<ProvincesModels> provincesModelsList = db.Provinces.ToList();
            ViewBag.provincesModelsList = provincesModelsList;
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserModels userModels)
        {
            if(ModelState.IsValid)
            {
                string repassword = Request["repassword"];
                if (repassword.Equals(userModels.password))
                {
                    if(db.User.Where(m => m.username == userModels.username).FirstOrDefault()==null)
                    {
                        if (Request["areaid"] != null)
                        {
                            int areaid = int.Parse(Request["areaid"]);
                            userModels.password = md5fun.getMD5(userModels.password);
                            AreasModels areasModels = db.Areas.Find(areaid);
                            userModels.area = areasModels;
                            db.User.Add(userModels);
                            db.SaveChanges();
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            ModelState.AddModelError("areaid", "地址未选全！");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("username","用户名'"+userModels.username+"'已被注册！");
                    }
                }
                else
                {
                    ModelState.AddModelError("repasserror", "两次密码不相同！");
                }
            }
            List<ProvincesModels> provincesModelsList = db.Provinces.ToList();
            ViewBag.provincesModelsList = provincesModelsList;
            return View(userModels);
        }
        [Login("user")]
        public ActionResult showUserInfo()
        {
            List<MenuItem> menuList = menu.getUserMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/User/showUserInfo"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            if (Session["userid"] != null)
            {
                UserModels userModels = db.User.Find(Session["userid"]);
                CitiesModels cm = db.Cities.Where(m => m.cityid.Equals(userModels.area.cityid)).FirstOrDefault();
                ProvincesModels pm = db.Provinces.Where(m => m.provinceid.Equals(cm.provinceid)).FirstOrDefault();
                userModels.adress = pm.province + cm.city + userModels.area.area + userModels.adress;
                ViewBag.userModels = userModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [Login("user")]
        public ActionResult editUserInfo()
        {
            List<MenuItem> menuList = menu.getUserMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/User/showUserInfo"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            List<ProvincesModels> provincesModelsList = db.Provinces.ToList();
            ViewBag.provincesModelsList = provincesModelsList;
            AreasModels areasModels = db.Areas.Find(userModels.area.id);
            CitiesModels citiesModels = db.Cities.Where(m => m.cityid == areasModels.cityid).FirstOrDefault();
            ProvincesModels provincesModels = db.Provinces.Where(m => m.provinceid == citiesModels.provinceid).FirstOrDefault();
            List<CitiesModels> citiesModelsList = db.Cities.Where(m => m.provinceid == provincesModels.provinceid).ToList();
            List<AreasModels> areasModelsList = db.Areas.Where(m => m.cityid == citiesModels.cityid).ToList();
            ViewBag.citiesModelsList = citiesModelsList;
            ViewBag.areasModelsList = areasModelsList;
            ViewBag.citiesModels = citiesModels;
            ViewBag.provincesModels = provincesModels;
            return View(userModels);
        }
        [HttpPost]
        [Login("user")]
        public ActionResult editUserInfo(UserModels userModels)
        {
            List<MenuItem> menuList = menu.getUserMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/User/showUserInfo"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            ViewBag.userModels = userModels;
            if(ModelState.IsValid)
            {
                if (Request["areaid"] != null)
                {
                    int areaid = int.Parse(Request["areaid"]);
                    db.Entry(userModels).State = EntityState.Modified;
                    db.Database.ExecuteSqlCommand("update UserModels set area_id=" + areaid + " where id=" + userModels.id);
                    db.SaveChanges();
                    return RedirectToAction("showUserInfo");
                }
                else
                {
                    ModelState.AddModelError("areaid", "地址未选全！");
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
            userModels.area = areasModels;
            return View(userModels);
        }
        [Login("user")]
        public ActionResult userBuy()
        {
            List<MenuItem> menuList = menu.getUserMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/User/userBuy"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            List<OrderModels> OrderList = db.Order.Where(m => m.buyer.id == userModels.id).ToList();
            ViewBag.OrderList = OrderList;
            return View();
        }
        [Login("user")]
        [HttpPost]
        public ActionResult AjaxCancel(int? id)
        {
            Dictionary<string, int> d = new Dictionary<string, int>();
            if (id == null)
            {
                d.Add("ret", -1);
                return Json(d);
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
                return Json(d);
            }
            OrderModels om = db.Order.Find(id);
            HousePropertyModels hm = om.houseProperty;
            om.status = 2;
            om.cancelTime = DateTime.Now;
            db.Order.Attach(om);
            db.Entry(om).Property(m => m.status).IsModified = true;
            db.Entry(om).Property(m => m.cancelTime).IsModified = true;
            hm.status = 2;
            db.HousePropertyModels.Attach(hm);
            db.Entry(hm).Property(m => m.status).IsModified = true;
            db.SaveChanges();
            d.Add("ret", 1);
            return Json(d);
        }
        [Login("user")]
        [HttpPost]
        public ActionResult AjaxBuy(int? id)
        {
            Dictionary<string, int> d = new Dictionary<string, int>();
            if (id == null)
            {
                d.Add("ret", -1);
                return Json(d);
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
                return Json(d);
            }
            OrderModels om = db.Order.Find(id);
            HousePropertyModels hm = om.houseProperty;
            om.status = 3;
            db.Order.Attach(om);
            db.Entry(om).Property(m => m.status).IsModified = true;
            db.HousePropertyModels.Attach(hm);
            db.SaveChanges();
            d.Add("ret", 1);
            return Json(d);
        }
        [Login("user")]
        public ActionResult userSell()
        {
            List<MenuItem> menuList = menu.getUserMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/User/userSell"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            List<OrderModels> OrderList = db.Order.Where(m => m.seller.id == userModels.id).ToList();
            ViewBag.OrderList = OrderList;
            return View();
        }
        [Login("user")]
        public ActionResult Finish(int? id)
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
                return RedirectToAction("Login");
            }
            OrderModels om = db.Order.Find(id);
            HousePropertyModels hm = om.houseProperty;
            om.status = 1;
            om.finishTime = DateTime.Now;
            db.Order.Attach(om);
            db.Entry(om).Property(m => m.status).IsModified = true;
            db.Entry(om).Property(m => m.finishTime).IsModified = true;
            hm.status = 1;
            db.HousePropertyModels.Attach(hm);
            db.Entry(hm).Property(m => m.status).IsModified = true;
            db.Database.ExecuteSqlCommand("update HousePropertyModels set owner_id="+om.buyer.id+" where id="+hm.id);
            db.SaveChanges();
            return RedirectToAction("userSell");
        }
        [Login("user")]
        public ActionResult changePassword()
        {
            List<MenuItem> menuList = menu.getUserMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/User/showUserInfo"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View(userModels);
        }
        [HttpPost,ActionName("changePassword")]
        [Login("user")]
        public ActionResult doChangePassword()
        {
            List<MenuItem> menuList = menu.getUserMenu();
            foreach (MenuItem item in menuList)
            {
                if (item.url.Equals("/User/showUserInfo"))
                {
                    item.active = true;
                }
            }
            ViewBag.menuList = menuList;
            UserModels userModels = new UserModels();
            if (Session["userid"] != null)
            {
                userModels = db.User.Find(Session["userid"]);
                ViewBag.userModels = userModels;
            }
            else
            {
                return RedirectToAction("Login");
            }
            string passwordold = Request["passwordold"];
            string passwordnew = Request["passwordnew"];
            string passwordnewconfg = Request["passwordnewconfg"];
            if(passwordold==null||passwordold.Equals(""))
            {
                ModelState.AddModelError("passwordold", "原密码不能为空");
            }
            else if(passwordnew == null || passwordnew.Equals(""))
            {
                ModelState.AddModelError("passwordnew", "新密码不能为空");
            }
            else if (passwordnewconfg == null || passwordnewconfg.Equals(""))
            {
                ModelState.AddModelError("passwordnewconfg", "确定密码不能为空");
            }
            else if(!passwordnew.Equals(passwordnewconfg))
            {
                ModelState.AddModelError("passwordnewconfg", "新密码和确定密码不一致");
            }
            else
            {
                passwordold = md5fun.getMD5(passwordold);
                if (!passwordold.Equals(userModels.password))
                {
                    ModelState.AddModelError("passwordold", "原密码错误");
                }
                else
                {
                    userModels.password = md5fun.getMD5(passwordnew);
                    db.Entry(userModels).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Logout");
                }
            }            
            return View(userModels);
        }
    }
}