using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface IComplaintMasterSearchViewRepository : IRepository<ComplaintMasterSearchView, int>
    {
        IPagedList<ComplaintMasterSearchView> GetPageByComplaintMasterSearchView(GridSettings grid, bool IsFollowUp, bool FollowUpIndicator, bool ReportPoliceIndicator, bool OtherFollowUpIndicator,
                                                                                 DateTime? followUpFromDate = null, DateTime? followUpToDate = null);
    }

    public class ComplaintMasterSearchViewRepository : BaseRepository<ComplaintMasterSearchView, int>, IComplaintMasterSearchViewRepository
    {
        private readonly IComplaintOtherDepartmentEnquiryRepository _complaintOtherDepartmentEnquiryRepository;
        private readonly IComplaintFollowUpActionRepository _complaintFollowUpActionRepository;
        private readonly IComplaintPoliceCaseRepository _complaintPoliceCaseRepository;

        public ComplaintMasterSearchViewRepository(ISession session, IComplaintOtherDepartmentEnquiryRepository complaintOtherDepartmentEnquiryRepository,
                                                   IComplaintFollowUpActionRepository complaintFollowUpActionRepository, IComplaintPoliceCaseRepository complaintPoliceCaseRepository)
            : base(session)
        {
            this._complaintOtherDepartmentEnquiryRepository = complaintOtherDepartmentEnquiryRepository;
            this._complaintFollowUpActionRepository = complaintFollowUpActionRepository;
            this._complaintPoliceCaseRepository = complaintPoliceCaseRepository;
        }

        public IPagedList<ComplaintMasterSearchView> GetPageByComplaintMasterSearchView(GridSettings grid, bool IsFollowUp, bool FollowUpIndicator, bool ReportPoliceIndicator, bool OtherFollowUpIndicator,
                                                                                        DateTime? followUpFromDate = null, DateTime? followUpToDate = null)
        {
            var query = this.Table;

            // If Checkbox of 'Follow-up Action Taken' checked
            if (IsFollowUp)
            {
                // If no action specified
                if (!FollowUpIndicator && !ReportPoliceIndicator && !OtherFollowUpIndicator)
                {
                    // search all action
                    FollowUpIndicator = true;
                    ReportPoliceIndicator = true;
                    OtherFollowUpIndicator = true;
                }

                if (followUpFromDate != null)
                {
                    query =
                        query.Where(x => x.ComplaintFollowUpAction.Any(f => (FollowUpIndicator && f.ContactDate >= followUpFromDate) ||
                                                                            (ReportPoliceIndicator && (f.VerbalReportDate >= followUpFromDate || f.WrittenReferralDate >= followUpFromDate)) ||
                                                                            (OtherFollowUpIndicator && f.OtherFollowUpContactDate >= followUpFromDate)));
                }

                if (followUpToDate != null)
                {
                    query =
                        query.Where(x => x.ComplaintFollowUpAction.Any(f => (FollowUpIndicator && f.ContactDate <= followUpToDate) ||
                                                                            (ReportPoliceIndicator && (f.VerbalReportDate <= followUpToDate || f.WrittenReferralDate <= followUpToDate)) ||
                                                                            (OtherFollowUpIndicator && f.OtherFollowUpContactDate <= followUpToDate)));
                }
            }

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<ComplaintMasterSearchView>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<ComplaintMasterSearchView>(query, grid.PageIndex, grid.PageSize);

            return page;
        }
    }
}