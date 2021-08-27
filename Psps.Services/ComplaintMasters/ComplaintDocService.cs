using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Complaints;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Complaints
{
    /// <summary>
    /// Default implementation of rank service interface
    /// </summary>
    public partial class ComplaintDocService : IComplaintDocService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IComplaintDocRepository _complaintDocRepository;
        private readonly IComplaintDocSummaryViewRepository _complaintDocSummaryViewRepository;

        #endregion Fields

        #region Ctor

        public ComplaintDocService(ICacheManager cacheManager, IEventPublisher eventPublisher, IComplaintDocRepository complaintDocRepository, IComplaintDocSummaryViewRepository complaintDocSummaryViewRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._complaintDocRepository = complaintDocRepository;
            this._complaintDocSummaryViewRepository = complaintDocSummaryViewRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual IPagedList<ComplaintDoc> GetPage(GridSettings grid)
        {
            return _complaintDocRepository.GetPage(grid);
        }

        public virtual int GetComplaintDocAmount()
        {
            return _complaintDocRepository.Table.Count();
        }

        public void CreateComplaintDoc(ComplaintDoc model)
        {
            if (model.DocStatus)
            {
                _complaintDocRepository.ChangeOtherVersionStatus(model);
            }
            _complaintDocRepository.Add(model);
            _eventPublisher.EntityInserted<ComplaintDoc>(model);
        }

        public void UpdateComplaintDoc(ComplaintDoc model)
        {
            if (model.DocStatus)
            {
                _complaintDocRepository.ChangeOtherVersionStatus(model);
            }
            _complaintDocRepository.Update(model);
            _eventPublisher.EntityUpdated<ComplaintDoc>(model);
        }

        public ComplaintDoc GetComplaintDocById(int id)
        {
            return _complaintDocRepository.GetById(id);
        }

        public virtual IList<ComplaintDoc> GetRecordsByParam(string param)
        {
            return _complaintDocRepository.GetRecordsByParam(param);
        }

        public Core.Models.IPagedList<ComplaintDoc> GetPage(Core.JqGrid.Models.GridSettings grid, string docNum)
        {
            return _complaintDocRepository.GetPage(grid, docNum);
        }

        public bool CheckActiveTemplate(int id)
        {
            Ensure.Argument.NotNull(id, "complaintDocId");
            bool isTrue = true;
            if (_complaintDocRepository.Table.Count(s => s.ComplaintDocId == id && s.DocStatus == true) < 1)
            {
                isTrue = false;
            }
            return isTrue;
        }

        public bool CheckLeastTemplate(string docNum)
        {
            bool isTrue = true;
            if (_complaintDocRepository.Table.Count(s => s.DocNum == docNum && s.IsDeleted == false) > 1)
            {
                isTrue = false;
            }
            return isTrue;
        }

        public virtual void DeleteComplaintDoc(ComplaintDoc complaintDoc)
        {
            Ensure.Argument.NotNull(complaintDoc, "complaintDoc");

            complaintDoc.IsDeleted = true;
            UpdateComplaintDoc(complaintDoc);
        }

        public bool IsUniqueComplaintDocNum(int complaintDocId, string docNum)
        {
            Ensure.Argument.NotNull(docNum, "docNum");
            return _complaintDocRepository.Table.Count(l => l.DocNum == docNum && l.ComplaintDocId != complaintDocId) == 0;
        }

        public bool IsUniqueComplaintDocVersion(int complaintDocId, string docNum, string version)
        {
            Ensure.Argument.NotNull(docNum, "docNum");
            return _complaintDocRepository.Table.Count(l => l.DocNum == docNum && l.VersionNum == version && l.ComplaintDocId != complaintDocId) == 0;
        }

        public IPagedList<ComplaintDocSummaryView> GetComplaintDocSummaryViewPage(GridSettings grid)
        {
            return _complaintDocSummaryViewRepository.GetPage(grid);
        }

        public ComplaintDocSummaryView GetCompDocViewById(int id)
        {
            return _complaintDocSummaryViewRepository.GetById(id);
        }

        #endregion Methods
    }
}