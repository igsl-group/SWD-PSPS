using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Models.Dto.Reports
{
    public class R4MainDto : BaseReportDto
    {
        public int PspYear { get; set; }

        public int PspApply { get; set; }

        public int PspApproval { get; set; }

        public int PspEvent { get; set; }

        public string FdYear { get; set; }

        public int FdApply { get; set; }

        public int FdEvent { get; set; }

        public int PspWritten { get; set; }

        public int PspTelephone { get; set; }

        public int Psp1823 { get; set; }

        public int PspMass { get; set; }

        public int PspDC { get; set; }

        public int PspLegC { get; set; }

        public int PspOther { get; set; }

        public int PspFromPolice { get; set; }

        public int PspUnClass { get; set; }

        public int PspPolice { get; set; }

        public int PspNFA { get; set; }
        
        public int PspConvicted { get; set; }
        
        public int PspVerbalWarning { get; set; }

        public int PspWrittenWarning { get; set; }

        public int PspVerbalAdvice { get; set; }

        public int PspWrittenAdvice { get; set; }

        public int PspNoResult { get; set; }

        #region SSFA
        public int SsafApply { get; set; }

        public int SsafApproval { get; set; }

        public int SsafEvent { get; set; }

        public int SsafWritten { get; set; }

        public int SsafTelephone { get; set; }

        public int Ssaf1823 { get; set; }

        public int SsafMass { get; set; }

        public int SsafDC { get; set; }

        public int SsafLegC { get; set; }

        public int SsafOther { get; set; }

        public int SsafFromPolice { get; set; }

        public int SsafUnClass { get; set; }

        public int SsafPolice { get; set; }

        public int SsafNFA { get; set; }

        public int SsafConvicted { get; set; }

        public int SsafVerbalWarning { get; set; }

        public int SsafWrittenWarning { get; set; }

        public int SsafVerbalAdvice { get; set; }

        public int SsafWrittenAdvice { get; set; }

        public int SsafNoResult { get; set; }
        #endregion

        public int FdWritten { get; set; }

        public int FdTelephone { get; set; }

        public int Fd1823 { get; set; }

        public int FdMass { get; set; }

        public int FdDC { get; set; }

        public int FdLegC { get; set; }

        public int FdOther { get; set; }

        public int FdFromPolice { get; set; }

        public int FdUnClass { get; set; }

        public int FdPolice { get; set; }

        public int FdNFA { get; set; }

        public int FdConvicted { get; set; }

        public int FdVerbalWarning { get; set; }

        public int FdWrittenWarning { get; set; }

        public int FdVerbalAdvice { get; set; }

        public int FdWrittenAdvice { get; set; }

        public int FdNoResult { get; set; }

        public int OtherWritten { get; set; }

        public int OtherTelephone { get; set; }

        public int Other1823 { get; set; }

        public int OtherMass { get; set; }

        public int OtherDC { get; set; }

        public int OtherLegC { get; set; }

        public int OtherOther { get; set; }

        public int OtherFromPolice { get; set; }

        public int OtherUnClass { get; set; }

        public int OtherPolice { get; set; }

        public int OtherNFA { get; set; }

        public int OtherConvicted { get; set; }

        public int OtherVerbalWarning { get; set; }

        public int OtherWrittenWarning { get; set; }

        public int OtherVerbalAdvice { get; set; }

        public int OtherWrittenAdvice { get; set; }

        public int OtherNoResult { get; set; }
    }
}