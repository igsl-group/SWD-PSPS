using FluentValidation.Mvc;
using Psps.Core;
using Psps.Core.Caching;
using Psps.Core.Common;
using Psps.Core.Helper;
using Psps.Core.Infrastructure;
using Psps.Core.JqGrid.Models;
using Psps.Data.Infrastructure;
using Psps.Models.Domain;
using Psps.Services.DocumentLibrary;
using Psps.Services.SystemMessages;
using Psps.Services.SystemParameters;
using Psps.Web.Core.ActionFilters;
using Psps.Web.Core.Controllers;
using Psps.Web.Core.Mvc;
using Psps.Web.ViewModels.DocumentLibraries;
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
    [RoutePrefix("DocumentLibrary"), Route("{action=index}")]
    public class DocumentLibraryController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        private readonly IParameterService _parameterService;
        private readonly ICacheManager _cacheManager;
        private readonly IDocumentLibraryService _documentLibraryService;
        private readonly IDocumentService _documentService;

        public DocumentLibraryController(ICacheManager cacheManager, IUnitOfWork unitOfWork,
            IMessageService messageService, IParameterService parameterService,
            IDocumentLibraryService documentLibraryService, IDocumentService documentService)
        {
            this._cacheManager = cacheManager;
            this._unitOfWork = unitOfWork;
            this._parameterService = parameterService;
            this._messageService = messageService;
            this._documentLibraryService = documentLibraryService;
            this._documentService = documentService;
        }

        [HttpGet, Route]
        [RuleSetForClientSideMessagesAttribute("default", "Create")]
        public ActionResult Index()
        {
            DocumentLibraryViewModel model = new DocumentLibraryViewModel();
            BuildFolderDropdown(model.DocumentLibraries, _documentLibraryService.GetAllDocumentLibrary(), null);
            return View(model);
        }

        [RuleSetForClientSideMessagesAttribute("default", "Create")]
        public PartialViewResult RenderDocumentModal()
        {
            DocumentViewModel model = new DocumentViewModel();
            return PartialView("_DocumentModal", model);
        }

        [HttpGet, Route("~/document/{documentId:int}/download", Name = "DownloadDocument")]
        public FileResult Download(int documentId)
        {
            // Get the Document record by given the ID
            var document = _documentService.GetDocumentById(documentId);

            // Get the root path of DocumentLibrary from DB
            var docLibPath = _parameterService.GetParameterByCode(Constant.SystemParameter.DOC_LIB_PATH);
            Ensure.NotNull(docLibPath, "No letter found with the specified code");

            // combine with the Relative Path to get the Absolute Path that the file actually stored at
            string rootPath = docLibPath.Value;
            string absolutePath = Path.Combine(rootPath, document.Path);
            // ( [Root Folder of DocumentLibrary] \ [Sub-Folder Directory] \ [File Name] )

            // Return the file for download
            return FileDownload(absolutePath, document.FileName);
        }

        private void BuildFolderDropdown(Dictionary<int, string> dictionary, IEnumerable<DocumentLibrary> list, int? parentId, int depth = 0)
        {
            var result = parentId.HasValue ? list.Where(x => x.Parent != null && x.Parent.DocumentLibraryId == parentId.Value) : list.Where(x => x.Parent == null);
            result = result.OrderBy(x => x.Name);

            foreach (DocumentLibrary docLib in result)
            {
                dictionary.Add(docLib.Id, string.Format("{0}{1}", string.Concat(Enumerable.Repeat("–", depth)) + " ", docLib.Name));

                BuildFolderDropdown(dictionary, list, docLib.Id, depth + 1);
            }
        }

        #region REST-like API

        [HttpGet, Route("~/api/document/{documentLibraryId:int}/list", Name = "ListDocument")]
        public JsonResult ListDocument(GridSettings grid, int documentLibraryId)
        {
            Ensure.Argument.NotNull(grid);

            var documents = _documentService.GetPage(grid, documentLibraryId);

            //converting in grid format
            var gridResult = new GridResult
            {
                TotalPages = documents.TotalPages,
                CurrentPageIndex = documents.CurrentPageIndex,
                TotalCount = documents.TotalCount,
                Data = (from l in documents
                        select new DocumentViewModel
                        {
                            Name = l.Name,
                            Remark = l.Remark,
                            UploadedBy = l.UploadedById,
                            UploadedOn = l.UploadedOn,
                            RowVersion = l.RowVersion,
                            DocumentId = l.DocumentId
                        }).ToArray()
            };

            return Json(new JsonResponse(true)
            {
                Data = gridResult
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("~/api/documentLibrary/new", Name = "NewDocumentLibrary")]
        public JsonResult NewDocumentLibrary([CustomizeValidator(RuleSet = "default,Create")] DocumentLibraryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var docLibRootPath = _parameterService.GetParameterByCode("DocLibPath");

            using (_unitOfWork.BeginTransaction())
            {
                var documentLibrary = _documentLibraryService.CreateDocumentLibrary(model.Name, model.SelectedDocumentLibraryId);
                BuildFolderDropdown(model.DocumentLibraries, _documentLibraryService.GetAllDocumentLibrary(), null);

                var dirPath = Path.Combine(docLibRootPath.Value, documentLibrary.Path);
                if (!System.IO.Directory.Exists(dirPath))
                    System.IO.Directory.CreateDirectory(dirPath);

                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = model.DocumentLibraries.ToArray()
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, Route("~/api/documentLibrary/{selectedDocumentLibraryId:int}/delete", Name = "DeleteDocumentLibrary")]
        public JsonResult DeleteDocumentLibrary(DocumentLibraryViewModel model)
        {

            var documentLibrary = _documentLibraryService.GetDocumentLibrary(model.SelectedDocumentLibraryId.Value);
            if ((documentLibrary.DocumentLibraries != null && documentLibrary.DocumentLibraries.Count()>0)|| (documentLibrary.Documents != null && documentLibrary.Documents.Count()>0))
            {
                return Json(new JsonResponse(true)
                {
                    Message = _messageService.GetMessage(SystemMessage.Error.DocumentLibrary.FolderCannotBeDeleted),
                    Success = false,
                }, JsonRequestBehavior.DenyGet);
            }

            using (_unitOfWork.BeginTransaction())
            {
                _documentLibraryService.DeleteDocumentLibrary(model.SelectedDocumentLibraryId.Value);
                BuildFolderDropdown(model.DocumentLibraries, _documentLibraryService.GetAllDocumentLibrary(), null);
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
                Data = model.DocumentLibraries.ToArray()
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet, Route("~/api/document/{documentId:int}", Name = "GetDocument")]
        public JsonResult GetDocument(int documentId)
        {
            var document = _documentService.GetDocumentById(documentId);

            return Json(new JsonResponse(true)
            {
                Data = new DocumentViewModel
                {
                    DocumentId = document.DocumentId,
                    DocumentLibraryId = document.DocumentLibrary.DocumentLibraryId,
                    Name = document.Name,
                    Remark = document.Remark,
                    RowVersion = document.RowVersion
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("~/api/document/new", Name = "NewDocument")]
        public JsonResult NewDocument([CustomizeValidator(RuleSet = "default,Create")]DocumentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }


            // Get the DocumentLibrary record by given the ID
            var documentLibrary = _documentLibraryService.GetDocumentLibrary(model.DocumentLibraryId);

            // Get the Current User 
            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;

            // Get the root path of DocumentLibrary from DB
            var docLibPath = _parameterService.GetParameterByCode(Constant.SystemParameter.DOC_LIB_PATH);
            Ensure.NotNull(docLibPath, "No letter found with the specified code");

            // Rename the file name by adding the current times
            string fileName = Path.GetFileName(model.Document.FileName);
            string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
            string generatedFileName = string.Format("{0}-{1}", time, fileName);

            // Set the root path by adding the Document Library Directory    ( [Root Folder of DocumentLibrary] \ [Sub-Folder Directory] )
            string rootPath = Path.Combine(docLibPath.Value, documentLibrary.Path);
            // Form the Relative Path for storing in DB                      ( [Sub-Folder Directory] \ [File Name] )
            // and Absolute Path for actually saving the file                ( [Root Folder of DocumentLibrary] \ [Sub-Folder Directory] \ [File Name] )
            string relativePath = Path.Combine(documentLibrary.Path, generatedFileName); ;
            string absolutePath = Path.Combine(rootPath, generatedFileName);

            // Create new Document row and fill the value
            var document = new Document
            {
                DocumentLibrary = documentLibrary,
                Name = model.Name,
                FileName = fileName,
                Path = relativePath,
                UploadedOn = DateTime.Now,
                UploadedById = currentUser.UserId,
                UploadedByPost = currentUser.PostId,
                Remark = model.Remark
            };

            using (_unitOfWork.BeginTransaction())
            {
                try
                {
                    // Save the file to the Absolute Path
                    if (CommonHelper.CreateFolderIfNeeded(rootPath))
                    {
                        model.Document.SaveAs(absolutePath);
                    }

                    // Insert record to DB and commit
                    _documentService.CreateDocument(document);
                    _unitOfWork.Commit();
                }
                catch (Exception)
                {
                    if (System.IO.File.Exists(absolutePath))
                        System.IO.File.Delete(absolutePath);

                    throw;
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordCreated),
                Data = document
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [HttpPost, Route("~/api/document/{documentId}/edit", Name = "EditDocument")]
        public JsonResult EditDocument(int documentId, [CustomizeValidator(RuleSet = "Update")]DocumentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), "text/html", JsonRequestBehavior.DenyGet);
            }
            
            // Get the Document record by given the ID
            var document = _documentService.GetDocumentById(model.DocumentId.Value);

            // Get the root path of DocumentLibrary from DB
            var docLibPath = _parameterService.GetParameterByCode(Constant.SystemParameter.DOC_LIB_PATH);
            Ensure.NotNull(docLibPath, "No letter found with the specified code");
            
            // Get the Current User 
            var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;

            // Fill the update values
            document.Name = model.Name;
            document.Remark = model.Remark;

            // Absolute Path for new file if needed 
            string absolutePath = "";

            using (_unitOfWork.BeginTransaction())
            {
                try
                {
                    // If new file need to be upload
                    if (model.Document != null)
                    {
                        // Reforming the file name by adding current time
                        string fileName = Path.GetFileName(model.Document.FileName);
                        string time = String.Format("{0:HHmmssFFFF}", DateTime.Now);
                        string generatedFileName = string.Format("{0}-{1}", time, fileName);

                        // Set the root path by adding the Document Library Directory    ( [Root Folder of DocumentLibrary] \ [Sub-Folder Directory] )
                        string rootPath = Path.Combine(docLibPath.Value, document.DocumentLibrary.Path);
                        // Form the Relative Path for storing in DB                      ( [Sub-Folder Directory] \ [File Name] )
                        // and Absolute Path for actually saving the file                ( [Root Folder of DocumentLibrary] \ [Sub-Folder Directory] \ [File Name] )
                        string relativePath = Path.Combine(document.DocumentLibrary.Path, generatedFileName); ;
                        absolutePath = Path.Combine(rootPath, generatedFileName);

                        // Save the new file
                        if (CommonHelper.CreateFolderIfNeeded(rootPath))
                        {
                            model.Document.SaveAs(absolutePath);
                        }

                        // Form the path of the old file
                        string absolutePathOfOldFile = Path.Combine(docLibPath.Value, document.Path);

                        // Delete the old file
                        if (System.IO.File.Exists(absolutePathOfOldFile))
                        {
                            System.IO.File.Delete(absolutePathOfOldFile);
                        }

                        // Replace with the new file
                        document.FileName = fileName;
                        document.Path = relativePath;
                        document.UploadedOn = DateTime.Now;
                        document.UploadedById = currentUser.UserId;
                        document.UploadedByPost = currentUser.PostId;
                    }

                    // Update DB record and commit
                    _documentService.UpdateDocument(document);
                    _unitOfWork.Commit();
                }
                catch (Exception)
                {
                    // Delete uploaded file while exception 
                    if (System.IO.File.Exists(absolutePath))
                        System.IO.File.Delete(absolutePath);

                    throw;
                }
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                Data = document
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [HttpPost, Route("~/api/document/{documentId}/delete", Name = "DeleteDocument")]
        public JsonResult DeleteDocument(int documentId, [CustomizeValidator(RuleSet = "Delete")]DocumentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            // Get the Document record by given the ID
            var document = _documentService.GetDocumentById(model.DocumentId.Value);

            // Get the root path of DocumentLibrary from DB
            var docLibPath = _parameterService.GetParameterByCode(Constant.SystemParameter.DOC_LIB_PATH);
            Ensure.NotNull(docLibPath, "No letter found with the specified code");

            // combine with the Relative Path to get the Absolute Path that the file actually stored at
            string rootPath = docLibPath.Value;
            string absolutePath = Path.Combine(rootPath, document.Path);
            // ( [Root Folder of DocumentLibrary] \ [Sub-Folder Directory] \ [File Name] )

            using (_unitOfWork.BeginTransaction())
            {
                // Delete the record in DB (set IsDeleted flag)
                document.IsDeleted = true;
                document.RowVersion = model.RowVersion;                
                _documentService.DeleteDocument(document);

                // Delete the Attachment file if exists
                if (System.IO.File.Exists(absolutePath))
                {
                    System.IO.File.Delete(absolutePath);
                }

                // Commit Delete
                _unitOfWork.Commit();
            }

            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordDeleted),
                Data = document
            }, "text/html", JsonRequestBehavior.DenyGet);
        }

        [HttpGet, Route("~/api/document/{documentLibraryId:int}/getDocumentLibrary", Name = "GetDocumentLibrary")]
        public JsonResult GetDocumentLibrary(int documentLibraryId)
        {
            var documentLibrary = _documentLibraryService.GetDocumentLibrary(documentLibraryId);

            return Json(new JsonResponse(true)
            {
                Data = new DocumentViewModel
                {
                    DocumentLibraryId = documentLibrary.DocumentLibraryId,
                    Name = documentLibrary.Name,
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("~/api/documentLibrary/update", Name = "UpdateDocumentLibrary")]
        public JsonResult UpdateDocumentLibrary([CustomizeValidator(RuleSet = "default,Create")] DocumentLibraryViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return Json(JsonResponseFactory.ErrorResponse(ModelState), JsonRequestBehavior.DenyGet);
            }

            var documentLibrary = _documentLibraryService.GetDocumentLibrary(model.SelectedDocumentLibraryId.Value);
            documentLibrary.Name = model.Name;
            using (_unitOfWork.BeginTransaction())
            {
                _documentLibraryService.UpdateDocumentLibrary(documentLibrary);
                _unitOfWork.Commit();
            }
            BuildFolderDropdown(model.DocumentLibraries, _documentLibraryService.GetAllDocumentLibrary(), null);
            return Json(new JsonResponse(true)
            {
                Message = _messageService.GetMessage(SystemMessage.Info.RecordUpdated),
                Data = model.DocumentLibraries.ToArray()
            }, JsonRequestBehavior.DenyGet);
        }
        #endregion REST-like API
    }
}