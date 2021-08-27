using FluentValidation;
using Psps.Core;
using Psps.Core.Infrastructure;
using Psps.Models.Domain;
using Psps.Services.Accounts;
using Psps.Services.Actings;
using Psps.Services.Posts;
using Psps.Services.Ranks;
using Psps.Services.SystemMessages;
using Psps.Web.ViewModels.Account;
using Psps.Web.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Psps.Web.Validators
{
	public class PostViewModelValidator<T> : AbstractValidator<T> where T : PostViewModel
	{
		protected readonly IPostService _postService;
		protected readonly IUserService _userService;
		protected readonly IActingService _actingService;
		protected readonly IRankService _rankService;

		public PostViewModelValidator(IMessageService messageService, IPostService postService, IUserService userService, IActingService actingService, IRankService rankService)
		{
			_postService = postService;
			_actingService = actingService;
			_userService = userService;
			_rankService = rankService;

			RuleFor(x => x.PostId).NotEmpty().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));
            RuleFor(x => x.Rank).NotEmpty().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));

			RuleSet("EditPost", () =>
			{
				RuleFor(x => x.Rank).NotEmpty().WithMessage(messageService.GetMessage(SystemMessage.Error.Mandatory));
			});
		}

		protected bool OwnerWithoutOtherPost(PostViewModel model, string owner)
		{
			List<Post> post = _postService.GetPostByOwnerUserId(model.Owner);

			// if found;
			if (post.Count > 0)
			{
				// if the found post is same as the current post
				if (post[0].PostId == model.PostId)
				{
					// Is Valid
					return true;
				}
				else
				{
					// Otherwise, the owner has been used by other post. Invalid.
					return false;
				}
			}
			else
			{
				return true;
			}
		}

		protected bool RankofSupervisorLessthanCurrentPostRankLevel(PostViewModel model, string rank)
		{
			var post = _postService.GetPostById(model.Supervisor);
			var rankLevel = _rankService.GetRankById(rank);
			if (post.Rank.RankLevel < rankLevel.RankLevel)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		protected bool ValidateUserIsValid(string AssignTo)
		{
			var user = _userService.GetUserById(AssignTo);
			if (user != null)
			{
				return user.IsActive;
			}
			else
			{
				return false;
			}
		}

		protected bool ValidateAssignToOtherPost(PostActingViewModel model, string userId)
		{
			var post = _postService.GetPostById(model.PostId);
			if (post != null)
			{
				if (post.Owner != null)
					return !userId.Equals(post.Owner.UserId);
				else
					return true;
			}
			else
			{
				return false;
			}
		}

		protected bool ValidateFromDateEarlierThanToDate(PostActingViewModel model, DateTime fromDate)
		{
			return !(fromDate >= model.EffectiveTo);
		}

		protected bool ValidateNotAssigned(PostActingViewModel model, string assignToUserId)
		{
			return !this._actingService.ValidateIsAssigned(model.ActingId.HasValue ? model.ActingId.Value : -1, model.PostId, model.EffectiveFrom, model.EffectiveTo);
		}
	}

	public class CreatePostViewModelValidator : PostViewModelValidator<CreatePostViewModel>
	{
		public CreatePostViewModelValidator(IMessageService messageService, IPostService postService, IUserService userService, IActingService actingService, IRankService rankService)
			: base(messageService, postService, userService, actingService, rankService)
		{
			var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
			var uniqueMessage = messageService.GetMessage(SystemMessage.Error.Unique);
			var OwnerHeldByOthersMessage = messageService.GetMessage(SystemMessage.Error.Post.OwnerHeldByOthers);
			var RankofSupervisor = messageService.GetMessage(SystemMessage.Error.Post.RankofSupervisor);

			RuleFor(x => x.PostId)
			   .Must(UniquePostId).WithMessage(messageService.GetMessage(SystemMessage.Error.Unique));
			RuleFor(x => x.Owner)
			   .Must(OwnerWithoutOtherPost).When(x => !string.IsNullOrEmpty(x.Owner)).WithMessage(OwnerHeldByOthersMessage);
			RuleFor(x => x.Rank).NotEmpty().WithMessage(mandatoryMessage);
			RuleFor(x => x.Rank)
			   .Must(RankofSupervisorLessthanCurrentPostRankLevel).When(x => x.Supervisor.IsNotNullOrEmpty() && x.Rank.IsNotNullOrEmpty()).WithMessage(RankofSupervisor);
		}

		private bool UniquePostId(string PostId)
		{
			return _postService.IsUniquePostId(PostId);
		}
	}

	public class UpdatePostViewModelValidator : PostViewModelValidator<UpdatePostViewModel>
	{
		public UpdatePostViewModelValidator(IMessageService messageService, IPostService postService, IUserService userService, IActingService actingService, IRankService rankService)
			: base(messageService, postService, userService, actingService, rankService)
		{
			var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
			var OwnerHeldByOthersMessage = messageService.GetMessage(SystemMessage.Error.Post.OwnerHeldByOthers);
			var RankofSupervisor = messageService.GetMessage(SystemMessage.Error.Post.RankofSupervisor);
			RuleFor(x => x.Owner)
			   .Must(OwnerWithoutOtherPost).When(x => !string.IsNullOrEmpty(x.Owner)).WithMessage(OwnerHeldByOthersMessage);
            //RuleFor(x => x.Rank).NotEmpty().WithMessage(mandatoryMessage);
			RuleFor(x => x.Rank)
			   .Must(RankofSupervisorLessthanCurrentPostRankLevel).When(x => !string.IsNullOrEmpty(x.Supervisor) && !string.IsNullOrEmpty(x.Rank)).WithMessage(RankofSupervisor);
		}
	}

	public class CreatePostActingViewModelValidator : PostViewModelValidator<CreatePostActingViewModel>
	{
		public CreatePostActingViewModelValidator(IMessageService messageService, IPostService postService, IUserService userService, IActingService actingService, IRankService rankService)
			: base(messageService, postService, userService, actingService, rankService)
		{
			//Assign To cannot be empty.
			var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
			RuleFor(x => x.AssignTo).NotEmpty().WithMessage(mandatoryMessage);

			//UserId is either inactive or invalid
			var invalidUserMsg = messageService.GetMessage(SystemMessage.Error.User.InvalidUserError);

			RuleFor(x => x.AssignTo).Must(ValidateUserIsValid).When(x => !string.IsNullOrEmpty(x.AssignTo)).WithMessage(invalidUserMsg);

			var samePostErrorMsg = messageService.GetMessage(SystemMessage.Error.User.SamePostError);
			RuleFor(x => x.AssignTo).Must(ValidateAssignToOtherPost).When(x => !string.IsNullOrEmpty(x.AssignTo)).WithMessage(samePostErrorMsg);

			RuleFor(x => x.EffectiveFrom).NotEmpty().WithMessage(mandatoryMessage);
			RuleFor(x => x.EffectiveTo).NotEmpty().WithMessage(mandatoryMessage);

			var fromDateLaterThanToDateMsg = messageService.GetMessage(SystemMessage.Error.FromDateLaterThanToDate);
			RuleFor(x => x.EffectiveFrom).Must(ValidateFromDateEarlierThanToDate).When(x => x.EffectiveFrom != null && x.EffectiveTo != null).WithMessage(fromDateLaterThanToDateMsg);

			var isAssignedErrorMsg = messageService.GetMessage(SystemMessage.Error.User.UserIsAssignedError);
			RuleFor(x => x.AssignTo).Must(ValidateNotAssigned).When(x => !string.IsNullOrEmpty(x.AssignTo)).WithMessage(isAssignedErrorMsg);
		}
	}

	public class UpdatePostActingViewModelValidator : PostViewModelValidator<UpdatePostActingViewModel>
	{
		public UpdatePostActingViewModelValidator(IMessageService messageService, IPostService postService, IUserService userService, IActingService actingService, IRankService rankService)
			: base(messageService, postService, userService, actingService, rankService)
		{
			//Assign To cannot be empty.
			var mandatoryMessage = messageService.GetMessage(SystemMessage.Error.Mandatory);
			RuleFor(x => x.AssignTo).NotEmpty().WithMessage(mandatoryMessage);

			//UserId is either inactive or invalid
			var invalidUserMsg = messageService.GetMessage(SystemMessage.Error.User.InvalidUserError);

			RuleFor(x => x.AssignTo).Must(ValidateUserIsValid).When(x => !string.IsNullOrEmpty(x.AssignTo)).WithMessage(invalidUserMsg);

			var samePostErrorMsg = messageService.GetMessage(SystemMessage.Error.User.SamePostError);
			RuleFor(x => x.AssignTo).Must(ValidateAssignToOtherPost).When(x => !string.IsNullOrEmpty(x.AssignTo)).WithMessage(samePostErrorMsg);

			var invalidDateMsg = messageService.GetMessage(SystemMessage.Error.InvalidDate);
			RuleFor(x => x.EffectiveFrom).NotEmpty().WithMessage(mandatoryMessage);
			RuleFor(x => x.EffectiveTo).NotEmpty().WithMessage(mandatoryMessage);

			var fromDateLaterThanToDateMsg = messageService.GetMessage(SystemMessage.Error.FromDateLaterThanToDate);
			RuleFor(x => x.EffectiveFrom).Must(ValidateFromDateEarlierThanToDate).When(x => x.EffectiveFrom != null && x.EffectiveTo != null).WithMessage(fromDateLaterThanToDateMsg);

			var isAssignedErrorMsg = messageService.GetMessage(SystemMessage.Error.User.UserIsAssignedError);
			RuleFor(x => x.AssignTo).Must(ValidateNotAssigned).When(x => !string.IsNullOrEmpty(x.AssignTo)).WithMessage(isAssignedErrorMsg);
		}
	}
}