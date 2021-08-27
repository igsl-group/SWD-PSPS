using NHibernate;
using Psps.Core;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto.LegalAdvice;
using Psps.Models.Dto.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.Repositories
{
    public interface ILegalAdviceMasterRepository : IRepository<LegalAdviceMaster, int>
    {
        IList<LegalAdviceMaster> GetRecordsByParam(string param);

        IList<LegalAdviceMasterDto> GetDtoRecordsByParam(string param);

        string GetLegalAdviceCodeSuffix(string LegalAdviceCodePrefix);

        IList<LegalAdviceMasterDto> ListByRelatedLegalAdviceId(int relatedLegalAdviceID);
    }

    public class LegalAdviceMasterRepository : BaseRepository<LegalAdviceMaster, int>, ILegalAdviceMasterRepository
    {
        public LegalAdviceMasterRepository(ISession session)
            : base(session)
        {
        }

        public virtual IList<LegalAdviceMasterDto> ListByRelatedLegalAdviceId(int relatedLegalAdviceId)
        {
            string sql = "SELECT lam.LegalAdviceDescription, lam.PartNum, lam.EnclosureNum, lam.EffectiveDate, l.EngDescription AS RequirePspIndicator, lam.Remarks " + Environment.NewLine +
                         "FROM LegalAdviceMaster lam " + Environment.NewLine +
                         "LEFT JOIN Lookup l ON l.Type='" + LookupType.PSPRequiredIndicator.ToEnumValue() + "' AND l.Code=lam.RequirePspIndicator AND l.IsDeleted = 0 " + Environment.NewLine +
                         "WHERE lam.IsDeleted = 0 ";

            if (relatedLegalAdviceId != 0)
                sql = sql + "AND lam.RelatedLegalAdviceId='" + relatedLegalAdviceId + "' ";

            sql = sql + Environment.NewLine + "ORDER BY lam.LegalAdviceId desc";

            IQuery sqlQuery = this.Session.CreateSQLQuery(sql);
            IList<object[]> list = sqlQuery.List<object[]>();
            IList<LegalAdviceMasterDto> resultList = new List<LegalAdviceMasterDto>();
            for (var i = 0; i < list.Count; i++)
            {
                var dto = new LegalAdviceMasterDto();
                object[] obj = list[i];
                dto.LegalAdviceDescription = Convert.ToString(obj[0]);
                dto.PartNum = Convert.ToString(obj[1]);
                dto.EnclosureNum = Convert.ToString(obj[2]);
                dto.EffectiveDate = CommonHelper.ConvertDateTimeToString(Convert.ToDateTime(obj[3]));
                dto.RequirePspIndicator = Convert.ToString(obj[4]);
                dto.Remarks = Convert.ToString(obj[5]);
                resultList.Add(dto);
            }
            return resultList;
        }

        public virtual IList<LegalAdviceMaster> GetRecordsByParam(string param)
        {
            var hql = "from LegalAdviceMaster l ";
            IQuery hqlQuery = null;
            string[] values;
            IList<string> paramList = null;
            if (param != null && param.Length > 0)
            {
                var where = "where l.LegalAdviceId IN (:param)";
                hql = hql + where;
                values = param.Split(',');
                paramList = new List<string>();
                for (int i = 0; i < values.Length; i++)
                {
                    var value = values[i];
                    if (value != null && value.Length > 0)
                    {
                        paramList.Add(values[i]);
                    }
                }
            }
            hql = hql + "order by l.LegalAdviceType,l.LegalAdviceCode asc";
            hqlQuery = this.Session.CreateQuery(hql);
            hqlQuery.SetParameterList("param", paramList);

            var list = hqlQuery.List<LegalAdviceMaster>();

            return list;
        }

        public virtual IList<LegalAdviceMasterDto> GetDtoRecordsByParam(string param)
        {
            var sql = "SELECT lam.LegalAdviceCode, lam.LegalAdviceDescription, lam.PartNum, lam.EnclosureNum, lam.EffectiveDate, lam.Remarks," + Environment.NewLine +
                      "       l_lat.EngDescription AS LegalAdviceType, l_vt.EngDescription AS VenueType, l_rpi.EngDescription AS RequirePspIndicator" + Environment.NewLine +
                      "FROM LegalAdviceMaster lam" + Environment.NewLine +
                      "LEFT JOIN Lookup l_lat ON l_lat.Type='" + LookupType.LegalAdviceType.ToEnumValue() + "' AND l_lat.Code=lam.LegalAdviceType AND l_lat.IsDeleted = 0" + Environment.NewLine +
                      "LEFT JOIN Lookup l_vt ON l_vt.Type='" + LookupType.VenueType.ToEnumValue() + "' AND l_vt.Code=lam.VenueType AND l_vt.IsDeleted = 0" + Environment.NewLine +
                      "LEFT JOIN Lookup l_rpi ON l_rpi.Type='" + LookupType.PSPRequiredIndicator.ToEnumValue() + "' AND l_rpi.Code=lam.RequirePspIndicator AND l_rpi.IsDeleted = 0" + Environment.NewLine +
                      "WHERE lam.IsDeleted=0 ";

            IQuery sqlQuery = null;
            string[] values;
            IList<string> paramList = null;
            if (param != null && param.Length > 0)
            {
                var where = "AND lam.LegalAdviceId IN (:param) ";
                sql = sql + where;
                values = param.Split(',');
                paramList = new List<string>();
                for (int i = 0; i < values.Length; i++)
                {
                    var value = values[i];
                    if (value != null && value.Length > 0)
                    {
                        paramList.Add(values[i]);
                    }
                }
            }

            sql = sql + Environment.NewLine + "ORDER BY lam.LegalAdviceType,lam.LegalAdviceCode asc";
            sqlQuery = this.Session.CreateSQLQuery(sql);
            sqlQuery.SetParameterList("param", paramList);
            IList<object[]> list = sqlQuery.List<object[]>();
            IList<LegalAdviceMasterDto> resultList = new List<LegalAdviceMasterDto>();
            for (var i = 0; i < list.Count; i++)
            {
                var dto = new LegalAdviceMasterDto();
                object[] obj = list[i];
                dto.LegalAdviceAndVenueType = Convert.ToString(obj[0]) + Convert.ToString(obj[1]);
                dto.LegalAdviceCode = Convert.ToString(obj[2]);
                dto.LegalAdviceDescription = Convert.ToString(obj[3]);
                dto.PartNum = Convert.ToString(obj[4]);
                dto.EnclosureNum = Convert.ToString(obj[5]);
                dto.EffectiveDate = CommonHelper.ConvertDateTimeToString(Convert.ToDateTime(obj[6]));
                dto.Remarks = Convert.ToString(obj[7]);
                dto.RequirePspIndicator = Convert.ToString(obj[8]);
                resultList.Add(dto);
            }

            return resultList;
        }

        public virtual string GetLegalAdviceCodeSuffix(string LegalAdviceCodePrefix)
        {
            string RetStr = "001";
            var hql = "select LegalAdviceCode from LegalAdviceMaster l  where l.LegalAdviceCode like '%" + LegalAdviceCodePrefix + "%' ";

            hql = hql + "order by l.LegalAdviceId desc";
            IList<string> list = this.Session.CreateSQLQuery(hql).List<string>();

            int RetInt = 0;
            for (int i = 0; i < list.Count; i++)
            {
                string listvalue = list[i].ToString();
                if (listvalue.Length > 0)
                {
                    listvalue = listvalue.Replace(LegalAdviceCodePrefix, "");
                    if (ConvertInt(listvalue) > RetInt)
                        RetInt = ConvertInt(listvalue);
                }
            }

            if (RetInt > 0)
            {
                int num = RetInt + 1;
                RetStr = num.ToString().PadLeft(3, '0');
            }
            return RetStr;
        }

        public int ConvertInt(string num)
        {
            int mValue = 0;
            try
            {
                mValue = Convert.ToInt32(num);
            }
            catch
            {
                mValue = 0;
            }
            return mValue;
        }
    }
}