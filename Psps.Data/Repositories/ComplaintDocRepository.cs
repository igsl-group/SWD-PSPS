using NHibernate;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Psps.Core;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Extensions;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Infrastructure;
using Psps.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Text;

namespace Psps.Data.Repositories
{
    public interface IComplaintDocRepository : IRepository<ComplaintDoc, int>
    {
        IList<ComplaintDoc> GetRecordsByParam(string param);
        IPagedList<ComplaintDoc> GetPage(GridSettings grid, string docNum);
        void ChangeOtherVersionStatus(ComplaintDoc complaintDoc);
    }

    public class ComplaintDocRepository : BaseRepository<ComplaintDoc, int>, IComplaintDocRepository
    {
        public ComplaintDocRepository(ISession session)
            : base(session)
        {
        }

        public virtual IList<ComplaintDoc> GetRecordsByParam(string param)
        {
            var hql = "from ComplaintDoc s ";
            IQuery hqlQuery = null;
            string[] values;
            IList<string> paramList = null;
            if (param != null && param.Length > 0)
            {
                var where = "where s.DocNum IN (:param)";
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
            hql = hql + "order by s.DocNum asc";
            hqlQuery = this.Session.CreateQuery(hql);
            hqlQuery.SetParameterList("param", paramList);

            var list = hqlQuery.List<ComplaintDoc>();

            return list;
        }

        public IPagedList<ComplaintDoc> GetPage(GridSettings grid, string docNum)
        {
            var query = this.Table;

            //eager fetching
            query = query.Where(x => x.DocNum == docNum);

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<ComplaintDoc>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<ComplaintDoc>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public void ChangeOtherVersionStatus(ComplaintDoc complaintDoc)
        {
            StringBuilder hql = new StringBuilder("update ComplaintDoc o set o.DocStatus=0,o.UpdatedById= :UpdatedById,UpdatedByPost= :UpdatedByPost  where o.DocNum= :DocNum");
            if (!String.IsNullOrEmpty(complaintDoc.VersionNum))
            {
                hql.Append(" and o.VersionNum!= :VersionNum");
            }
            var hqlQuery = this.Session.CreateQuery(hql.ToString());
            var userId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.UserId;
            var postId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.PostId;
            hqlQuery.SetParameter("UpdatedById", userId);
            hqlQuery.SetParameter("UpdatedByPost", postId);
            hqlQuery.SetParameter("DocNum", complaintDoc.DocNum);
            if (!String.IsNullOrEmpty(complaintDoc.VersionNum))
            {
                hqlQuery.SetParameter("VersionNum", complaintDoc.VersionNum);
            }
            hqlQuery.ExecuteUpdate();
        }
    }
}