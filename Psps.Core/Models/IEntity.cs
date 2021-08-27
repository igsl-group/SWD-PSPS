using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Core.Models
{
    public interface IEntity<TPk>
    {
        TPk Id { get; set; }
    }
}