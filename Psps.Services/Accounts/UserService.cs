using AutoMapper;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Accounts;
using Psps.Models.Dto.Security;
using Psps.Services.Accounts;
using Psps.Services.Events;
using Psps.Services.UserLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psps.Services.Accounts
{
    public partial class UserService : IUserService
    {
        #region Keys

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string USERROLES_ALL_KEY = "Psps.userrole.all-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string USERROLES_PATTERN_KEY = "Psps.userrole.";

        private const string USER_FOR_DROWDROP_KEY = "Psps.user.dropdown.all";

        #endregion Keys

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly IUserRepository _userRepository;

        private readonly IPostRepository _postRepository;

        private readonly IUserLogService _userLogService;

        public UserService(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IUserRepository userRepository,
            IPostRepository postRepository,
            IUserLogService userLogService)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._userRepository = userRepository;
            this._postRepository = postRepository;
            this._userLogService = userLogService;
        }

        #region Users

        public User GetUserByEmail(string email)
        {
            Ensure.Argument.NotNullOrEmpty(email, "email");

            return _userRepository.Get(u => u.Email == email);
        }

        public User GetUserById(string userId)
        {
            Ensure.Argument.NotNullOrEmpty(userId, "userId");

            return _userRepository.Get(u => u.UserId == userId);
        }

        public void CreateUser(UserInfoDto userInfoDto)
        {
            Ensure.Argument.NotNull(userInfoDto, "userInfoDto");

            var user = Mapper.Map<UserInfoDto, User>(userInfoDto);
            user.Password = Encryptor.Hash(user.Password);

            _userRepository.Add(user);
            _eventPublisher.EntityInserted<User>(user);

            if (!string.IsNullOrEmpty(userInfoDto.PostId))
            {
                var post = _postRepository.GetById(userInfoDto.PostId);
                Ensure.NotNull(post, "No post found with the specified id");

                post.Owner = user;
                _postRepository.Update(post);
                _eventPublisher.EntityUpdated<Post>(post);
            }
        }

        public void UpdateUser(UserInfoDto userInfoDto)
        {
            Ensure.Argument.NotNull(userInfoDto, "userInfoDto");

            var user = _userRepository.GetById(userInfoDto.UserId);
            Ensure.NotNull(user, "No user found with the specified id");

            //Will not update the password if incoming password is empty
            var hashedPassword = userInfoDto.Password.IsNotNullOrEmpty() ? Encryptor.Hash(userInfoDto.Password) : user.Password;
            Mapper.Map<UserInfoDto, User>(userInfoDto, user);
            user.Password = hashedPassword;
            //Resert the password changed date so that enforce to change password when login
            if (userInfoDto.Password.IsNotNullOrEmpty()) user.PasswordChangedDate = null;
            _userRepository.Update(user);
            _eventPublisher.EntityUpdated<User>(user);

            //Check the logic for update post as well
            var originalPost = user.Post;
            var originalPostId = originalPost != null ? originalPost.PostId : "";
            var newPostId = userInfoDto.PostId;
            var isPostChanged = !originalPostId.Equals(newPostId);

            if (isPostChanged)
            {
                Post newPost = null;
                if (!string.IsNullOrEmpty(newPostId))
                {
                    newPost = _postRepository.GetById(newPostId);
                    Ensure.NotNull(newPost, "No post found with the specified id");
                }

                if (originalPost != null)
                {
                    originalPost.Owner = null;
                    _postRepository.Update(originalPost);
                    _eventPublisher.EntityUpdated<Post>(originalPost);
                }

                if (newPost != null)
                {
                    newPost.Owner = user;
                    _postRepository.Update(newPost);
                    _eventPublisher.EntityUpdated<Post>(newPost);
                }
            }
        }

        public void Update(User user)
        {
            Ensure.Argument.NotNull(user, "user");
            _userRepository.Update(user);
            _eventPublisher.EntityUpdated<User>(user);
        }

        public void ChangePassword(string userId, string password)
        {
            Ensure.Argument.NotNullOrEmpty(userId, "userId");
            Ensure.Argument.NotNullOrEmpty(password, "password");

            var user = _userRepository.GetById(userId);
            Ensure.NotNull(user, "No user found with the specified id");

            //Will not update the password if incoming password is empty
            var hashedPassword = string.IsNullOrEmpty(password) ? user.Password : Encryptor.Hash(password);

            //Update Prev Password
            user.PrevPassword8 = user.PrevPassword7;
            user.PrevPassword7 = user.PrevPassword6;
            user.PrevPassword6 = user.PrevPassword5;
            user.PrevPassword5 = user.PrevPassword4;

            user.PrevPassword4 = user.PrevPassword3;
            user.PrevPassword3 = user.PrevPassword2;
            user.PrevPassword2 = user.PrevPassword1;
            user.PrevPassword1 = user.Password;
            user.Password = hashedPassword;
            user.PasswordChangedDate = DateTime.Now;

            _userRepository.Update(user);
            _eventPublisher.EntityUpdated<User>(user);
        }

        public bool ExistsInPasswordHistory(string userId, string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            password = Encryptor.Hash(password);
            var result = _userRepository.Table
                .Where(i => i.UserId == userId && 
                        (
                            password.Equals(i.Password) || 
                            password.Equals(i.PrevPassword1) || 
                            password.Equals(i.PrevPassword2) || 
                            password.Equals(i.PrevPassword3) || 
                            password.Equals(i.PrevPassword4) ||
                            password.Equals(i.PrevPassword5) ||
                            password.Equals(i.PrevPassword6) ||
                            password.Equals(i.PrevPassword7) ||
                            password.Equals(i.PrevPassword8)
                        ))
                .Any();

            return result;
        }

        public UserLoginResults ValidateUser(string userId, string password)
        {
            Ensure.Argument.NotNullOrEmpty(userId, "userId");
            Ensure.Argument.NotNullOrEmpty(password, "password");

            var user = _userRepository.Get(u => u.UserId == userId, "Post");

            if (user != null && !user.IsDeleted)
            {
                if (!user.IsActive)
                    return UserLoginResults.NotActive;
                else if (user.Post == null)
                    return UserLoginResults.NoPost;
                else
                {
                    var hashedPassword = Encryptor.Hash(password);
                    return hashedPassword.Equals(user.Password) ? UserLoginResults.Successful : UserLoginResults.WrongPassword;
                }
            }

            return UserLoginResults.UserNotExist;
        }

        public virtual IPagedList<User> GetPage(GridSettings grid)
        {
            return _userRepository.GetPage(grid);
        }

        public IDictionary<string, string> getAllUserForDropdown()
        {
            string key = USER_FOR_DROWDROP_KEY;
            return _cacheManager.Get(key, () =>
            {
                return this._userRepository.Table
                     .OrderBy(p => p.UserId)
                     .Where(p => p.IsActive == true)
                     .Select(p => new { Key = p.UserId, Value = p.UserId + "-" + p.EngUserName })
                     .ToDictionary(k => k.Key, v => v.Value);
            });
        }

        #endregion Users
    }
}