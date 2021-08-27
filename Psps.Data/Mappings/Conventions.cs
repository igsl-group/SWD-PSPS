using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Psps.Data.Mappings
{
    public class FullIdNameConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column(instance.EntityType.Name + "Id");
        }
    }

    public class ManyToManyIdNameConvention : IHasManyToManyConvention
    {
        public void Apply(IManyToManyCollectionInstance instance)
        {
            string fkName = string.Format("FK_{0}_{1}", instance.Member.Name, instance.EntityType.Name);
            instance.Key.ForeignKey(fkName);
            instance.Key.Column(instance.EntityType.Name + "Id");
            instance.Relationship.Column(instance.Relationship.StringIdentifierForModel + "Id");
        }
    }

    public class ReferenceIdNameConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            string fkName = string.Format("FK_{0}_{1}", instance.Name, instance.EntityType.Name);
            instance.ForeignKey(fkName);
            instance.Column(instance.Name + "Id");
        }
    }

    public class VersionConvention : IVersionConvention
    {
        public void Apply(IVersionInstance instance)
        {
            instance.CustomType<BinaryTimestamp>();
            instance.CustomSqlType("Timestamp");
            instance.Not.Nullable();
            instance.Generated.Always();
        }
    }

    public class DeletedFilterConvention : IHasManyConvention, IHasManyToManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.ApplyFilter<DeletedFilter>();
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.ApplyFilter<DeletedFilter>();
        }
    }
}