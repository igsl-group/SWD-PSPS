using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Core.Helper;
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
    public partial class OrgProvisionNotAdoptService : IOrgProvisionNotAdoptService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IOrgProvisionNotAdoptRepository _orgProvisionNotAdoptRepository;

        #endregion Fields

        #region Ctor

        public OrgProvisionNotAdoptService(IEventPublisher eventPublisher, IOrgProvisionNotAdoptRepository orgProvisionNotAdoptRepository)
        {
            this._eventPublisher = eventPublisher;
            this._orgProvisionNotAdoptRepository = orgProvisionNotAdoptRepository;
        }

        #endregion Ctor

        #region Methods
        public void DeleteByGuidePromulgationId(int OrgRefGuidePromulgationId) 
        {
            this._orgProvisionNotAdoptRepository.DeleteByGuidePromulgationId(OrgRefGuidePromulgationId);
        }

        public void CreateOrgProvisionNotAdopt(OrgProvisionNotAdopt orgProvisionNotAdopt) 
        {
            Ensure.Argument.NotNull(orgProvisionNotAdopt, "orgProvisionNotAdopt");
            this._orgProvisionNotAdoptRepository.Add(orgProvisionNotAdopt);
        }
        public IList<OrgProvisionNotAdopt> GetAllByOrgRefGuidePromulgationId(int OrgRefGuidePromulgationId) 
        {
            Ensure.Argument.NotNull(OrgRefGuidePromulgationId, "OrgRefGuidePromulgationId");
            return this._orgProvisionNotAdoptRepository.Table.Where(x => x.OrgRefGuidePromulgationId == OrgRefGuidePromulgationId).ToList();
        }
        #endregion Methods
    }
}