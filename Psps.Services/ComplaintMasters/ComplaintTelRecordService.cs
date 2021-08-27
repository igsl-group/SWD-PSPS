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
    public partial class ComplaintTelRecordService : IComplaintTelRecordService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IComplaintTelRecordRepository _complaintTelRecordRepository;

        #endregion Fields

        #region Ctor

        public ComplaintTelRecordService(IEventPublisher eventPublisher,
            IComplaintTelRecordRepository complaintTelRecordRepository)
        {
            this._eventPublisher = eventPublisher;
            this._complaintTelRecordRepository = complaintTelRecordRepository;
        }

        #endregion Ctor

        #region Methods

        public IPagedList<ComplaintTelRecord> GetPage(GridSettings grid)
        {
            return _complaintTelRecordRepository.GetPage(grid);
        }

        public void Create(ComplaintTelRecord complaintTelRecord)
        {
            Ensure.Argument.NotNull(complaintTelRecord, "complaintTelRecord");
            _complaintTelRecordRepository.Add(complaintTelRecord);
            _eventPublisher.EntityInserted<ComplaintTelRecord>(complaintTelRecord);
        }

        public void Update(ComplaintTelRecord complaintTelRecord)
        {
            Ensure.Argument.NotNull(complaintTelRecord, "complaintTelRecord");
            _complaintTelRecordRepository.Update(complaintTelRecord);
            _eventPublisher.EntityUpdated<ComplaintTelRecord>(complaintTelRecord);
        }

        public void Delete(ComplaintTelRecord complaintTelRecord)
        {
            Ensure.Argument.NotNull(complaintTelRecord, "complaintTelRecord");
            complaintTelRecord.IsDeleted = true;
            this.Update(complaintTelRecord);
        }

        public ComplaintTelRecord GetById(int id)
        {
            return _complaintTelRecordRepository.GetById(id);
        }

        public string GenerateTelComplaintRef(int year)
        {
            return _complaintTelRecordRepository.GenerateTelComplaintRef(year); ;
        }

        #endregion Methods
    }
}