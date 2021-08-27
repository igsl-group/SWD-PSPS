using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Organisation
{
    public partial class OrgRefGuidePromulgationXlsFileDto : BaseDto
    {
        public int OrgId { get; set; }

        public string OrgRef { get; set; }

        public string SendDate { get; set; }

        public string PartNum { get; set; }

        public string EnclosureNum { get; set; }
        
        public OrgMaster OrgMaster { get; set; }
    }
}