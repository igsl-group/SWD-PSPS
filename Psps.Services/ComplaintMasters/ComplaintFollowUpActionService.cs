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
    public partial class ComplaintFollowUpActionService : IComplaintFollowUpActionService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IComplaintFollowUpActionRepository _complaintFollowUpActionRepository;

        #endregion Fields

        #region Ctor

        public ComplaintFollowUpActionService(IEventPublisher eventPublisher,
            IComplaintFollowUpActionRepository complaintFollowUpActionRepository)
        {
            this._eventPublisher = eventPublisher;
            this._complaintFollowUpActionRepository = complaintFollowUpActionRepository;
        }

        #endregion Ctor

        #region Methods

        public IPagedList<ComplaintFollowUpAction> GetPage(GridSettings grid)
        {
            return _complaintFollowUpActionRepository.GetPage(grid);
        }

        public void Create(ComplaintFollowUpAction complaintFollowUpAction)
        {
            Ensure.Argument.NotNull(complaintFollowUpAction, "complaintFollowUpAction");
            _complaintFollowUpActionRepository.Add(complaintFollowUpAction);
            _eventPublisher.EntityInserted<ComplaintFollowUpAction>(complaintFollowUpAction);
        }

        public void Update(ComplaintFollowUpAction complaintFollowUpAction)
        {
            Ensure.Argument.NotNull(complaintFollowUpAction, "complaintFollowUpAction");
            _complaintFollowUpActionRepository.Update(complaintFollowUpAction);
            _eventPublisher.EntityUpdated<ComplaintFollowUpAction>(complaintFollowUpAction);
        }

        public void Delete(ComplaintFollowUpAction complaintFollowUpAction)
        {
            Ensure.Argument.NotNull(complaintFollowUpAction, "complaintFollowUpAction");
            complaintFollowUpAction.IsDeleted = true;
            _complaintFollowUpActionRepository.Update(complaintFollowUpAction);
            _eventPublisher.EntityUpdated<ComplaintFollowUpAction>(complaintFollowUpAction);
        }

        public ComplaintFollowUpAction GetById(int id)
        {
            return _complaintFollowUpActionRepository.GetById(id);
        }

        public string GenerateEnclosureNum(int complaintMasterId)
        {
            string maxRefNum = this._complaintFollowUpActionRepository.GenerateEnclosureNum(complaintMasterId);
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