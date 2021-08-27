namespace Psps.Models.Dto.Security
{
    /// <summary>
    /// Represents the user login result enumeration
    /// </summary>
    public enum UserLoginResults : int
    {
        /// <summary>
        /// Login successful
        /// </summary>
        Successful = 1,

        /// <summary>
        /// User does not exist
        /// </summary>
        UserNotExist = 2,

        /// <summary>
        /// No post assiged to user
        /// </summary>
        NoPost = 3,

        /// <summary>
        /// Wrong password
        /// </summary>
        WrongPassword = 4,

        /// <summary>
        /// Account have not been activated
        /// </summary>
        NotActive = 5,

        /// <summary>
        /// User has been deleted
        /// </summary>
        Deleted = 6
    }
}