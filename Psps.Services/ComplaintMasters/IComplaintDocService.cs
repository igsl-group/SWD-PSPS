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
using System.Threading.Tasks;

namespace Psps.Services.Complaints
{
    public partial interface IComplaintDocService
    {
        /// <summary>
        /// List ComplaintDocs
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Actings</returns>
        IPagedList<ComplaintDoc> GetPage(GridSettings grid);

        /// <summary>
        /// Get ComplaintDoc Amount By code
        /// </summary>
        /// <param name="Code">string</param>
        /// <returns>int</returns>
        int GetComplaintDocAmount();

        /// <summary>
        /// Create a ComplaintDoc
        /// </summary>
        /// <param name="model">ComplaintDoc</param>
        void CreateComplaintDoc(ComplaintDoc model);

        /// <summary>
        /// Update a ComplaintDoc
        /// </summary>
        /// <param name="model">ComplaintDoc</param>
        void UpdateComplaintDoc(ComplaintDoc model);

        /// <summary>
        /// Get a ComplaintDoc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ComplaintDoc GetComplaintDocById(int id);

        /// <summary>
        /// Get All ComplaintDoc Records
        /// </summary>
        /// <param name="param">ComplaintRef param eg : 300,301</param>
        IList<ComplaintDoc> GetRecordsByParam(string param);

        /// <summary>
        /// Check Active Template ComplaintDoc as deleted
        /// </summary>
        /// <param name="letterId">letterId</param>
        bool CheckActiveTemplate(int letterId);

        /// <summary>
        /// Check Least Template ComplaintDoc as deleted
        /// </summary>
        /// <param name="DocNum">DocNum</param>
        bool CheckLeastTemplate(string docNum);

        void DeleteComplaintDoc(ComplaintDoc complaintDoc);

        IPagedList<ComplaintDoc> GetPage(GridSettings grid, string docNum);

        /// <summary>
        /// Determine the uniqueness of Name
        /// </summary>
        /// <param name="letterId">Letter Id</param>
        /// <param name="name">Name</param>
        /// <returns>unique within the File Name</returns>
        bool IsUniqueComplaintDocNum(int complaintDocId, string docNum);

        /// <summary>
        /// Determine the uniqueness of Version within the same set of Letters
        /// </summary>
        /// <param name="letterId">Letter Id</param>
        /// <param name="name">Name</param>
        /// <param name="version">Version</param>
        /// <returns>unique within the File Name</returns>
        bool IsUniqueComplaintDocVersion(int complaintDocId, string docNum, string version);

        IPagedList<ComplaintDocSummaryView> GetComplaintDocSummaryViewPage(GridSettings grid);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ComplaintDocSummaryView GetCompDocViewById(int id);
    }
}