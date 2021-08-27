using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.OGCIO
{
    public class ListOrganisationResult
    {
        public int TotalOrganisations { get; set; }

        public int TotalPages { get; set; }

        public List<Organisation> Organisations { get; set; }
    }
}