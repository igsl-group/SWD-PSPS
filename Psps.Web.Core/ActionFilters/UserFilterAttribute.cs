using Psps.Core;
using Psps.Core.Models;
using Psps.Models.Dto.Security;
using System.Web.Mvc;

namespace Psps.Web.Core.ActionFilters
{
    //Inject a ViewBag object to Views for getting information about an authenticated user
    public class UserFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            UserModel userModel;

            if (filterContext.Controller.ViewBag.UserModel == null)
            {
                userModel = new UserModel();
                filterContext.Controller.ViewBag.UserModel = userModel;
            }
            else
            {
                userModel = filterContext.Controller.ViewBag.UserModel as UserModel;
            }

            if (filterContext.HttpContext.User != null
                && filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                IPspsUser user = filterContext.HttpContext.User.GetPspsUser();
                userModel.Name = user.Name;
                userModel.UserId = user.UserId;
                userModel.PostId = user.PostId;
                userModel.OriginalPostIdIfActed = user.OriginalPostIdIfActed;
                userModel.IsAuthenticated = user.IsAuthenticated;
                userModel.IsSysAdmin = user.IsSysAdmin;
            }

            base.OnActionExecuted(filterContext);
        }
    }

    public class UserModel : IPspsUser
    {
        public string Name { get; set; }

        public string UserId { get; set; }

        public string PostId { get; set; }

        public string OriginalPostIdIfActed { get; set; }

        public bool IsAuthenticated { get; set; }

        public bool IsSysAdmin { get; set; }

        public string AuthenticationType
        {
            get { throw new System.NotImplementedException(); }
        }

        public IUserInfo GetUserInfo()
        {
            throw new System.NotImplementedException();
        }
    }
}