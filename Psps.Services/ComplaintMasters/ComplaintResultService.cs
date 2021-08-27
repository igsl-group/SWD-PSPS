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
    public partial class ComplaintResultService : IComplaintResultService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IComplaintResultRepository _complaintResultRepository;

        #endregion Fields

        #region Ctor

        public ComplaintResultService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IComplaintResultRepository complaintResultRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._complaintResultRepository = complaintResultRepository;
        }

        public void Create(ComplaintResult complaintResult)
        {
            Ensure.Argument.NotNull(complaintResult, "complaintResult");

            //var disasterMaster = Mapper.Map<DisasterInfoDto, DisasterMaster>(disasterInfoDto);
            Ensure.NotNull(complaintResult, "No Complaint Result found with the specified id");

            _complaintResultRepository.Add(complaintResult);
            _eventPublisher.EntityInserted<ComplaintResult>(complaintResult);
        }

        public void Delete(ComplaintResult complaintResult)
        {
            Ensure.Argument.NotNull(complaintResult, "complaintResult");

            Ensure.NotNull(complaintResult, "No Complaint Result  found with the specified id");
            complaintResult.IsDeleted = true;
            _complaintResultRepository.Update(complaintResult);
            _eventPublisher.EntityDeleted<ComplaintResult>(complaintResult);
        }

        public void Update(ComplaintResult complaintResult)
        {
            Ensure.Argument.NotNull(complaintResult, "No Complaint Result  found with the specified id");

            _complaintResultRepository.Update(complaintResult);
            _eventPublisher.EntityUpdated<ComplaintResult>(complaintResult);
        }

        public IPagedList<ComplaintResult> GetPageByComplaintMasterId(GridSettings Grid, int complaintMasterId)
        {
            return _complaintResultRepository.GetPageByComplaintMasterId(Grid, complaintMasterId);
        }

        public ComplaintResult GetComplaintResultById(int complaintResultId)
        {
            return _complaintResultRepository.GetById(complaintResultId);
        }

        public bool IsDistinctByCmIdAndResult(int complaintMasterId, int? complaintResultId, IList<string> nonComplianceNature)
        {
            var data = _complaintResultRepository.Table.Where(x => x.ComplaintMaster.ComplaintMasterId == complaintMasterId && x.ComplaintResultId != (complaintResultId.HasValue ? complaintResultId.Value : -1)).Select(x => x.NonComplianceNature);

            foreach (var rec in data)
            {
                if (rec.Split(',').Intersect(nonComplianceNature).Any())
                    return false;
            }

            return true;
        }

        #endregion Ctor
    }
}