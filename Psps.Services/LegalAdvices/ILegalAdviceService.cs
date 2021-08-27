using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.LegalAdvice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.LegalAdvices
{
    public partial interface ILegalAdviceService
    {

        /// <summary>
        /// List LegalAdvice
        /// </summary>
        /// <param name="grid">GridSettings</param>
        /// <returns>IPagedList</returns>
        IPagedList<LegalAdviceMaster> GetPage(GridSettings grid);


        /// <summary>
        /// Get LegalAdvice Amount By code 
        /// </summary>
        /// <param name="Code">string</param>
        /// <returns>int</returns>
        int GetRelatedLegalAdviceAmountByCode(string code);


        /// <summary>
        /// Create a LegalAdviceMaster
        /// </summary>
        /// <param name="model">LegalAdviceMaster</param>
        void CreateLegalAdviceMaster(LegalAdviceMaster model);

        /// <summary>
        /// Update a LegalAdviceMaster
        /// </summary>
        /// <param name="model">LegalAdviceMaster</param>
        void UpdateLegalAdviceMaster(LegalAdviceMaster model);

        /// <summary>
        /// Get a LegalAdviceMaster 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        LegalAdviceMaster GetLegalAdviceMasterById(int Id);

        /// <summary>
        /// Get All LegalAdviceMaster Records
        /// </summary>
        /// <param name="param">LegalAdviceMasterId param eg : 300,301</param>
        IList<LegalAdviceMaster> GetRecordsByParam(string param);

        /// <summary>
        /// Gets all LegalAdviceMasters for dropdown
        /// </summary>
        /// <returns>LegalAdviceMasters</returns>
        IDictionary<string, string> GetDescriptionForDropdownByType(string LegalAdviceType, string VenueType);


        /// <summary>
        /// Gets all LegalAdviceMasters for dropdown
        /// </summary>
        /// <returns>LegalAdviceMasters</returns>
        IDictionary<string, string> GetDescriptionForDropdownAll();


        /// <summary>
        /// Get GetLegalAdviceCodeSuffix
        /// </summary>
        /// <param name="LegalAdviceCodePrefix">string</param>
        /// <returns>string</returns>
        string GetLegalAdviceCodeSuffix(string LegalAdviceCodePrefix);

        /// <summary>
        /// Get All LegalAdviceMaster Records
        /// </summary>
        /// <param name="param">LegalAdviceMasterId param eg : 300,301</param>
        IList<LegalAdviceMasterDto> GetDtoRecordsByParam(string param);

        /// <summary>
        /// ListByRelatedLegalAdviceID
        /// </summary>
        /// <param name="relatedLegalAdviceID">int</param>
        IList<LegalAdviceMasterDto> ListByRelatedLegalAdviceId(int relatedLegalAdviceID);
    }
}
