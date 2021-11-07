using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
using PromotionsWebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PromotionsWebApp.ViewComponents.UploadDocumentsViewComponent;

namespace PromotionsWebApp.Controllers
{
    public class DocumentController : BaseController
    {
        public DocumentController(IServiceScopeFactory factory) : base(factory)
        {

        }

        [HttpGet]
        public async Task<JsonResult> GetDocument([FromQuery] int docId)
        {
            try
            {
                var doc = await _documentRepo.GetSingle(x => x.Id == docId);
                if (doc != null)
                {
                    var content = doc.Content;
                    return Json(content);
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(true);
        }

        [HttpGet]
        public async Task<JsonResult> Delete([FromQuery] int docId)
        {
            try
            {
                var doc = await _documentRepo.GetSingle(x => x.Id == docId);
                if (doc != null)
                    await _documentRepo.Delete(doc.Id);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument(UploadDocumentsViewModel model)
        {
            int staffId = 0;
            try
            {
                var supportDoc = await _supportDocumentsRepo.GetSingle(model.SupportingsDocumentId);
                if (supportDoc != null)
                {
                    staffId = supportDoc.StaffId;
                    switch (model.UploadType)
                    {
                        case UploadTypeEnum.CV:
                            supportDoc.CV = new Document
                            {
                                FileName = "Staff_" + staffId + "_CV",
                                Content = await model.UploadFile.GetBytes(),
                                SupportingDocumentsId = model.SupportingsDocumentId
                            };
                            await _documentRepo.Add(supportDoc.CV);
                            supportDoc.CVId = supportDoc.CV.Id;
                            break;
                        case UploadTypeEnum.ID:
                            supportDoc.IdentityDocument = new Document
                            {
                                FileName = "Staff_" + staffId + "_ID",
                                Content = await model.UploadFile.GetBytes(),
                                SupportingDocumentsId = model.SupportingsDocumentId
                            };
                            await _documentRepo.Add(supportDoc.IdentityDocument);
                            supportDoc.IdentityDocumentId = supportDoc.IdentityDocument.Id;
                            break;
                        case UploadTypeEnum.StudentEval:
                            supportDoc.StudentEvalForm = new Document
                            {
                                FileName = "Staff_" + staffId + "_SE",
                                Content = await model.UploadFile.GetBytes(),
                                SupportingDocumentsId = model.SupportingsDocumentId
                            };
                            await _documentRepo.Add(supportDoc.StudentEvalForm);
                            supportDoc.StudentEvalFormId = supportDoc.StudentEvalForm.Id;
                            break;
                        case UploadTypeEnum.PeerEval:
                            supportDoc.PeerEvalForm = new Document
                            {
                                FileName = "Staff_" + staffId + "_PE",
                                Content = await model.UploadFile.GetBytes(),
                                SupportingDocumentsId = model.SupportingsDocumentId
                            };
                            await _documentRepo.Add(supportDoc.PeerEvalForm);
                            supportDoc.PeerEvalFormId = supportDoc.PeerEvalForm.Id;
                            break;
                        case UploadTypeEnum.CommunityService:
                            supportDoc.CommunityServiceForm = new Document
                            {
                                FileName = "Staff_" + staffId + "_CS",
                                Content = await model.UploadFile.GetBytes(),
                                SupportingDocumentsId = model.SupportingsDocumentId
                            };
                            await _documentRepo.Add(supportDoc.CommunityServiceForm);
                            supportDoc.CommunityServiceFormId = supportDoc.CommunityServiceForm.Id;
                            break;
                        case UploadTypeEnum.ScholarshipInTeaching:
                            supportDoc.ScholarshipInTeachingForm = new Document
                            {
                                FileName = "Staff_" + staffId + "_ST",
                                Content = await model.UploadFile.GetBytes(),
                                SupportingDocumentsId = model.SupportingsDocumentId
                            };
                            await _documentRepo.Add(supportDoc.ScholarshipInTeachingForm);
                            supportDoc.ScholarshipInTeachingFormId = supportDoc.ScholarshipInTeachingForm.Id;
                            break;
                    }
                }

                TempData["Toast"] = "Document has been successfuly upload";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Profile", new { staffId = staffId });
            }
            return RedirectToAction("Profile", new { staffId = staffId });
        }
    }
}
