using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Core.Models
{
    [Serializable]
    public abstract class BaseEntity<TPk> : IEntity<TPk>
    {
        public abstract TPk Id { get; set; }

        public static bool operator !=(BaseEntity<TPk> x, BaseEntity<TPk> y)
        {
            return !(x == y);
        }

        public static bool operator ==(BaseEntity<TPk> x, BaseEntity<TPk> y)
        {
            return Equals(x, y);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity<TPk>);
        }

        public virtual bool Equals(BaseEntity<TPk> other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        private static bool IsTransient(BaseEntity<TPk> obj)
        {
            return obj != null && Equals(obj.Id, default(TPk));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }
    }
}