using System.Text;
using System.Web;
using QiBuBlog.Entity;
using QiBuBlog.Util;
using System.Web.Mvc;
using QiBuBlog.Service;


namespace QiBuBlog.WWW
{
    public sealed class SelfOnlyAttribute : FilterAttribute, IAuthorizationFilter, IActionFilter
    {
        private readonly string _parameter;
        private User _currentUser;

        public SelfOnlyAttribute(string parameter = "user")
        {
            this._parameter = parameter;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            if (request.Url == null) return;
            var retUrl = request.Url.AbsoluteUri.ToLower();
            retUrl = string.IsNullOrEmpty(retUrl) ? string.Empty : HttpUtility.UrlEncode(retUrl, Encoding.UTF8);
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _currentUser = FormLoginHelper<User>.Get();
            }
            else
            {
                var area = filterContext.RouteData.DataTokens["area"];
                if (area != null && area.ToString() =="Manage")
                {
                    filterContext.Result = new RedirectResult($"/login?returnUrl={retUrl.ToLower()}");
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.ViewBag.CurrentUser = _currentUser;
            }
            filterContext.Controller.ViewBag.Setup = new SetupService().GetSetup();
            filterContext.Controller.ViewBag.Menu = new MenuService().GetList();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey(_parameter))
            {
                filterContext.ActionParameters[_parameter] = _currentUser;
            }
        }
    }
}