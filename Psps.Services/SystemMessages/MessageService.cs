using Psps.Core.Caching;
using Psps.Core.Helper;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Services.Events;
using Psps.Services.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psps.Services.SystemMessages
{
    public partial class MessageService : IMessageService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string SYSTEMMESSAGE_ALL_KEY = "Psps.systemmessage.all";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : code
        /// </remarks>
        private const string SYSTEMMESSAGE_BY_CODE_KEY = "Psps.systemmessage.{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string SYSTEMMESSAGE_PATTERN_KEY = "Psps.systemmessage.";

        #endregion Constants

        #region Fields

        private readonly ICacheManager _cacheManager;

        private readonly IEventPublisher _eventPublisher;

        private readonly ISystemMessageRepository _systemMessageRepository;

        #endregion Fields

        #region Ctor

        public MessageService(ICacheManager cacheManager, IEventPublisher eventPublisher, ISystemMessageRepository systemMessageRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._systemMessageRepository = systemMessageRepository;
        }

        #endregion Ctor

        #region Methods

        public SystemMessage GetMessageById(int systemMessageId)
        {
            if (systemMessageId == 0)
                return null;

            return _systemMessageRepository.GetById(systemMessageId);
        }

        public string GetMessage(string code, string defaultValue = "", bool returnEmptyIfNotFound = false)
        {
            string result = "";

            if (string.IsNullOrEmpty(code))
                return defaultValue;

            //load all records (we know they are cached)
            var messages = GetAllMessageValues();
            if (messages.ContainsKey(code))
            {
                result = messages[code].Value;
            }

            if (String.IsNullOrEmpty(result))
            {
                //gradual loading
                string key = string.Format(SYSTEMMESSAGE_BY_CODE_KEY, code);
                string message = _cacheManager.Get(key, () =>
                {
                    var query = from m in _systemMessageRepository.Table
                                where m.Code == code
                                select m.Value;
                    return query.FirstOrDefault();
                });

                if (message != null)
                    result = message;
            }

            if (String.IsNullOrEmpty(result))
            {
                if (!String.IsNullOrEmpty(defaultValue))
                {
                    result = defaultValue;
                }
                else
                {
                    if (!returnEmptyIfNotFound)
                        result = code;
                }
            }

            return result;
        }

        public Dictionary<string, KeyValuePair<int, string>> GetAllMessageValues()
        {
            string key = SYSTEMMESSAGE_ALL_KEY;
            return _cacheManager.Get(key, () =>
            {
                //format: <code, <id, value>>
                return this._systemMessageRepository.Table
                    .Select(m => new { m.SystemMessageId, m.Code, m.Value })
                    .ToDictionary(k => k.Code, v => new KeyValuePair<int, string>(v.SystemMessageId, v.Value));
            });
        }

        public void UpdateMessage(SystemMessage systemMessage)
        {
            Ensure.Argument.NotNull(systemMessage, "systemMessage");

            _systemMessageRepository.Update(systemMessage);

            //cache
            _cacheManager.RemoveByPattern(SYSTEMMESSAGE_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated<SystemMessage>(systemMessage);
        }

        public Core.Models.IPagedList<SystemMessage> GetPage(Core.JqGrid.Models.GridSettings grid)
        {
            return _systemMessageRepository.GetPage(grid);
        }

        #endregion Methods
    }
}