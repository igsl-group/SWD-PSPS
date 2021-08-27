using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Events;
using Psps.Services.FlagDays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psp.Services.FlagDays
{
    public partial class FdAttachmentService : IFdAttachmentService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IFdAttachmentRepository _fdAttachmentRepository;

        #endregion Fields

        #region Ctor

        public FdAttachmentService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IFdAttachmentRepository fdAttachmentRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._fdAttachmentRepository = fdAttachmentRepository;
        }

        #endregion Ctor

        #region Methods

        public IPagedList<FdAttachment> GetPage(GridSettings grid)
        {
            return _fdAttachmentRepository.GetPage(grid);
        }

        public void Create(FdAttachment fdAttachment)
        {
            Ensure.Argument.NotNull(fdAttachment, "fdAttachment");
            _fdAttachmentRepository.Add(fdAttachment);
            _eventPublisher.EntityInserted<FdAttachment>(fdAttachment);
        }

        public void Update(FdAttachment fdAttachment)
        {
            Ensure.Argument.NotNull(fdAttachment, "fdAttachment");
            _fdAttachmentRepository.Update(fdAttachment);
            _eventPublisher.EntityUpdated<FdAttachment>(fdAttachment);
        }

        public FdAttachment GetById(int id)
        {
            Ensure.Argument.NotNull(id, "id");
            return _fdAttachmentRepository.GetById(id);
        }

        public void Delete(FdAttachment fdAttachment)
        {
            Ensure.Argument.NotNull(fdAttachment, "fdAttachment");
            fdAttachment.IsDeleted = true;
            _fdAttachmentRepository.Update(fdAttachment);
            _eventPublisher.EntityUpdated<FdAttachment>(fdAttachment);
        }

        public IPagedList<FdAttachment> GetPageByFdMasterId(GridSettings grid, int fdMasterId)
        {
            return _fdAttachmentRepository.GetPageByFdMasterId(grid, fdMasterId);
        }

        #endregion Methods
    }
}