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
using System.Threading.Tasks;

namespace Psps.Services.Organisations
{
    public partial class OrgAttachmentService : IOrgAttachmentService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IOrgAttachmentRepository _orgAttachmentRepository;

        #endregion Fields

        #region Ctor

        public OrgAttachmentService(ICacheManager cacheManager, IEventPublisher eventPublisher, IOrgAttachmentRepository orgAttachmentRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._orgAttachmentRepository = orgAttachmentRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual IPagedList<OrgAttachment> GetPage(GridSettings grid)
        {
            return _orgAttachmentRepository.GetPage(grid);
        }

        public virtual int GetOrgAttachmentAmountByCode(string code)
        {
            Ensure.Argument.NotNullOrEmpty(code, "code");
            return _orgAttachmentRepository.Table.Count(a => a.OrgMaster.OrgId == Convert.ToInt32(code));
        }

        public virtual void CreateOrgAttachment(OrgAttachment model)
        {
            Ensure.Argument.NotNull(model);
            _orgAttachmentRepository.Add(model);
            _eventPublisher.EntityInserted<OrgAttachment>(model);
        }

        public virtual OrgAttachment GetOrgAttachmentByName(string name)
        {
            Ensure.Argument.NotNullOrEmpty(name, "name");
            var list = _orgAttachmentRepository.Table.Where(a => a.FileName == name).ToList();
            OrgAttachment orgAttachment = null;
            if (list != null && list.Count() > 0)
            {
                orgAttachment = list.First();
            }
            return orgAttachment;
        }

        public virtual OrgAttachment GetOrgAttachmentById(int attachmentId)
        {
            return _orgAttachmentRepository.GetById(attachmentId);
        }

        public virtual void UpdateOrgAttachment(OrgAttachment model)
        {
            Ensure.Argument.NotNull(model);
            _orgAttachmentRepository.Update(model);
            _eventPublisher.EntityUpdated<OrgAttachment>(model);
        }

        public virtual void DeleteOrgAttachment(OrgAttachment model)
        {
            model.IsDeleted = true;
            _orgAttachmentRepository.Update(model);
            _eventPublisher.EntityUpdated<OrgAttachment>(model);
        }

        #endregion Methods
    }
}