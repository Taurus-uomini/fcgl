using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fcgl.function
{
    public class Menu
    {
        private List<MenuItem> userMenu=new List<MenuItem>();
        private List<MenuItem> adminMenu=new List<MenuItem>();
        public Menu()
        {
            MenuItem item = new MenuItem();
            item.name= "用户信息";
            item.url= "/User/showUserInfo";
            item.active= false;
            item.type= 2;
            userMenu.Add(item);
            item = new MenuItem();
            item.name= "房产管理";
            item.url= "/HouseProperty/userPropertyList";
            item.active= false;
            item.type= 2;
            userMenu.Add(item);
            item = new MenuItem();
            item.name = "我的购买";
            item.url = "/User/userBuy";
            item.active = false;
            item.type = 2;
            userMenu.Add(item);
            item = new MenuItem();
            item.name = "我的卖出";
            item.url = "/User/userSell";
            item.active = false;
            item.type = 2;
            userMenu.Add(item);

            item = new MenuItem();
            item.name = "用户管理";
            item.url = "/Admin/UserManage";
            item.active = false;
            item.type = 0;
            adminMenu.Add(item);
            item = new MenuItem();
            item.name = "房产管理";
            item.url = "/Admin/PropertyManage";
            item.active = false;
            item.type = 0;
            adminMenu.Add(item);
            item = new MenuItem();
            item.name = "订单管理";
            item.url = "/Admin/OrderManage";
            item.active = false;
            item.type = 0;
            adminMenu.Add(item);
            item = new MenuItem();
            item.name = "网站设置";
            item.url = "/Admin/WebSet";
            item.active = false;
            item.type = 0;
            adminMenu.Add(item);
            item = new MenuItem();
            item.name = "修改密码";
            item.url = "/Admin/changePassword";
            item.active = false;
            item.type = 0;
            adminMenu.Add(item);
        }
        public List<MenuItem> getUserMenu()
        {
            return userMenu;
        }
        public List<MenuItem> getAdminMenu()
        {
            return adminMenu;
        }
    }
}