using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.PSPs
{
    public partial interface IPspAttachmentService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        IPagedList<PspAttachment> GetPage(GridSettings grid);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pspAttachment"></param>
        void Create(PspAttachment pspAttachment);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pspAttachment"></param>
        void Update(PspAttachment pspAttachment);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PspAttachment GetById(int id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pspAttachment"></param>
        void Delete(PspAttachment pspAttachment);

        /// <summary>
        ///
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="pspMasterId"></param>
        /// <returns></returns>
        IPagedList<PspAttachment> GetPageByPspMasterId(GridSettings grid, int pspMasterId);
    }
}