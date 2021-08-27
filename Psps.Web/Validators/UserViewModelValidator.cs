using FluentValidation;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Account;

namespace Psps.Web.Validators
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        protected readonly IUserService _userService;
        protected readonly IPostService _postService;

        public UserViewModelValidator(IMessageService messageService, IUserService userService, IPostService postService)
        {
            _userService = userService;
            _postService = postService;

            var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);

            RuleFor(x => x.UserId).NotEmpty().WithMessage(mandatoryMessage);
            //RuleFor(x => x.PostId).NotEmpty().WithMessage(mandatoryMessage);
            RuleFor(x => x.EngUserName).NotEmpty().WithMessage(mandatoryMessage);

            RuleSet("CreateUser", () =>
            {
                var uniqueMessage = messageService.GetMessage(SystemMessage.Error.Unique);
                var postHeldByOthersMessage = messageService.GetMessage(SystemMessage.Error.User.PostHeldByOthers);

                RuleFor(x => x.UserId).Must(UniqueUserId).When(x => !string.IsNullOrEmpty(x.UserId)).WithMessage(uniqueMessage);

                RuleFor(x => x.PostId).Must(PostWithoutOtherOwner).When(x => !string.IsNullOrEmpty(x.PostId)).WithMessage(postHeldByOthersMessage);

                RuleFor(x => x.Password).NotEmpty().WithMessage(mandatoryMessage);
            });

            RuleSet("UpdateUser", () =>
            {
                var postHeldByOthersMessage = messageService.GetMessage(SystemMessage.Error.User.PostHeldByOthers);

                RuleFor(x => x.PostId)
                    .Must(PostWithoutOtherOwner).When(x => !string.IsNullOrEmpty(x.PostId)).WithMessage(postHeldByOthersMessage);
            });
        }

        protected bool PostWithoutOtherOwner(UserViewModel model, string postId)
        {
            var post = _postService.GetPostById(model.PostId);

            return post.Owner == null || post.Owner.UserId == model.UserId;
        }

        private bool UniqueUserId(UserViewModel model, string userId)
        {
            return _userService.GetUserById(userId) == null;
        }
    }
}