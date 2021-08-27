using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Security;
using System.Collections.Generic;

namespace Psps.Services.Security
{
    public class FunctionService : IFunctionService
    {
        private readonly IFunctionRepository _functionRepository;

        public FunctionService(IFunctionRepository functionRepository)
        {
            this._functionRepository = functionRepository;
        }

        public IPagedList<Function> GetPage(GridSettings grid)
        {
            return _functionRepository.GetPage(grid);
        }

        public List<Function> GetFunctionList()
        {
            return (List<Function>)_functionRepository.GetAll();
        }

        public Function GetFunctionById(string funcId)
        {
            return _functionRepository.GetById(funcId);
        }

        public IList<FunctionDto> GetFunctionsByRoleId(string roleId)
        {
            return _functionRepository.GetFunctionsByRoleId(roleId);
        }
    }
}