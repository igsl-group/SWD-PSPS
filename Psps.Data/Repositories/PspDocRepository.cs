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
    public interface IPspDocRepository : IRepository<PspDoc, int>
    {
        IList<PspDoc> GetRecordsByParam(string param);
        IPagedList<PspDoc> GetPage(GridSettings grid, string docNum);
        void ChangeOtherVersionStatus(PspDoc pspDoc);
    }

    public class PspDocRepository : BaseRepository<PspDoc, int>, IPspDocRepository
    {
        public PspDocRepository(ISession session)
            : base(session)
        {
        }

        public virtual IList<PspDoc> GetRecordsByParam(string param)
        {
            var hql = "from PspDoc s ";
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

            var list = hqlQuery.List<PspDoc>();

            return list;
        }

        public IPagedList<PspDoc> GetPage(GridSettings grid, string docNum)
        {
            var query = this.Table;

            //eager fetching
            query = query.Where(x => x.DocNum == docNum);

            //filtring
            if (grid.IsSearch)
                query = query.Where(grid.Where);

            //sorting
            if (!string.IsNullOrEmpty(grid.SortColumn))
                query = query.OrderBy<PspDoc>(grid.SortColumn, grid.SortOrder);

            var page = new PagedList<PspDoc>(query, grid.PageIndex, grid.PageSize);

            return page;
        }

        public void ChangeOtherVersionStatus(PspDoc pspDoc)
        {
            StringBuilder hql = new StringBuilder("update PspDoc o set o.DocStatus=0,o.UpdatedById= :UpdatedById,UpdatedByPost= :UpdatedByPost  where o.DocNum= :DocNum");
            if (!String.IsNullOrEmpty(pspDoc.VersionNum))
            {
                hql.Append(" and o.VersionNum!= :VersionNum");
            }
            var hqlQuery = this.Session.CreateQuery(hql.ToString());
            var userId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.UserId;
            var postId = EngineContext.Current.Resolve<IWorkContext>().CurrentUser.PostId;
            hqlQuery.SetParameter("UpdatedById", userId);
            hqlQuery.SetParameter("UpdatedByPost", postId);
            hqlQuery.SetParameter("DocNum", pspDoc.DocNum);
            if (!String.IsNullOrEmpty(pspDoc.VersionNum))
            {
                hqlQuery.SetParameter("VersionNum", pspDoc.VersionNum);
            }
            hqlQuery.ExecuteUpdate();
        }
    }
}