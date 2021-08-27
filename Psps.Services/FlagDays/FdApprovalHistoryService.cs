using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.FdMaster;
using Psps.Models.Dto.FdStatus;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.FlagDays
{
    public partial class FdApprovalHistoryService : IFdApprovalHistoryService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IFdApprovalHistoryRepository _fdApprovalHistoryRepository;

        #endregion Fields

        #region Ctor

        public FdApprovalHistoryService(IEventPublisher eventPublisher, IFdApprovalHistoryRepository fdApprovalHistoryRepository)
        {
            this._eventPublisher = eventPublisher;
            this._fdApprovalHistoryRepository = fdApprovalHistoryRepository;
        }

        #endregion Ctor

        #region Methods

        public IPagedList<FdApprovalHistory> GetPage(GridSettings grid)
        {
            return _fdApprovalHistoryRepository.GetPage(grid);
        }

        public IPagedList<FdStatusSummary> GetFdStatus(GridSettings grid)
        {
            return _fdApprovalHistoryRepository.GetFdStatus(grid);
        }

        public void CreateFdApprovalHistory(FdApprovalHistory fdApproveHist)
        {
            _fdApprovalHistoryRepository.Add(fdApproveHist);
            _eventPublisher.EntityInserted<FdApprovalHistory>(fdApproveHist);
        }

        public FdApprovalHistory GetFdApprovalHistoryById(string fdYear)
        {
            return _fdApprovalHistoryRepository.GetById(fdYear);
        }

        #endregion Methods
    }
}