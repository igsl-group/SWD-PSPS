using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.PSPs
{
    public partial class PspApprovalHistoryService : IPspApprovalHistoryService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IPspApprovalHistoryRepository _pspApprovalHistoryRepository;

        private readonly IPSPMasterRepository _pspMasterRepository;

        #endregion Fields

        #region Ctor

        public PspApprovalHistoryService(IEventPublisher eventPublisher, IPspApprovalHistoryRepository pspApprovalHistoryRepository, IPSPMasterRepository pspMasterRepository)
        {
            this._eventPublisher = eventPublisher;
            this._pspApprovalHistoryRepository = pspApprovalHistoryRepository;
            this._pspMasterRepository = pspMasterRepository;
        }

        #endregion Ctor

        #region Methods

        public IPagedList<PspApprovalHistory> GetPage(GridSettings grid)
        {
            return _pspApprovalHistoryRepository.GetPage(grid);
        }

        public void CreatePspApprovalHistory(PspApprovalHistory model)
        {
            _pspApprovalHistoryRepository.Add(model);
            _eventPublisher.EntityInserted<PspApprovalHistory>(model);
        }

        public void UpdatePspApprovalHistory(PspApprovalHistory model)
        {
            _pspApprovalHistoryRepository.Update(model);
            _eventPublisher.EntityUpdated<PspApprovalHistory>(model);
        }

        public void Delete(PspApprovalHistory model)
        {
            _pspApprovalHistoryRepository.Delete(model);
            _eventPublisher.EntityDeleted<PspApprovalHistory>(model);
        }

        public PspApprovalHistory getPspApprovalHistoryById(int id)
        {
            return _pspApprovalHistoryRepository.GetById(id);
        }

        public PspMaster getPspMasterByApprHistId(int id)
        {
            var pspMastId = from u in _pspApprovalHistoryRepository.Table.Where(u => u.PspApprovalHistoryId == id)
                            select u.PspMaster.PspMasterId;
            var pspMaster = _pspMasterRepository.GetById(Convert.ToInt32(pspMastId.First()));
            return pspMaster;
        }

        public PspApprovalHistory GetPspApprovalHistByPermitNo(string permitNo)
        {
            return GetPspApprovalHistByPermitNo(permitNo);
        }

        public string GetNewCaseMaxSeq(string pspYear)
        {
            return _pspApprovalHistoryRepository.GetNewCaseMaxSeq(pspYear);
        }

        public string GetOldCaseMaxSeq(int prevPspMasterId, string pspYear)
        {
            return _pspApprovalHistoryRepository.GetOldCaseMaxSeq(prevPspMasterId, pspYear);
        }

        public int ifContainTwoBatchRec(int pspMasterId)
        {
            return (_pspApprovalHistoryRepository.Table.Where(x => x.ApprovalType == "TW" && x.PspMaster.PspMasterId == pspMasterId)).ToList().Count();
        }

        public bool HasRecomAprovRecs(int pspMasterId)
        {
            int recordCount = _pspApprovalHistoryRepository.Table.Where(x => x.PspMaster.PspMasterId == pspMasterId).ToList().Count();

            return recordCount > 0;
        }

        public bool HasCancelledRecs(int pspMasterId)
        {
            int recordCount = _pspApprovalHistoryRepository.Table.Where(x => x.PspMaster.PspMasterId == pspMasterId && x.ApprovalStatus == "CA").ToList().Count();

            return recordCount > 0;
        }

        public IPagedList<PspApprovalHistorySummaryDto> GetPspApprovHistSummary(GridSettings grid, int pspMasterId)
        {
            return _pspApprovalHistoryRepository.GetPspApprovHistSummary(grid, pspMasterId);
        }

        public bool ifPspMasterIsApproved(int? pspMasterId)
        {
            return _pspApprovalHistoryRepository.Table.Any(x => x.PspMaster.PspMasterId == pspMasterId);
        }

        #endregion Methods
    }
}