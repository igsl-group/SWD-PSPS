using Psps.Core.Helper;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psps.Services.Security
{
    public partial class AclService : IAclService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IActingRepository _actingRepository;
        private List<string> _cachedFunctions;

        public AclService(IPostRepository postRepository, IUserRepository userRepository, IActingRepository actingRepository)
        {
            this._postRepository = postRepository;
            this._userRepository = userRepository;
            this._actingRepository = actingRepository;
        }

        public List<string> GetAllowedFunctionsByPost(string postId)
        {
            if (_cachedFunctions != null)
                return _cachedFunctions;

            var query = (from p in _postRepository.Table
                         from r in p.Roles
                         from f in r.Functions
                         where p.PostId == postId
                         select f.FunctionId).Distinct();

            _cachedFunctions = query.ToList<string>();

            return _cachedFunctions;
        }

        public bool ValidateUserIdAndPostId(string userId, string postId, out User currentUser)
        {
            Ensure.Argument.NotNullOrEmpty(userId, "userId");
            Ensure.Argument.NotNullOrEmpty(postId, "postId");

            currentUser = _userRepository.Get(u => u.UserId == userId, "Post");
            var postToBeActed = GetPostToBeActed(userId, DateTime.Today);

            var validUser = currentUser != null && !currentUser.IsDeleted && currentUser.IsActive;
            var validPost = currentUser != null && currentUser.Post != null && currentUser.Post.PostId == postId;
            var validActingPost = postToBeActed.Contains(postId);

            return validUser && (validPost || validActingPost);
        }

        public List<string> GetPostToBeActed(string userId, DateTime? today)
        {
            if (today == null) today = DateTime.Today;

            var q = from a in _actingRepository.Table
                    where today.Value.Date >= a.EffectiveFrom
                        && (a.EffectiveTo == null || today.Value.Date <= a.EffectiveTo)
                        && a.User.UserId == userId
                    select a.PostToBeActed.PostId;

            return q.ToList();
        }
    }
}