using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.LegalAdvice;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.LegalAdvices
{
    public partial class LegalAdviceService : ILegalAdviceService
    {

        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly ILegalAdviceMasterRepository _legalAdviceMasterRepository;

        #endregion Fields

        #region Ctor
        public LegalAdviceService(IEventPublisher eventPublisher, ILegalAdviceMasterRepository legalAdviceMasterRepository
            , ICacheManager cacheManager)
        {
            this._eventPublisher = eventPublisher;
            this._legalAdviceMasterRepository = legalAdviceMasterRepository;
            this._cacheManager = cacheManager;
        }

        #endregion Ctor

        #region Methods

        public virtual IPagedList<LegalAdviceMaster> GetPage(GridSettings grid)
        {
            return _legalAdviceMasterRepository.GetPage(grid);
        }
        public virtual int GetRelatedLegalAdviceAmountByCode(string code) 
        {
            Ensure.Argument.NotNullOrEmpty(code, "code");
            return _legalAdviceMasterRepository.Table.Count(a => a.RelatedLegalAdviceId == Convert.ToInt32(code) && a.IsDeleted==false);
        
        }

        public void CreateLegalAdviceMaster(LegalAdviceMaster model)
        {
            _legalAdviceMasterRepository.Add(model);
            _eventPublisher.EntityInserted<LegalAdviceMaster>(model);
        }

        public void UpdateLegalAdviceMaster(LegalAdviceMaster model)
        {
            _legalAdviceMasterRepository.Update(model);
            _eventPublisher.EntityUpdated<LegalAdviceMaster>(model);
        }

        public LegalAdviceMaster GetLegalAdviceMasterById(int Id)
        {
            return _legalAdviceMasterRepository.GetById(Id);
        }

        public IList<LegalAdviceMasterDto> ListByRelatedLegalAdviceId(int relatedLegalAdviceId) 
        {
            return _legalAdviceMasterRepository.ListByRelatedLegalAdviceId(relatedLegalAdviceId);
        }

        public virtual IList<LegalAdviceMaster> GetRecordsByParam(string param)
        {
            return _legalAdviceMasterRepository.GetRecordsByParam(param);
        }
        public virtual IList<LegalAdviceMasterDto> GetDtoRecordsByParam(string param) 
        {
            return _legalAdviceMasterRepository.GetDtoRecordsByParam(param);
        }
        public IDictionary<string, string> GetDescriptionForDropdownByType(string LegalAdviceType, string VenueType)
        {
            Ensure.Argument.NotNull(LegalAdviceType, "LegalAdviceType");
            string key = string.Format(Constant.LegalAdvice_FOR_DROWDROP_KEY, LegalAdviceType);

            if (VenueType != null && VenueType != "")
                return this._cacheManager.Get(key, () =>
                {
                    return this._legalAdviceMasterRepository.Table
                         .OrderBy(l => l.LegalAdviceDescription)
                         .Where(l => l.LegalAdviceType == LegalAdviceType && l.VenueType == VenueType)
                         .Select(l => new { Key = l.LegalAdviceId.ToString(), Value = l.LegalAdviceDescription })
                         .ToDictionary(k => k.Key, v => v.Value);
                });
            return this._cacheManager.Get(key, () =>
            {
                return this._legalAdviceMasterRepository.Table
                     .OrderBy(l => l.LegalAdviceDescription)
                     .Where(l => l.LegalAdviceType == LegalAdviceType)
                     .Select(l => new { Key = l.LegalAdviceId.ToString(), Value = l.LegalAdviceDescription })
                     .ToDictionary(k => k.Key, v => v.Value);
            });

        }

        public IDictionary<string, string> GetDescriptionForDropdownAll()
        {
            string key = string.Format(Constant.LegalAdvice_FOR_DROWDROP_KEY, "1");
            return this._cacheManager.Get(key, () =>
            {
                return this._legalAdviceMasterRepository.Table
                     .OrderBy(l => l.LegalAdviceDescription)
                     .Select(l => new { Key = l.LegalAdviceId.ToString(), Value = l.LegalAdviceDescription })
                     .ToDictionary(k => k.Key, v => v.Value);
            });
        }
        
        public string GetLegalAdviceCodeSuffix(string LegalAdviceCodePrefix)
        {
            return _legalAdviceMasterRepository.GetLegalAdviceCodeSuffix(LegalAdviceCodePrefix);
        }
        
        #endregion Methods
    }
}
