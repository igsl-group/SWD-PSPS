using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.SystemMessages;
using Psps.Services.Security;
using Psps.Services.Posts;
using Psps.Web.ViewModels.Account;
using Psps.Web.ViewModels.Role;

namespace Psps.Web.Validators
{
    public class RoleViewModelValidator<T> :AbstractValidator<T> where T:RoleViewModel
    {

        public RoleViewModelValidator(IMessageService messageService, IRoleService roleService, IPostsInRolesService postsInRolesService) 
        {
        }

    }


    public class CreateRoleViewModelValidator : RoleViewModelValidator<CreateRoleViewModel>
    {
        protected readonly IRoleService _roleService;
        protected readonly IPostsInRolesService _postsInRolesService;

        public CreateRoleViewModelValidator(IMessageService messageService, IRoleService roleService, IPostsInRolesService postsInRolesService)
            : base(messageService, roleService, postsInRolesService)
        {
            _roleService = roleService;
            _postsInRolesService = postsInRolesService;
            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);

            RuleFor(x => x.RoleName).NotEmpty().WithMessage(mandatoryMessage);
            var uniqueMessage = messageService.GetMessage(SystemMessage.Error.Unique);

            RuleFor(x => x.RoleName).Must(UniqueRoleId).When(x => !string.IsNullOrEmpty(x.RoleName)).WithMessage(uniqueMessage);
        }

        private bool UniqueRoleId(string roleId)
        {
            return _roleService.GetRoleById(roleId) == null;
        }
    }


    public class CreatePostsInRolesViewModelValidator : RoleViewModelValidator<CreatePostsInRolesViewModel>
    {
        protected readonly IRoleService _roleService;
        protected readonly IPostsInRolesService _postsInRolesService;

        public CreatePostsInRolesViewModelValidator(IMessageService messageService, IRoleService roleService, IPostsInRolesService postsInRolesService)
            : base(messageService, roleService, postsInRolesService)
        {
            _roleService = roleService;
            _postsInRolesService = postsInRolesService;

            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
            RuleFor(x => x.RolePostId).NotEmpty().WithMessage(mandatoryMessage);

            var postAlreadyExistedErrorMessage = messageService.GetMessage(SystemMessage.Error.Role.PostAlreadyExistedError);
            RuleFor(x => x.RolePostId).Must(ValidatePostIsNotExisted).When(x => !string.IsNullOrEmpty(x.RolePostId)).WithMessage(postAlreadyExistedErrorMessage);
            //RuleFor(x => x.RolePostId).Must(ValidatePostIsExisted).When(x => !string.IsNullOrEmpty(x.RolePostId)).WithMessage(postAlreadyExistedErrorMessage);
        }

        private bool ValidatePostIsNotExisted(CreatePostsInRolesViewModel model, string postId)
        {
            return !_postsInRolesService.ValidatePostIsExisted(model.RoleId, postId);
        }

    }
}