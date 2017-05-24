using System.Web;
using System.Web.Mvc;

namespace fcgl.App_Start
{
    public class LoginAttribute: AuthorizeAttribute
    {
        private string logintype;
        public LoginAttribute(string logintype)
        {
            this.logintype = logintype;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if(logintype.Equals("admin"))
            {
                object adminid = httpContext.Session["adminid"];
                if (adminid != null)
                {
                    return true;
                }
            }
            else if(logintype.Equals("user"))
            {
                object userid = httpContext.Session["userid"];
                if (userid != null)
                {
                    return true;
                }
            }
            return base.AuthorizeCore(httpContext);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if(logintype.Equals("admin"))
            {
                filterContext.Result = new RedirectResult("/Admin/Login");
            }
            else if(logintype.Equals("user"))
            {
                filterContext.Result = new RedirectResult("/User/Login");
            }
        }
    }
}