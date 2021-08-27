using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Reports;
using Psps.Services.Events;
using Psps.Services.UserLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Suggestions
{
    /// <summary>
    /// Default implementation of rank service interface
    /// </summary>
    public partial class SuggestionMasterService : ISuggestionMasterService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISuggestionMasterRepository _suggestionMasterRepository;

        private readonly IUserLogService _userLogService;

        #endregion Fields

        #region Ctor

        public SuggestionMasterService(ICacheManager cacheManager, IEventPublisher eventPublisher, ISuggestionMasterRepository suggestionMasterRepository, IUserLogService userLogService)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._suggestionMasterRepository = suggestionMasterRepository;
            this._userLogService = userLogService;
        }

        #endregion Ctor

        #region Methods

        public virtual IPagedList<SuggestionMaster> GetPage(GridSettings grid)
        {
            return _suggestionMasterRepository.GetPage(grid);
        }

        public void CreateSuggestionMaster(SuggestionMaster model)
        {
            _suggestionMasterRepository.Add(model);
            _eventPublisher.EntityInserted<SuggestionMaster>(model);
        }

        public void UpdateSuggestionMaster(SuggestionMaster model)
        {
            _suggestionMasterRepository.Update(model);
            _eventPublisher.EntityUpdated<SuggestionMaster>(model);
        }

        public void UpdateSuggestionMaster(SuggestionMaster oldSuggestionMaster, SuggestionMaster newSuggestionMaster)
        {
            Ensure.Argument.NotNull(oldSuggestionMaster, "Old Suggestion");
            Ensure.Argument.NotNull(newSuggestionMaster, "New Suggestion");

            _userLogService.LogSuggestionMasterInformation(oldSuggestionMaster, newSuggestionMaster);

            _suggestionMasterRepository.Update(newSuggestionMaster);
            _eventPublisher.EntityUpdated<SuggestionMaster>(newSuggestionMaster);
        }

        public SuggestionMaster GetSuggestionMasterById(int id)
        {
            return _suggestionMasterRepository.GetById(id);
        }

        public virtual IList<SuggestionMaster> GetRecordsByParam(string param)
        {
            return _suggestionMasterRepository.GetRecordsByParam(param);
        }

        public string CreateSuggestionRefNum()
        {
            return _suggestionMasterRepository.CreateSuggestionRefNum();
        }

        private string GeneratedBy()
        {
            var userId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.UserId;
            return userId;
        }

        #region R20

        public System.IO.MemoryStream GenerateR20PDF(String templatePath, DateTime? fromDate, DateTime? toDate)
        {
            IList<R20Dto> data = _suggestionMasterRepository.GenerateR20Report(fromDate, toDate);

            try
            {
                string strFromDate = fromDate.ToString();     // Setting FromDate
                string strToDate = toDate.ToString();         // Setting ToDate
                ReportDocument rd = new ReportDocument();
                rd.Load(templatePath);

                if (data != null && data.GetType().ToString() != "System.String")
                {
                    rd.SetDataSource(data);
                }

                rd.SetParameterValue("GeneratedBy", GeneratedBy());

                if (fromDate.HasValue)
                {
                    rd.SetParameterValue("FromDate", fromDate.Value.ToString("dd/MM/yyyy"));
                }
                else
                {
                    rd.SetParameterValue("FromDate", "");
                }

                if (toDate.HasValue)
                {
                    rd.SetParameterValue("ToDate", toDate.Value.ToString("dd/MM/yyyy"));
                }
                else
                {
                    rd.SetParameterValue("ToDate", "");
                }

                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                return ms;
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.StackTrace);
                return null;
            }
        }

        #endregion R20

        #endregion Methods
    }
}