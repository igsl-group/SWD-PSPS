using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.FdMaster;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.FlagDays
{
    public partial interface IFdEventService
    {
        FdEvent GetFdEventById(int pspId);

        IPagedList<FdEvent> GetPage(GridSettings grid);

        IPagedList<FdReadEventDto> GetPageByFdMasterId(GridSettings grid, int fdMasterId);

        bool InsertFdEventByImportXls(Stream xlsxStream, int fdMasterId);

        /// <summary>
        /// Get All FdEvent Records
        /// </summary>
        /// <param name="fdMasterId">int</param>
        IList<FdEvent> GetAllByFdMasterId(int fdMasterId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdMasterId"></param>
        /// <returns></returns>
        FdEvent GetEveByFdMasterId(int fdMasterId);

        /// <summary>
        /// List ComplaintFdPermitNumSearchView
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>IPagedList</returns>
        IPagedList<ComplaintFdPermitNumSearchView> GetPageByComplaintFdPermitNumSearchView(GridSettings grid);

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetDay"></param>
        /// <returns></returns>
        int GetFdEventCountByFlagDay(DateTime targetDay);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdEvent"></param>
        void Update(FdEvent fdEvent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdEvent"></param>
        void Create(FdEvent fdEvent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdEvent"></param>
        void Delete(FdEvent fdEvent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="flagDay"></param>
        /// <param name="type"></param>
        /// <param name="district"></param>
        /// <param name="fdYear"></param>
        /// <param name="fdEventId"></param>
        /// <returns></returns>
        bool AvaliableFlagDay(DateTime flagDay, string type, string district, string fdYear, int? fdEventId);

        #region OGCIO FRAS

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdEvent"></param>
        /// <param name="content"></param>
        /// <param name="assignOn"></param>
        /// <returns></returns>
        bool CreateFRAS(FdEvent fdEvent, out string content, DateTime? approvedOn = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdEvent"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        bool UpdateFRAS(FdEvent fdEvent, out string content);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdEvent"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        bool DeleteFRAS(FdEvent fdEvent, out string content);

        #endregion OGCIO FRAS
    }
}