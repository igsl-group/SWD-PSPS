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
    public interface IOrgEditEnquiryViewRepository : IRepository<OrgEditEnquiryView, int> { }

    public class OrgEditEnquiryViewRepository : BaseRepository<OrgEditEnquiryView, int>, IOrgEditEnquiryViewRepository
    {
        public OrgEditEnquiryViewRepository(ISession session)
            : base(session)
        {
        }
    }
}