using Psps.Models.Domain;
using System;
using System.Collections.Generic;

namespace Psps.Services.Security
{
    public partial interface IAclService
    {
        List<string> GetAllowedFunctionsByPost(string postId);

        /// <summary>
        /// Validate user created from form ticket
        /// </summary>
        /// <param name="userId">User id stored at ticket</param>
        /// <param name="postId">Post id stored at ticket</param>
        /// <param name="isSysAdmin">System administrator indicate</param>
        /// <returns>Is the ticket user still valid</returns>
        bool ValidateUserIdAndPostId(string userId, string postId, out User currentUser);

        List<string> GetPostToBeActed(string userId, DateTime? today);
    }
}