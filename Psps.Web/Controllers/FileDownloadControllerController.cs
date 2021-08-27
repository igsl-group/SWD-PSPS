using FluentValidation.Mvc;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Models.Dto;
using Psps.Models.Dto.Reports;
using Psps.Services.Lookups;
using Psps.Services.Posts;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Psps.Web.Controllers
{
    [RequireHttps]
    [PspsAuthorize]
    [RoutePrefix("FileDownload"), Route("{action=Donwload}")]
    public class FileDownloadController : BaseController
    {
        private readonly ICacheManager _cacheManager;

        public FileDownloadController(ICacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        [HttpGet, Route("{uniqueId}", Name = "FileDownload")]
        public ActionResult Download(string uniqueId)
        {
            var fileResultDto = Session[uniqueId] as ReportResultDto;
            if (fileResultDto == null || fileResultDto.ReportStream == null)
                return new EmptyResult();
            Session[uniqueId] = null;
            return File(fileResultDto.ReportStream, System.Net.Mime.MediaTypeNames.Application.Octet, Url.Encode(fileResultDto.FileName));
        }
    }
}