using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Complaint;
using Psps.Models.Dto.FdMaster;
using Psps.Models.Dto.FdStatus;
using Psps.Models.Dto.PspMaster;
using Psps.Models.Dto.SuggestionMaster;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Home
{
    public partial class HomeService : IHomeService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IFDMasterRepository _fdMasterRepository;
        private readonly IFdApprovalHistoryRepository _fdApprovalHistoryRepository;
        private readonly ISuggestionMasterRepository _suggestionMasterRepository;
        private readonly IPSPMasterRepository _pspMasterRepository;
        private readonly IComplaintMasterRepository _complaintMasterRepository;
        private readonly IPspApplicationStatusViewRepository _pspApplicationStatusViewRepository;
        private readonly ISsafApplicationStatusViewRepository _ssafApplicationStatusViewRepository;
        private readonly IPspBringUpSummaryViewRepository _pspBringUpSummaryViewRepository;
        private readonly IComplaintBringUpSummaryViewRepository _complaintBringUpSummaryViewRepository;
        
        #endregion Fields

        #region Ctor

        public HomeService(IEventPublisher eventPublisher,
                           IFDMasterRepository fdMasterRepository,
                           IFdApprovalHistoryRepository fdApprovalHistoryRepository,
                           ISuggestionMasterRepository suggestionMasterRepository,
                           IPSPMasterRepository pspMasterRepository,
                           IComplaintMasterRepository complaintMasterRepository,
                           IPspApplicationStatusViewRepository pspApplicationStatusViewRepository,
                           ISsafApplicationStatusViewRepository ssafApplicationStatusViewRepository,
                           IPspBringUpSummaryViewRepository pspBringUpSummaryViewRepository,
                           IComplaintBringUpSummaryViewRepository complaintBringUpSummaryViewRepository)
        {
            this._eventPublisher = eventPublisher;
            this._fdMasterRepository = fdMasterRepository;
            this._fdApprovalHistoryRepository = fdApprovalHistoryRepository;
            this._suggestionMasterRepository = suggestionMasterRepository;
            this._pspMasterRepository = pspMasterRepository;
            this._complaintMasterRepository = complaintMasterRepository;
            this._pspApplicationStatusViewRepository = pspApplicationStatusViewRepository;
            this._ssafApplicationStatusViewRepository = ssafApplicationStatusViewRepository;
            this._pspBringUpSummaryViewRepository = pspBringUpSummaryViewRepository;
            this._complaintBringUpSummaryViewRepository = complaintBringUpSummaryViewRepository;
        }

        #endregion Ctor

        #region Methods

        public IPagedList<FdStatusDto> GetFdSatusSummary(GridSettings grid, int year)
        {
            var fdStatus = _fdMasterRepository.GetFdStatusSummary(year);

            var page = new PagedList<FdStatusDto>(fdStatus, grid.PageIndex, grid.PageSize);

            return page;
        }

        public List<SuggestDto> GetLastFiveYrsSuggestionSummary()
        {
            //grid.SortColumn = null;

            var suggestList = _suggestionMasterRepository.GetLastFiveYrsSuggestionSummary();

            return suggestList;
        }

        public List<CompEnqDto> GetLastFiveYrsCompEnqSummary()
        {
            //grid.SortColumn = null;

            var compEnqList = _complaintMasterRepository.GetLastFiveYrsCompEnqSummary();

            return compEnqList;
        }

        public IPagedList<PspMasterDto> GetPspBringUpSummary(GridSettings grid)
        {
            //grid.SortColumn = null;

            var pspBringUp = _pspMasterRepository.GetPspBringUpSummary();

            var page = new PagedList<PspMasterDto>(pspBringUp, grid.PageIndex, grid.PageSize);

            return page;
        }

        public IPagedList<ComplaintBringUpDto> GetComplaintBringUpSummary(GridSettings grid)
        {
            //grid.SortColumn = null;

            var complaintBringUp = _complaintMasterRepository.GetComplaintBringUpSummary();

            var page = new PagedList<ComplaintBringUpDto>(complaintBringUp, grid.PageIndex, grid.PageSize);

            return page;
        }

        public IPagedList<PspApplicationStatusView> GetPspApplicationStatus(GridSettings grid) 
        {
            return _pspApplicationStatusViewRepository.GetPage(grid);
        }

        public IPagedList<SsafApplicationStatusView> GetSsafApplicationStatus(GridSettings grid)
        {
            return _ssafApplicationStatusViewRepository.GetPage(grid);
        }

        public IPagedList<PspBringUpSummaryView> GetPspBringUpSummaryView(GridSettings grid) 
        {
            return _pspBringUpSummaryViewRepository.GetPage(grid);
        }

        public IPagedList<ComplaintBringUpSummaryView> GetComplaintBringUpSummaryView(GridSettings grid) 
        {
            return _complaintBringUpSummaryViewRepository.GetPage(grid);
        }

        #endregion Methods
    }
}