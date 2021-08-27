using FluentNHibernate.Mapping;

namespace Psps.Data.Mappings
{
    public class DeletedFilter : FilterDefinition
    {
        new public static string Name = "DeletedFilter";

        public DeletedFilter()
        {
            WithName(DeletedFilter.Name).WithCondition("IsDeleted = 0");
        }
    }
}