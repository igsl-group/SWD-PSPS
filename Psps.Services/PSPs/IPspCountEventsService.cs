using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.PSPs
{
    public partial interface IPspCountEventsService
    {
        PspCountEvents GetByPspMasterId(int PspMasterId);
    }
}