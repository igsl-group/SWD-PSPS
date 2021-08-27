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
    public partial class ComplaintPoliceCaseService : IComplaintPoliceCaseService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IComplaintPoliceCaseRepository _complaintPoliceCaseRepository;

        #endregion Fields

        #region Ctor

        public ComplaintPoliceCaseService(IEventPublisher eventPublisher,
            IComplaintPoliceCaseRepository complaintPoliceCaseRepository)
        {
            this._eventPublisher = eventPublisher;
            this._complaintPoliceCaseRepository = complaintPoliceCaseRepository;
        }

        #endregion Ctor

        #region Methods

        public IPagedList<ComplaintPoliceCase> GetPage(GridSettings grid)
        {
            return _complaintPoliceCaseRepository.GetPage(grid);
        }

        public void Create(ComplaintPoliceCase complaintPoliceCase)
        {
            Ensure.Argument.NotNull(complaintPoliceCase, "complaintPoliceCase");
            _complaintPoliceCaseRepository.Add(complaintPoliceCase);
            _eventPublisher.EntityInserted<ComplaintPoliceCase>(complaintPoliceCase);
        }

        public void Update(ComplaintPoliceCase complaintPoliceCase)
        {
            Ensure.Argument.NotNull(complaintPoliceCase, "complaintPoliceCase");
            _complaintPoliceCaseRepository.Update(complaintPoliceCase);
            _eventPublisher.EntityUpdated<ComplaintPoliceCase>(complaintPoliceCase);
        }

        public void Delete(ComplaintPoliceCase complaintPoliceCase)
        {
            Ensure.Argument.NotNull(complaintPoliceCase, "complaintPoliceCase");
            complaintPoliceCase.IsDeleted = true;
            _complaintPoliceCaseRepository.Update(complaintPoliceCase);
            _eventPublisher.EntityUpdated<ComplaintPoliceCase>(complaintPoliceCase);
        }

        public ComplaintPoliceCase GetById(int id)
        {
            return _complaintPoliceCaseRepository.GetById(id);
        }

        public string GenerateRefNum(int complaintMasterId)
        {
            string maxRefNum = this._complaintPoliceCaseRepository.GenerateRefNum(complaintMasterId);
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