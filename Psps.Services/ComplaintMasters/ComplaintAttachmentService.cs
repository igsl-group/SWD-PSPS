using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Psps.Models.Dto.Complaint;

namespace Psps.Services.ComplaintMasters
{
    /// <summary>
    /// Default implementation of rank service interface
    /// </summary>
    public partial class ComplaintAttachmentService : IComplaintAttachmentService
    {

        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IComplaintAttachmentRepository _complaintAttachmentRepository;

        #endregion Fields

        #region Ctor

        public ComplaintAttachmentService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IComplaintAttachmentRepository complaintAttachmentRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._complaintAttachmentRepository = complaintAttachmentRepository;
        }

        #endregion Ctor

        #region Methods
        public IPagedList<ComplaintAttachment> GetPage(GridSettings grid)
        {
            return _complaintAttachmentRepository.GetPage(grid);
        }
        public void Create(ComplaintAttachment complaintAttachment) 
        {
            Ensure.Argument.NotNull(complaintAttachment, "complaintAttachment");
            _complaintAttachmentRepository.Add(complaintAttachment);
            _eventPublisher.EntityInserted<ComplaintAttachment>(complaintAttachment);
        }

        public void Update(ComplaintAttachment complaintAttachment) 
        {
            Ensure.Argument.NotNull(complaintAttachment, "complaintAttachment");
            _complaintAttachmentRepository.Update(complaintAttachment);
            _eventPublisher.EntityUpdated<ComplaintAttachment>(complaintAttachment);
        }

        public ComplaintAttachment GetById(int id) 
        {
            Ensure.Argument.NotNull(id, "id");
            return _complaintAttachmentRepository.GetById(id);
        }

        public void Delete(ComplaintAttachment complaintAttachment) 
        {
            Ensure.Argument.NotNull(complaintAttachment, "complaintAttachment");
            complaintAttachment.IsDeleted = true;
            _complaintAttachmentRepository.Update(complaintAttachment);
            _eventPublisher.EntityUpdated<ComplaintAttachment>(complaintAttachment);
        }
        #endregion Methods
    }
}