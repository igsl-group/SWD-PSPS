using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psps.Services.ComplaintMasters
{
    /// <summary>
    /// Default implementation of rank service interface
    /// </summary>
    public partial class ComplaintOtherDepartmentEnquiryService : IComplaintOtherDepartmentEnquiryService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IComplaintOtherDepartmentEnquiryRepository _complaintOtherDepartmentEnquiryRepository;

        #endregion Fields

        #region Ctor

        public ComplaintOtherDepartmentEnquiryService(IEventPublisher eventPublisher,
            IComplaintOtherDepartmentEnquiryRepository complaintOtherDepartmentEnquiryRepository)
        {
            this._eventPublisher = eventPublisher;
            this._complaintOtherDepartmentEnquiryRepository = complaintOtherDepartmentEnquiryRepository;
        }

        #endregion Ctor

        #region Methods

        public IPagedList<ComplaintOtherDepartmentEnquiry> GetPage(GridSettings grid)
        {
            return _complaintOtherDepartmentEnquiryRepository.GetPage(grid);
        }

        public void Create(ComplaintOtherDepartmentEnquiry complaintOtherDepartmentEnquiry)
        {
            Ensure.Argument.NotNull(complaintOtherDepartmentEnquiry, "complaintOtherDepartmentEnquiry");

            complaintOtherDepartmentEnquiry.RefNum = this.GenerateRefNum();
            _complaintOtherDepartmentEnquiryRepository.Add(complaintOtherDepartmentEnquiry);
            _eventPublisher.EntityInserted<ComplaintOtherDepartmentEnquiry>(complaintOtherDepartmentEnquiry);
        }

        public void Update(ComplaintOtherDepartmentEnquiry complaintOtherDepartmentEnquiry)
        {
            Ensure.Argument.NotNull(complaintOtherDepartmentEnquiry, "complaintOtherDepartmentEnquiry");
            _complaintOtherDepartmentEnquiryRepository.Update(complaintOtherDepartmentEnquiry);
            _eventPublisher.EntityUpdated<ComplaintOtherDepartmentEnquiry>(complaintOtherDepartmentEnquiry);
        }

        public void Delete(ComplaintOtherDepartmentEnquiry complaintOtherDepartmentEnquiry)
        {
            Ensure.Argument.NotNull(complaintOtherDepartmentEnquiry, "complaintOtherDepartmentEnquiry");
            complaintOtherDepartmentEnquiry.IsDeleted = true;
            _complaintOtherDepartmentEnquiryRepository.Update(complaintOtherDepartmentEnquiry);
            _eventPublisher.EntityUpdated<ComplaintOtherDepartmentEnquiry>(complaintOtherDepartmentEnquiry);
        }

        public ComplaintOtherDepartmentEnquiry GetById(int id)
        {
            return _complaintOtherDepartmentEnquiryRepository.GetById(id);
        }

        public string GenerateRefNum()
        {
            string maxRefNum = this._complaintOtherDepartmentEnquiryRepository.GenerateRefNum();
            if (maxRefNum.IsNotNullOrEmpty())
            {
                return (Convert.ToInt32(maxRefNum) + 1).ToString();
            }
            else
            {
                return "1";
            }
        }

        #endregion Methods
    }
}