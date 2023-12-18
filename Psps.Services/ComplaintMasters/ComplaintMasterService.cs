using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Services.Events;
using Psps.Services.UserLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psps.Services.ComplaintMasters
{
    /// <summary>
    /// Default implementation of rank service interface
    /// </summary>
    public partial class ComplaintMasterService : IComplaintMasterService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IComplaintMasterRepository _complaintMasterRepository;
        private readonly IComplaintTelRecordRepository _complaintTelRecordRepository;
        private readonly IComplaintDocSummaryViewRepository _complaintDocSummaryRepository;
        private readonly IComplaintAttachmentRepository _complaintAttachmentRepository;
        private readonly IComplaintFollowUpActionRepository _complaintFollowUpActionRepository;
        private readonly IComplaintPoliceCaseRepository _complaintPoliceCaseRepository;
        private readonly IComplaintResultRepository _complaintResultRepository;
        private readonly IComplaintOtherDepartmentEnquiryRepository _complaintOtherDepartmentEnquiryRepository;
        private readonly IOrgMasterRepository _orgMasterRepository;
        private readonly IComplaintMasterSearchViewRepository _complaintMasterSearchViewRepository;


        #endregion Fields

        #region Ctor

        public ComplaintMasterService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IComplaintMasterRepository complaintMasterRepository, IComplaintTelRecordRepository complaintTelRecordRepository,
            IComplaintAttachmentRepository complaintAttachmentRepository, IComplaintDocSummaryViewRepository complaintDocSummaryRepository,
            IComplaintFollowUpActionRepository complaintFollowUpActionRepository, IComplaintPoliceCaseRepository complaintPoliceCaseRepository,
            IComplaintOtherDepartmentEnquiryRepository complaintOtherDepartmentEnquiryRepository, IOrgMasterRepository orgMasterRepository,
            IComplaintResultRepository complaintResultRepository,
            IComplaintMasterSearchViewRepository complaintMasterSearchViewRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._complaintMasterRepository = complaintMasterRepository;
            this._complaintTelRecordRepository = complaintTelRecordRepository;
            this._complaintAttachmentRepository = complaintAttachmentRepository;
            this._complaintDocSummaryRepository = complaintDocSummaryRepository;
            this._complaintFollowUpActionRepository = complaintFollowUpActionRepository;
            this._complaintPoliceCaseRepository = complaintPoliceCaseRepository;
            this._complaintOtherDepartmentEnquiryRepository = complaintOtherDepartmentEnquiryRepository;
            this._complaintResultRepository = complaintResultRepository;
            this._orgMasterRepository = orgMasterRepository;
            this._complaintMasterSearchViewRepository = complaintMasterSearchViewRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual void Create(ComplaintMaster complaintMaster)
        {
            Ensure.Argument.NotNull(complaintMaster, "complaintMaster");

            _complaintMasterRepository.Add(complaintMaster);
            _eventPublisher.EntityInserted<ComplaintMaster>(complaintMaster);
        }

        public virtual void Update(ComplaintMaster complaintMaster)
        {
            Ensure.Argument.NotNull(complaintMaster, "complaintMaster");
            _complaintMasterRepository.Update(complaintMaster);
            _eventPublisher.EntityUpdated<ComplaintMaster>(complaintMaster);
        }

        public virtual void Update(ComplaintMaster oldComplaintMaster, ComplaintMaster newComplaintMaster)
        {
            Ensure.Argument.NotNull(oldComplaintMaster, "Old Flag Day");
            Ensure.Argument.NotNull(newComplaintMaster, "New Flag Day");            

            _complaintMasterRepository.Update(newComplaintMaster);
            _eventPublisher.EntityUpdated<ComplaintMaster>(newComplaintMaster);
        }

        public virtual IPagedList<ComplaintMaster> GetPage(GridSettings grid)
        {
            return _complaintMasterRepository.GetPage(grid);
        }

        public virtual IPagedList<ComplaintMaster> GetPage(GridSettings grid, string fundRaisingLocation)
        {
            return _complaintMasterRepository.GetPage(grid, fundRaisingLocation);
        }

        public virtual string GenerateComplaintRef(int year)
        {
            return _complaintMasterRepository.GenerateComplaintRef(year);
        }

        public virtual ComplaintMaster GetComplaintMasterById(int complaintMasterId)
        {
            return _complaintMasterRepository.GetById(complaintMasterId);
        }

        public virtual IList<ComplaintMaster> GetRecordsByParam(string param)
        {
            return _complaintMasterRepository.GetRecordsByParam(param);
        }

        public virtual void Delete(ComplaintMaster complaintMaster)
        {
            Ensure.Argument.NotNull(complaintMaster, "complaintMaster");
            complaintMaster.IsDeleted = true;
            _complaintMasterRepository.Update(complaintMaster);
            _eventPublisher.EntityUpdated<ComplaintMaster>(complaintMaster);
        }

        public virtual Hashtable CalEditEnquiryComplaintRelevantRecordsAmount(int complaintMasterId)
        {
            var map = new Hashtable();
            var complaintTelRecordAmt = _complaintTelRecordRepository.Table.Count(a => a.ComplaintMaster.ComplaintMasterId == complaintMasterId);
            var complaintAttachmentAmt = _complaintAttachmentRepository.Table.Count(a => a.ComplaintMaster.ComplaintMasterId == complaintMasterId);
            var complaintDocAmt = _complaintDocSummaryRepository.Table.Count(a => a.Enabled == true);
            var complaintFollowUpActionAmt = _complaintFollowUpActionRepository.Table.Count(a => a.ComplaintMaster.ComplaintMasterId == complaintMasterId);
            var complaintPoliceCaseAmt = _complaintPoliceCaseRepository.Table.Count(a => a.ComplaintMaster.ComplaintMasterId == complaintMasterId);
            var complaintResultAmt = _complaintResultRepository.Table.Count(a => a.ComplaintMaster.ComplaintMasterId == complaintMasterId);
            var complaintOtherDepartmentEnquiryAmt = _complaintOtherDepartmentEnquiryRepository.Table.Count(a => a.ComplaintMaster.ComplaintMasterId == complaintMasterId);
            map.Add("ComplaintTelRecordAmt", complaintTelRecordAmt);
            map.Add("ComplaintDocAmt", complaintDocAmt);
            map.Add("ComplaintAttachmentAmt", complaintAttachmentAmt);
            map.Add("ComplaintFollowUpActionAmt", complaintFollowUpActionAmt);
            map.Add("ComplaintPoliceCaseAmt", complaintPoliceCaseAmt);
            map.Add("ComplaintResultAmt", complaintResultAmt);
            map.Add("ComplaintOtherDepartmentEnquiryAmt", complaintOtherDepartmentEnquiryAmt);
            return map;
        }

        public virtual IPagedList<ComplaintMasterSearchDto> GetPageByComplaintMasterSearchDto(GridSettings grid, bool IsFollowUp, bool FollowUpIndicator, bool ReportPoliceIndicator, bool OthersFollowUpIndicator)
        {
            return _complaintMasterRepository.GetPageByComplaintMasterSearchDto(grid, IsFollowUp, FollowUpIndicator, ReportPoliceIndicator, OthersFollowUpIndicator);
        }

        public virtual IPagedList<ComplaintMasterSearchView> GetPageByComplaintMasterSearchView(GridSettings grid, bool IsFollowUp, bool FollowUpIndicator, bool ReportPoliceIndicator, bool OthersFollowUpIndicator,
                                                                                                DateTime? followUpFromDate = null, DateTime? followUpToDate = null)
        {
            return _complaintMasterSearchViewRepository.GetPageByComplaintMasterSearchView(grid, IsFollowUp, FollowUpIndicator, ReportPoliceIndicator, OthersFollowUpIndicator, followUpFromDate, followUpToDate);
        }


        public ComplaintMaster GetByComplaintRef(string complaintRef)
        {
            return _complaintMasterRepository.GetByComplaintRef(complaintRef);
        }



        #endregion Methods
    }
}