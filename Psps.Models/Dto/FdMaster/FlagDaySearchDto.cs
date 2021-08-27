using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.FdMaster
{
    public partial class FlagDaySearchDto : BaseDto
    {
        /// <summary>
        /// grid properties
        /// </summary>

        public int FdMasterId { get; set; }

        public string OrgRef { get; set; }

        public string OrgName { get; set; }

        public string EngOrgNameSorting { get; set; }

        public string EngOrgName { get; set; }

        public string ChiOrgName { get; set; }

        public bool SubventedIndicator { get; set; }

        public string FdRef { get; set; }

        public string PermitNum { get; set; }

        public int WithPermit { get; set; }

        public DateTime? FlagDay { get; set; }

        public string ApplyForTWR { get; set; }

        public string TWR { get; set; }

        public bool? NewApplicantIndicator { get; set; }

        public string ApplicationResult { get; set; }

        /// <summary>
        /// search properties
        /// </summary>
        ///

        public DateTime? ApplicationReceiveDate { get; set; }

        public bool? DisableIndicator { get; set; } // 1 postponed, 0 active

        public string ContactPersonChiName { get; set; }

        public string ContactPersonName { get; set; }

        //public string ContactPersonFirstName { get; set; }

        //public string ContactPersonLastName { get; set; }

        //public string ContactPersonChiFirstName { get; set; }

        //public string ContactPersonChiLastName { get; set; }

        public string TwrDistrict { get; set; } //TwrDistrict

        public string ContactPersonSalute { get; set; }

        public string ContactPersonFirstName { get; set; }

        public string ContactPersonLastName { get; set; }

        public string EngMailingAddress1 { get; set; }

        public string EngMailingAddress2 { get; set; }

        public string EngMailingAddress3 { get; set; }

        public string EngMailingAddress4 { get; set; }
        
        public string EngMailingAddress5 { get; set; }

        public string ChiMailingAddress1 { get; set; }

        public string ChiMailingAddress2 { get; set; }

        public string ChiMailingAddress3 { get; set; }

        public string ChiMailingAddress4 { get; set; }

        public string ChiMailingAddress5 { get; set; }

        public string ContactPersonEmailAddress { get; set; }

        public string EmailAddress { get; set; }

        public string FdYear { get; set; }

        public string FdGroup { get; set; }

        public string UsedLanguage { get; set; }
        
        public string FdLotResult { get; set; }

        public bool? PermitRevokeIndicator { get; set; }

        public bool? ApplyPledgingMechanismIndicator { get; set; }

        public string ApplicationResultInLastYear { get; set; }

        public string LotGroupInLastYear { get; set; }

        public string RefLotGroup { get; set; }

        public int Id
        {
            get
            {
                return FdMasterId;
            }
            set
            {
                FdMasterId = value;
            }
        }
    }
}