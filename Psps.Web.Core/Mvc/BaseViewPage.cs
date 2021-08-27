using Psps.Core.Models;
using Psps.Models.Dto.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Psps.Web.Core.Mvc
{
    public abstract class BaseViewPage : WebViewPage
    {
        public virtual IPspsUser CurrentUser
        {
            get { return base.User != null ? base.User.Identity as PspsUser : null; }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual IPspsUser CurrentUser
        {
            get { return base.User != null ? base.User.Identity as PspsUser : null; }
        }
    }
}