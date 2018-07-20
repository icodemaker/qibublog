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
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();

            var request = filterContext.RequestContext.HttpContext.Request;
            if (request.Url == null) return;
            var retUrl = request.Url.AbsoluteUri.ToLower();
            retUrl = string.IsNullOrEmpty(retUrl) ? string.Empty : System.Web.HttpUtility.UrlEncode(retUrl, System.Text.Encoding.UTF8);
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _currentUser = FormLoginHelper<User>.Get();
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.ViewBag.CurrentUser = _currentUser;
            }
            filterContext.Controller.ViewBag.Setup = SetupService.Instance.GetSetup();
            filterContext.Controller.ViewBag.Menu = MenuService.Instance.GetList();
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