using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.FlagDays
{
    public partial interface IFdAttachmentService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        IPagedList<FdAttachment> GetPage(GridSettings grid);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdAttachment"></param>
        void Create(FdAttachment fdAttachment);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdAttachment"></param>
        void Update(FdAttachment fdAttachment);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FdAttachment GetById(int id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fdAttachment"></param>
        void Delete(FdAttachment fdAttachment);

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="fdMasterId"></param>
        /// <returns></returns>
        IPagedList<FdAttachment> GetPageByFdMasterId(GridSettings grid, int fdMasterId);
    }
}