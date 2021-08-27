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

namespace Psps.Services.PSPs
{
    public partial class PspAttachmentService : IPspAttachmentService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IPspAttachmentRepository _pspAttachmentRepository;

        #endregion Fields

        #region Ctor

        public PspAttachmentService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IPspAttachmentRepository pspAttachmentRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._pspAttachmentRepository = pspAttachmentRepository;
        }

        #endregion Ctor

        #region Methods

        public IPagedList<PspAttachment> GetPage(GridSettings grid)
        {
            return _pspAttachmentRepository.GetPage(grid);
        }

        public void Create(PspAttachment pspAttachment)
        {
            Ensure.Argument.NotNull(pspAttachment, "pspAttachment");
            _pspAttachmentRepository.Add(pspAttachment);
            _eventPublisher.EntityInserted<PspAttachment>(pspAttachment);
        }

        public void Update(PspAttachment pspAttachment)
        {
            Ensure.Argument.NotNull(pspAttachment, "pspAttachment");
            _pspAttachmentRepository.Update(pspAttachment);
            _eventPublisher.EntityUpdated<PspAttachment>(pspAttachment);
        }

        public PspAttachment GetById(int id)
        {
            Ensure.Argument.NotNull(id, "id");
            return _pspAttachmentRepository.GetById(id);
        }

        public void Delete(PspAttachment pspAttachment)
        {
            Ensure.Argument.NotNull(pspAttachment, "pspAttachment");
            pspAttachment.IsDeleted = true;
            _pspAttachmentRepository.Update(pspAttachment);
            _eventPublisher.EntityUpdated<PspAttachment>(pspAttachment);
        }

        public IPagedList<PspAttachment> GetPageByPspMasterId(GridSettings grid, int pspMasterId)
        {
            return _pspAttachmentRepository.GetPageByPspMasterId(grid, pspMasterId);
        }

        #endregion Methods
    }
}