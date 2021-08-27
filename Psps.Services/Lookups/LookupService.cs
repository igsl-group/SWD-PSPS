using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Lookups;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Lookups
{
    /// <summary>
    /// Default implementation of lookup service interface
    /// </summary>
    public partial class LookupService : ILookupService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly ILookupRepository _lookupRepository;

        #endregion Fields

        #region Ctor

        public LookupService(ICacheManager cacheManager, IEventPublisher eventPublisher, ILookupRepository lookupRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._lookupRepository = lookupRepository;
        }

        #endregion Ctor

        #region Methods

        public virtual void DeleteLookup(Lookup lookup)
        {
            Ensure.Argument.NotNull(lookup, "lookup");

            lookup.IsDeleted = true;
            UpdateLookup(lookup);
        }

        public virtual IPagedList<Lookup> GetPage(String lookupType, GridSettings grid)
        {
            grid.AddDefaultRule(new Rule()
            {
                field = "Type",
                data = lookupType,
                op = WhereOperation.Equal.ToEnumValue()
            });

            if (lookupType == LookupType.NoSolicitationDate.ToEnumValue())
            {
                var query = _lookupRepository.Table;

                //filtring
                if (grid.IsSearch)
                    query = query.Where(grid.Where);

                if (grid.SortColumn.IsNullOrEmpty() || (grid.SortColumn.IsNotNullOrEmpty() && grid.SortColumn.ToLower() == "code"))
                {
                    if (grid.SortOrder.IsNullOrEmpty() || grid.SortOrder == "desc")
                        query = query.OrderByDescending(x => x.Code.Substring(6, 4)).ThenByDescending(x => x.Code.Substring(3, 2)).ThenByDescending(x => x.Code.Substring(0, 2));
                    else
                        query = query.OrderBy(x => x.Code.Substring(6, 4)).ThenBy(x => x.Code.Substring(3, 2)).ThenBy(x => x.Code.Substring(0, 2));                
                }
                else if (grid.SortColumn.IsNotNullOrEmpty())
                {
                    query = query.OrderBy(grid.SortColumn, grid.SortOrder);
                }

                return new PagedList<Lookup>(query, grid.PageIndex, grid.PageSize);
            }
            else
            {
                return _lookupRepository.GetPage(grid);
            }
        }

        public virtual void CreateLookup(Lookup lookup)
        {
            Ensure.Argument.NotNull(lookup, "lookup");
            var othersLookpu = _lookupRepository.Get(l => l.Type == lookup.Type && l.IsDeleted == false && l.Code == "Others");
            if (IsExistLookup(lookup))
            {
                var downlist = _lookupRepository.GetMany(l => l.Type == lookup.Type && l.DisplayOrder < lookup.DisplayOrder && l.IsDeleted == false && l.Code != "Others").OrderBy(l => l.DisplayOrder);
                int index = 1;
                foreach (var l in downlist)
                {
                    l.DisplayOrder = index;
                    _lookupRepository.Update(l);
                    _eventPublisher.EntityUpdated<Lookup>(l);
                    index++;
                }

                var uplist = _lookupRepository.GetMany(l => l.Type == lookup.Type && l.DisplayOrder >= lookup.DisplayOrder && l.IsDeleted == false && l.Code != "Others").OrderBy(l => l.DisplayOrder);
                var lastDisplayOrder = lookup.DisplayOrder;
                foreach (var l in uplist)
                {
                    l.DisplayOrder = l.DisplayOrder + 1;
                    _lookupRepository.Update(l);
                    _eventPublisher.EntityUpdated<Lookup>(l);
                    lastDisplayOrder = l.DisplayOrder;
                }

                if (othersLookpu != null)
                {
                    if (othersLookpu.DisplayOrder <= lastDisplayOrder)
                    {
                        othersLookpu.DisplayOrder = lastDisplayOrder + 1;
                        _lookupRepository.Update(othersLookpu);
                        _eventPublisher.EntityUpdated<Lookup>(othersLookpu);
                    }
                }
            }
            else
            {
                int? displayOrder = _lookupRepository.Table.Where(l => l.Type == lookup.Type && l.IsDeleted == false && l.Code != "Others").Max(l => (int?)l.DisplayOrder);
                lookup.DisplayOrder = displayOrder.HasValue ? displayOrder.Value + 1 : 1;
            }
            _lookupRepository.Add(lookup);
            _eventPublisher.EntityInserted<Lookup>(lookup);
        }

        public virtual void UpdateLookup(Lookup lookup)
        {
            Ensure.Argument.NotNull(lookup, "lookup");
            var mode = GetLookupById(lookup.LookupId);
            if (!lookup.Code.Equals("Others"))
            {
                if (IsExistLookup(lookup))
                {
                    //比原来的小
                    if (mode.DisplayOrder > lookup.DisplayOrder)
                    {
                        var list = _lookupRepository.GetMany(l => l.LookupId != lookup.LookupId && l.Type == lookup.Type && l.DisplayOrder >= lookup.DisplayOrder && l.IsDeleted == false && l.Code != "Others").OrderBy(l => l.DisplayOrder);
                        int index = lookup.DisplayOrder + 1;
                        foreach (var l in list)
                        {
                            l.DisplayOrder = index;
                            _lookupRepository.Update(l);
                            _eventPublisher.EntityUpdated<Lookup>(l);
                            index++;
                        }
                    }
                    else if (mode.DisplayOrder < lookup.DisplayOrder)
                    {
                        //比原来的大
                        var list = _lookupRepository.GetMany(l => l.LookupId != lookup.LookupId && l.Type == lookup.Type && l.DisplayOrder <= lookup.DisplayOrder && l.IsDeleted == false && l.Code != "Others").OrderBy(l => l.DisplayOrder);
                        int index = 1;
                        foreach (var l in list)
                        {
                            l.DisplayOrder = index;
                            _lookupRepository.Update(l);
                            _eventPublisher.EntityUpdated<Lookup>(l);
                            index++;
                        }
                    }
                }
                else
                {
                    //not exist，set DisplayOrder the max
                    var maxLookup = _lookupRepository.Table.Where(x => x.Type == lookup.Type && x.IsDeleted == false && x.Code != "Others").OrderByDescending(p => p.DisplayOrder).FirstOrDefault();
                    lookup.DisplayOrder = maxLookup.DisplayOrder;
                    var list = _lookupRepository.GetMany(l => l.LookupId != lookup.LookupId && l.Type == lookup.Type && l.DisplayOrder <= maxLookup.DisplayOrder && l.IsDeleted == false && l.Code != "Others").OrderBy(l => l.DisplayOrder);
                    int index = 1;
                    foreach (var l in list)
                    {
                        l.DisplayOrder = index;
                        _lookupRepository.Update(l);
                        _eventPublisher.EntityUpdated<Lookup>(l);
                        index++;
                    }
                }
            }

            if (mode.Type == LookupType.NoSolicitationDate.ToEnumValue())
                mode.Code = lookup.Code;

            mode.EngDescription = lookup.EngDescription;
            mode.ChiDescription = lookup.ChiDescription;
            mode.DisplayOrder = lookup.DisplayOrder;
            mode.Type = lookup.Type;
            mode.IsActive = lookup.IsActive;
            mode.RowVersion = lookup.RowVersion;

            _lookupRepository.Update(mode);
            _eventPublisher.EntityUpdated<Lookup>(mode);
        }

        public virtual Lookup GetLookupById(int lookupId)
        {
            if (lookupId < 1)
                return null;

            return _lookupRepository.GetById(lookupId);
        }

        public bool IsUniqueLookupCode(LookupType lookupType, int lookupId, string lookupCode)
        {
            Ensure.Argument.NotNull(lookupType, "lookupType");
            Ensure.Argument.NotNullOrEmpty(lookupCode, "lookupCode");

            var type = lookupType.GetName();
            return _lookupRepository.Table.Count(l => l.LookupId != lookupId && l.Type == type && l.Code == lookupCode) == 0;
        }

        public int GetDefaultDisplayOrder(LookupType lookupType)
        {
            Ensure.Argument.NotNull(lookupType, "lookupType");

            int? defaultDisplayOrder = _lookupRepository.Table.Where(l => l.Type == lookupType.ToEnumValue()).Max(l => (int?)l.DisplayOrder);

            return defaultDisplayOrder.HasValue ? defaultDisplayOrder.Value + 1 : 1;
        }

        public bool IsUniqueDisplayOrder(LookupType lookupType, int lookupId, int displayOrder)
        {
            Ensure.Argument.NotNull(lookupType, "lookupType");

            var type = lookupType.GetName();
            return _lookupRepository.Table.Count(l => l.LookupId != lookupId && l.Type == type && l.DisplayOrder == displayOrder) == 0;
        }

        public IDictionary<string, string> getAllLookupForDropdownByType(LookupType type)
        {
            Ensure.Argument.NotNull(type, "type");
            string key = string.Format(Constant.LOOKUP_FOR_DROWDROP_KEY, type);
            return this._cacheManager.Get(key, () =>
            {
                return this._lookupRepository.Table
                     .OrderBy(l => l.DisplayOrder)
                     .Where(l => l.Type == type.ToEnumValue())
                     .Select(l => new { Key = l.Code, Value = l.Code + "-" + l.EngDescription })
                     .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        public IDictionary<string, string> getAllLookupChiDescForDropdownByType(LookupType type)
        {
            Ensure.Argument.NotNull(type, "type");
            string key = string.Format(Constant.LOOKUP_FOR_DROWDROP_KEY, type + "chiDesc");

            return this._cacheManager.Get(key, () =>
            {
                return this._lookupRepository.Table
                     .OrderBy(l => l.DisplayOrder)
                     .Where(l => l.Type == type.ToEnumValue())
                     .Select(l => new { Key = l.Code, Value = l.Code + "-" + l.ChiDescription })
                     .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        public IDictionary<string, string> getAllLkpInCodec(LookupType type)
        {
            Ensure.Argument.NotNull(type, "type");
            string key = string.Format(Constant.LOOKUP_FOR_DROWDROP_KEY, type + "Codec");

            return this._cacheManager.Get(key, () =>
            {
                return this._lookupRepository.Table
                     .OrderBy(l => l.DisplayOrder)
                     .Where(l => l.Type == type.ToEnumValue())
                     .Select(l => new { Key = l.Code, Value = l.EngDescription })
                     .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        public IDictionary<string, string> getAllLkpInChiCodec(LookupType type)
        {
            Ensure.Argument.NotNull(type, "type");
            string key = string.Format(Constant.LOOKUP_FOR_DROWDROP_KEY, type + "ChiCodec");

            return this._cacheManager.Get(key, () =>
            {
                return this._lookupRepository.Table
                     .OrderBy(l => l.DisplayOrder)
                     .Where(l => l.Type == type.ToEnumValue())
                     .Select(l => new { Key = l.Code, Value = l.ChiDescription })
                     .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        public IDictionary<string, string> getAllLkpInPlainDec(LookupType type)
        {
            Ensure.Argument.NotNull(type, "type");
            string key = string.Format(Constant.LOOKUP_FOR_DROWDROP_KEY, type + "plainDesc");

            return this._cacheManager.Get(key, () =>
            {
                return this._lookupRepository.Table
                     .OrderBy(l => l.DisplayOrder)
                     .Where(l => l.Type == type.ToEnumValue())
                     .Select(l => new { Key = l.EngDescription, Value = l.EngDescription })
                     .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        public IDictionary<string, string> GetAllLookupByType(LookupType type)
        {
            Ensure.Argument.NotNull(type, "type");
            IDictionary<string, string> list = _lookupRepository.Table
                            .OrderBy(l => l.DisplayOrder)
                            .Where(l => l.Type == type.ToEnumValue() && l.Code != "03")
                            .Select(x => new { x.Code, x.EngDescription })

                            .ToDictionary(ri => ri.Code, rs => "No. of " + rs.EngDescription + " Received");

            if (list != null && list.Count > 0)
            {
                return list;
            }
            return null;
        }

        public IList<Lookup> GetAllLookupListByType(LookupType type)
        {
            return _lookupRepository.Table
                .OrderBy(l => l.DisplayOrder)
                .Where(l => l.Type == type.ToEnumValue()).ToList();
        }

        public bool IsMaxDisplayOrder(LookupType lookupType, string code, int displayOrder)
        {
            bool flag = true;
            var type = lookupType.GetName();
            if (code.Equals("Others"))
            {
                var maxLookup = _lookupRepository.Table.Where(x => x.Type == type && x.IsDeleted == false && x.Code != "Others").OrderByDescending(p => p.DisplayOrder).FirstOrDefault();
                if (maxLookup != null)
                {
                    if (maxLookup.DisplayOrder >= displayOrder)
                    {
                        flag = false;
                    }
                }
            }
            else
            {
                //Check if Code : 'Others' exist, if not exist => no max display order limit
                bool existOthers = _lookupRepository.Table.Where(x => x.Type == type && x.IsDeleted == false && x.Code == "Others").Any();

                if (existOthers)
                {
                    var maxLookup = _lookupRepository.Table.Where(x => x.Type == type && x.IsDeleted == false).OrderByDescending(p => p.DisplayOrder).FirstOrDefault();
                    if (maxLookup != null)
                    {
                        if (maxLookup.DisplayOrder < displayOrder)
                        {
                            flag = false;
                        }
                    }
                }
            }
            return flag;
        }

        public bool IsSolicitDate(DateTime eventDate)
        {
            var strEveDate = eventDate.ToString("d/M/yyyy");
            var strLongEveDate = eventDate.ToString("dd/MM/yyyy");

            var result = _lookupRepository.Table.Any(x => x.Type == LookupType.NoSolicitationDate.ToEnumValue() && (x.Code == strEveDate || x.Code == strLongEveDate));

            return result;
        }

        public bool EveRngChk(DateTime eveStartDt, DateTime eveEndDt)
        {
            return (from u in _lookupRepository.Table
                    where u.Type == LookupType.NoSolicitationDate.ToEnumValue()
                    select u.Code).AsEnumerable()
                                  .Select(x => CommonHelper.ConvertStringToDateTime(x))
                                  .Any(x => eveStartDt <= x && eveEndDt >= x);

            //List<DateTime> dts = new List<DateTime>();

            //for (var x = 0; x < result.Count(); x++)
            //{
            //    DateTime solicitDt = DateTime.ParseExact(result[x], "d/M/yyyy", new CultureInfo("en-US"), DateTimeStyles.None);

            //    if (eveStartDt <= solicitDt && eveEndDt >= solicitDt)
            //    {
            //        return false;
            //    }
            //}

            ////var solictDt = from x in result

            //return true;
        }

        public string GetDescription(LookupType lookupType, string code, string defaultValue = "")
        {
            Ensure.Argument.NotNull(lookupType, "lookupType");
            string type = lookupType.GetName();
            string result = "";

            if (string.IsNullOrEmpty(code))
                return defaultValue;

            //load all records (we know they are cached)
            string key = string.Format(Constant.LOOKUP_BY_TYPE_KEY, lookupType.ToEnumValue());
            var codes = this._cacheManager.Get(key, () =>
            {
                return this._lookupRepository.Table
                     .OrderBy(l => l.DisplayOrder)
                     .Where(l => l.Type == lookupType.ToEnumValue())
                     .Select(l => new { Key = l.Code, Value = l.EngDescription })
                     .ToDictionary(k => k.Key, v => v.Value);
            });

            if (codes.ContainsKey(code))
                result = codes[code];

            if (String.IsNullOrEmpty(result))
            {
                //gradual loading
                string message = _cacheManager.Get(Constant.LOOKUP_BY_TYPE_AND_CODE_KEY.FormatWith(type, code), () =>
                {
                    var query = this._lookupRepository.Table
                                 .Where(l => l.Type == type && l.Code == code)
                                 .Select(l => l.EngDescription);
                    return query.FirstOrDefault();
                });

                if (message != null)
                    result = message;
            }

            if (String.IsNullOrEmpty(result))
            {
                if (!String.IsNullOrEmpty(defaultValue))
                    result = defaultValue;
                else
                    result = code;
            }

            return result;
        }

        private bool IsExistLookup(Lookup lookup)
        {
            return _lookupRepository.Table.Where(l => l.Type == lookup.Type && l.DisplayOrder == lookup.DisplayOrder).Count() != 0;
        }

        #endregion Methods
    }
}