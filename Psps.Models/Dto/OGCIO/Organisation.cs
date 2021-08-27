using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.OGCIO
{
    public class Organisation
    {
        public long? OrganisationId { get; set; }

        public string OrganisationNameEnglish { get; set; }

        public string OrganisationNameTChinese { get; set; }

        public string OrganisationNameSChinese { get; set; }

        public string OrganisationUrlEnglish { get; set; }

        public string OrganisationUrlTChinese { get; set; }

        public string OrganisationUrlSChinese { get; set; }

        public string OrganisationAbbreviation { get; set; }

        public int? S88 { get; set; }

        public int Disable { get; set; }
    }
}