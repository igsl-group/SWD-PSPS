using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Data.Repositories;
using Psps.Models.Domain;
using Psps.Models.Dto.Psp;
using Psps.Services.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Psps.Services.PSPs
{
    public partial class PspCountEventsService : IPspCountEventsService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IPspCountEventsRepository _pspCountEventsRepository;

        #endregion Fields

        #region Ctor

        public PspCountEventsService(IEventPublisher eventPublisher, IPspCountEventsRepository pspCountEventsRepository)
        {
            this._eventPublisher = eventPublisher;
            this._pspCountEventsRepository = pspCountEventsRepository;
        }

        #endregion Ctor

        #region Methods

        public PspCountEvents GetByPspMasterId(int PspMasterId)
        {
            return _pspCountEventsRepository.Get(u => u.PspMasterId == PspMasterId);
        }

        #endregion Methods
    }
}