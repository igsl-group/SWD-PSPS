using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;

namespace Psps.Services.SystemMessages
{
    /// <summary>
    /// System message service interface
    /// </summary>
    public partial interface IMessageService
    {
        /// <summary>
        /// Get system message by id
        /// </summary>
        /// <param name="systemMessageId">System message Id</param>
        /// <returns>System message entity</returns>
        SystemMessage GetMessageById(int systemMessageId);

        /// <summary>
        /// Get system message value by code
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="returnEmptyIfNotFound">A value indicating whether to empty string will be returned if a resource is not found and default value is set to empty string</param>
        /// <returns>System message value</returns>
        string GetMessage(string code, string defaultValue = "", bool returnEmptyIfNotFound = false);

        /// <summary>
        /// Gets all system messages
        /// </summary>
        /// <returns>System message values</returns>
        Dictionary<string, KeyValuePair<int, string>> GetAllMessageValues();

        /// <summary>
        /// Updates the system message
        /// </summary>
        /// <param name="SystemMessage">System message entity</param>
        void UpdateMessage(SystemMessage systemMessage);

        /// <summary>
        /// List messages
        /// </summary>
        /// <param name="grid">jqGrid parameters</param>
        /// <returns>Messages</returns>
        IPagedList<SystemMessage> GetPage(GridSettings grid);
    }
}