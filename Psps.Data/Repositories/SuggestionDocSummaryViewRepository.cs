using NHibernate;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Data.Repositories
{
    public interface ISuggestionDocSummaryViewRepository : IRepository<SuggestionDocSummaryView, int> { }

    public class SuggestionDocSummaryViewRepository : BaseRepository<SuggestionDocSummaryView, int>, ISuggestionDocSummaryViewRepository
    {
        public SuggestionDocSummaryViewRepository(ISession session)
            : base(session)
        {
        }
    }
}