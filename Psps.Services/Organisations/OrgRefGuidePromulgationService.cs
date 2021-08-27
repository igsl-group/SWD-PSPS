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
    public partial class OrgRefGuidePromulgationService : IOrgRefGuidePromulgationService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IOrgRefGuidePromulgationRepository _orgRefGuidePromulgationRepository;

        #endregion Fields

        #region Ctor

        public OrgRefGuidePromulgationService(IEventPublisher eventPublisher, IOrgRefGuidePromulgationRepository orgRefGuidePromulgationRepository)
        {
            this._eventPublisher = eventPublisher;
            this._orgRefGuidePromulgationRepository = orgRefGuidePromulgationRepository;
        }

        #endregion Ctor

        #region Methods

        public IPagedList<OrgRefGuidePromulgation> GetPage(GridSettings grid)
        {
            return _orgRefGuidePromulgationRepository.GetPage(grid);
        }

        public void CreateOrgRefGuidePromulgation(OrgRefGuidePromulgation model)
        {
            _orgRefGuidePromulgationRepository.Add(model);
            _eventPublisher.EntityInserted<OrgRefGuidePromulgation>(model);
        }

        public void UpdateOrgRefGuidePromulgation(OrgRefGuidePromulgation model)
        {
            _orgRefGuidePromulgationRepository.Update(model);
            _eventPublisher.EntityUpdated<OrgRefGuidePromulgation>(model);
        }

        public OrgRefGuidePromulgation GetOrgRefGuidePromulgationByOrgRefGuidePromulgationId(int orgRefGuidePromulgationId)
        {
            return _orgRefGuidePromulgationRepository.Get(x => x.OrgRefGuidePromulgationId == orgRefGuidePromulgationId);
        }

        #endregion Methods
    }
}