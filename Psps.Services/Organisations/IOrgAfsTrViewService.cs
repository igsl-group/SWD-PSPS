using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Organisations
{
    /// <summary>
    /// System message service interface
    /// </summary>
    public partial interface IOrgAfsTrViewService
    {
        IPagedList<OrgAfsTrView> GetPage(GridSettings grid);

        int GetTrViewAmt(string orgId);

        int GetAfsViewAmt(string orgId);
    }
}