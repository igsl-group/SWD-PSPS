﻿using Psps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Infrastructure
{
    public interface IService<T, TPk> where T : BaseEntity<TPk>
    {
    }
}