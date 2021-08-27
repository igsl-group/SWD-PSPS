using Psps.Core.Helper;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Security;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Security
{
    public class FunctionsInRolesService : IFunctionsInRolesService
    {
        private readonly IFunctionsInRoleRepository _functionsInRoleRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEventPublisher _eventPublisher;

        public FunctionsInRolesService(IEventPublisher eventPublisher, IFunctionsInRoleRepository functionsInRoleRepository, IFunctionRepository functionRepository, IRoleRepository roleRepository)
        {
            this._eventPublisher = eventPublisher;
            this._functionsInRoleRepository = functionsInRoleRepository;
            this._functionRepository = functionRepository;
            this._roleRepository = roleRepository;
        }

        public void CreateOrUpdateFunctionsInRoles(IList<FunctionsInRolesDto> list)
        {
            Ensure.Argument.NotNull(list, "list");
            // no data ,no update or create
            if (list == null || list.Count() == 0)
            {
                return;
            }

            for (var i = 0; i < list.Count(); i++)
            {
                var item = list[i];
                var funcInRoles = _functionsInRoleRepository.GetByRoleIdAndFuncId(item.RoleId, item.FunctionId);

                if (funcInRoles == null && item.IsEnabled)//create
                {
                    var function = _functionRepository.GetById(item.FunctionId);
                    var role = _roleRepository.GetById(item.RoleId);
                    funcInRoles = new FunctionsInRoles
                    {
                        RoleId = item.RoleId,
                        Role = role,
                        FunctionId = item.FunctionId,
                        Function = function
                    };

                    _functionsInRoleRepository.Add(funcInRoles);
                    _eventPublisher.EntityInserted<FunctionsInRoles>(funcInRoles);
                }
                else if (funcInRoles != null && !item.IsEnabled)//delete
                {
                    bool result = _functionsInRoleRepository.Delete(funcInRoles);
                    _eventPublisher.EntityDeleted<FunctionsInRoles>(funcInRoles);
                }
            }
        }

        public IList<FunctionsInRoles> GetByRoleId(string roleId)
        {
            Ensure.Argument.NotNull(roleId, "roleId");
            return _functionsInRoleRepository.GetByRoleId(roleId);
        }
    }
}