using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models;
using Psps.Models.Domain;
using Psps.Models.Dto.Reports;
using Psps.Models.Dto.SuggestionMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Psps.Data.Repositories
{
    public interface ISuggestionMasterRepository : IRepository<SuggestionMaster, int>
    {
        IList<SuggestionMaster> GetRecordsByParam(string param);

        List<SuggestDto> GetLastFiveYrsSuggestionSummary();

        string CreateSuggestionRefNum();

        IList<R20Dto> GenerateR20Report(DateTime? fromDate, DateTime? toDate);
    }

    public class SuggestionMasterRepository : BaseRepository<SuggestionMaster, int>, ISuggestionMasterRepository
    {
        public SuggestionMasterRepository(ISession session)
            : base(session)
        {
        }

        public virtual IList<SuggestionMaster> GetRecordsByParam(string param)
        {
            var hql = "from SuggestionMaster s ";
            IQuery hqlQuery = null;
            string[] values;
            IList<string> paramList = null;
            if (param != null && param.Length > 0)
            {
                var where = "where s.SuggestionRefNum IN (:param)";
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
            hql = hql + "order by s.SuggestionRefNum asc";
            hqlQuery = this.Session.CreateQuery(hql);
            hqlQuery.SetParameterList("param", paramList);

            var list = hqlQuery.List<SuggestionMaster>();


            return list;
        }

        public List<SuggestDto> GetLastFiveYrsSuggestionSummary()
        {
            string sql = " select year(SuggestionDate) as Year, l.Code, count(SuggestionNature) as result " +
                             " from ( select * from [Lookup] l where l.[type] = 'SuggestionNature' and Code <> '03' ) AS l " +
                             " inner join SuggestionMaster  " +
                             " on  l.Code = SuggestionNature  " +
                             " and (year(SuggestionDate) <= Year(GETDATE()) and year(SuggestionDate) >= Year(GETDATE())- 5)  " +
                             "  " +
                             " group by year(SuggestionDate) , l.Code " +
                             "  " +
                             "  " +
                             " union " +
                             "  " +
                             " select year(SuggestionDate) as Year, 'null' as Code, sum( case when SuggestionNature is null then 1 else 0 end) as result " +
                             " from SuggestionMaster  " +
                             " where (year(SuggestionDate) <= Year(GETDATE()) and year(SuggestionDate) >= Year(GETDATE())- 5 )  " +
                             " group by year(SuggestionDate)  " +
                             "  " +
                             " order by year(SuggestionDate) desc,  Code asc "
                             ;

            IList<object[]> list = this.Session.CreateSQLQuery(sql).List<object[]>();
            var resultList = new List<SuggestDto>();

            for (var i = 0; i < list.Count; i++)
            {
                var SuggestDto = new SuggestDto();
                object[] obj = list[i];
                SuggestDto.Year = Convert.ToString(obj[0]);
                SuggestDto.Code = Convert.ToString(obj[1]);
                SuggestDto.Count = Convert.ToInt32(obj[2]);

                resultList.Add(SuggestDto);
            }

            var returnlist = resultList.GroupBy(rl => rl.Year)
                                  .Select(
                                        g => new SuggestDto
                                        {
                                            Year = g.Key,

                                            CountList = g.Select(x => new { x.Code, x.Count }).AsEnumerable().ToDictionary(ri => ri.Code, rs => rs.Count),
                                        }

                                  ).ToList()

                       ;

            return returnlist;
        }

        public virtual string CreateSuggestionRefNum()
        {
            string SuggestionRefNumPrefix = "(" + DateTime.Now.Year.ToString() + ")-S-";

            string RetStr = "01";
            var hql = "select SuggestionRefNum from SuggestionMaster s  where s.SuggestionRefNum like '%" + SuggestionRefNumPrefix + "%' ";

            hql = hql + "order by s.SuggestionMasterId desc";
            IList<string> list = this.Session.CreateSQLQuery(hql).List<string>();

            int RetInt = 0;
            for (int i = 0; i < list.Count; i++)
            {
                string listvalue = list[i].ToString();
                if (listvalue.Length > 2)
                {
                    listvalue = listvalue.Substring(listvalue.Length - 2, 2);
                    if (ConvertInt(listvalue) > RetInt)
                        RetInt = ConvertInt(listvalue);
                }
            }

            if (RetInt > 0)
            {
                //code = code.Replace(SuggestionRefNumPrefix, "");
                int num = RetInt + 1;
                RetStr = num.ToString().PadLeft(2, '0');
            }
            RetStr = SuggestionRefNumPrefix + RetStr;
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

        public IList<R20Dto> GenerateR20Report(DateTime? fromDate, DateTime? toDate)
        {
            var query = this.Table;

            if (fromDate != null)
            {
                query = query.Where(x => x.SuggestionDate >= fromDate);
            }

            if (toDate != null)
            {
                query = query.Where(x => x.SuggestionDate <= toDate);
            }

            return query.Select(t => new R20Dto()
            {
                SuggestionMasterId = t.SuggestionMasterId,
                SuggestionNature = t.SuggestionNature,
                StrSuggestionDate = (t.SuggestionDate.HasValue ? t.SuggestionDate.Value.ToString("dd'/'MM'/'yyyy") : " "),
                StrAcknowledgementSentDate = (t.AcknowledgementSentDate.HasValue ? t.AcknowledgementSentDate.Value.ToString("dd'/'MM'/'yyyy") : " "),
                SuggestionSenderName = t.SuggestionSenderName,
                SuggestionDescription = t.SuggestionDescription,
                SuggestionRefNum = t.SuggestionRefNum
            }).ToList();
        }

        /*
        public IList<R20Dto> GenerateR20Report(DateTime? fromDate, DateTime? toDate)
        {
            string sql = "select s.SuggestionMasterId, s.SuggestionNature, ISNULL(CONVERT ( varchar(10) , s.SuggestionDate, 103 ), ' ') as StrSuggestionDate, s.SuggestionSenderName, s.SuggestionDescription, s.SuggestionRefNum, ISNULL(CONVERT ( varchar(10) , s.AcknowledgementSentDate, 103 ), ' ') as StrAcknowledgementSentDate " +
            " from SuggestionMaster s ";

            string condition = " where s.IsDeleted = 0 ";
            if (fromDate.HasValue)
                condition += " and s.SuggestionDate >= :fromDate ";
            if (toDate.HasValue)
                condition += " and s.SuggestionDate <= :toDate ";

            sql += condition;

            var query = this.Session.CreateSQLQuery(sql);

            if (fromDate.HasValue)
                query.SetDateTime("fromDate", fromDate.Value);
            if (toDate.HasValue)
                query.SetDateTime("toDate", toDate.Value);

            IList<R20Dto> list = query
                .SetResultTransformer(new AliasToBeanResultTransformer(typeof(R20Dto)))
                .List<R20Dto>();

            return list;
        }
         */
    }
}