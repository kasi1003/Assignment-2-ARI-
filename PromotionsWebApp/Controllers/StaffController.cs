using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PromotionsWebApp.Core.Interfaces;
using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
using PromotionsWebApp.Models;
using PromotionsWebApp.Models.Qualification;
using PromotionsWebApp.Models.Staff;
using PromotionsWebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PromotionsWebApp.ViewComponents.EducationFormViewComponent;
using static PromotionsWebApp.ViewComponents.PersonalDetailFormViewComponent;
using static PromotionsWebApp.ViewComponents.PublicationFormViewComponent;

namespace PromotionsWebApp.Controllers
{
    public class StaffController : BaseController
    {
        private readonly ILogger<StaffController> _logger;

        public StaffController(ILogger<StaffController> logger, IServiceScopeFactory scope) : base(scope)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string currentFilter = null,
                                                string searchString = null,
                                                int? pageNumber = null)
        {
            StaffsVM model = new StaffsVM();
            try
            {
                var staffList = await _staffRepo.AllIncluding(x => x.Jobs);
                ViewData["CurrentFilter"] = searchString;
                if (!String.IsNullOrEmpty(searchString))
                {
                    staffList = staffList.Where(s => s.User.ToString().ToUpper().Contains(searchString.ToUpper())).ToList();
                }
                foreach (Staff staff in staffList)
                {
                    staff.User = await _userManager.FindByIdAsync(staff.UserId);
                    var jobs = await _jobRepo.FindByIncluding(x => x.StaffId == staff.Id, x => x.Rank);
                    staff.Jobs = jobs.ToList();
                }

                if (searchString != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                var list = new List<StaffVM>();
                foreach (Staff staff in staffList)
                {
                    var staffToAdd = new StaffVM
                    {
                        Id = staff.Id,
                        Username = staff.User.ToString(),
                        UserId = staff.UserId,
                        ProfileImage = staff.User.ProfileImage
                    };
                    if (staff.Jobs != null && staff.Jobs.Any())
                    {
                        foreach (StaffJob jobs in staff.Jobs)
                        {
                            if (jobs.IsCurrent)
                            {
                                staffToAdd.StaffJob = jobs.Rank.Name;
                                staffToAdd.Department = jobs.Department;
                                staffToAdd.Faculty = jobs.Faculty;
                                staffToAdd.DateEmployed = jobs.DateEmployed;
                                break;
                            }
                        }
                    }

                    list.Add(staffToAdd);
                }
                model.Staffs = new PaginatedList<StaffVM>(list.OrderBy(x => x.Username).ToList(), pageNumber ?? 1, 7);
                string toastMessage = "";
                if (TempData.ContainsKey("Toast"))
                    toastMessage = TempData["Toast"].ToString();
                ViewData["Toast"] = toastMessage;
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Profile(int staffId=0)
        {
            StaffProfileVM model = new StaffProfileVM();
            if(staffId == 0)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    var stff = await _staffRepo.FindBy(x => x.UserId == user.Id);
                    staffId = stff.First().Id;
                }
            }
            try
            {
                Staff staff = await _staffRepo.GetSingle(x => x.Id == staffId, x => x.Qualifications, x => x.Jobs, x => x.Publications, x => x.SupportingDocuments);
                foreach (StaffJob job in staff.Jobs)
                {
                    job.Rank = await _rankRepo.GetSingle(job.RankId);
                }
                User user = await _userManager.FindByIdAsync(staff.UserId);
                user.Department = await _departmentRepo.GetSingle(x => x.Id == user.DepartmentId, x => x.Faculty);
                model.Id = staff.Id;
                model.Department = user.Department.Name;
                model.Faculty = user.Department.Faculty.Name;
                model.StaffNr = staff.StaffNr;
                model.Rank = staff.Jobs.Where(x => x.IsCurrent).First().Rank.Name;
                model.ProfileImage = user.ProfileImage;
                model.Name = user.ToString();
                model.Qualifications = new List<QualificationVM>();
                model.Jobs = new List<StaffJobVM>();
                model.Publications = new List<PublicationVM>();
                model.SupportingDocumentsId = staff.SupportDocumentsId;
                model.SupportingDocuments = new SupportingDocumentsVM
                {
                    IdentityDocumentId = staff.SupportingDocuments.IdentityDocumentId,
                    CVId = staff.SupportingDocuments.CVId,
                    ScholarshipInTeachingFormId = staff.SupportingDocuments.ScholarshipInTeachingFormId,
                    CommunityServiceFormId = staff.SupportingDocuments.CommunityServiceFormId,
                    PeerEvalFormId = staff.SupportingDocuments.PeerEvalFormId,
                    StudentEvalFormId = staff.SupportingDocuments.StudentEvalFormId,
                    SelfScoredEvalutionFormId = staff.SupportingDocuments.SelfScoredEvaluationFormId
                };
                if (staff.Publications != null || staff.Publications.Any())
                {
                    foreach (Publication pub in staff.Publications)
                    {
                        PublicationVM item = new PublicationVM
                        {
                            Id = pub.Id,
                            Name = pub.Name,
                            Authors = pub.Authors,
                            PublicationType = pub.PublicationType,
                            StaffId = pub.StaffId,
                            YearObtained = pub.YearObtained
                        };
                        model.Publications.Add(item);
                    }
                }
                if (staff.Qualifications != null || staff.Qualifications.Any())
                {
                    foreach (Qualification q in staff.Qualifications)
                    {
                        QualificationVM quali = new QualificationVM
                        {
                            Id = q.Id,
                            StaffId = q.StaffId,
                            Name = q.Name,
                            Institution = q.Institution,
                            NQFLevel = q.NQFLevel,
                            CertificateDocumentId = q.CertificateDocumentId,
                            YearObtained = q.YearObtained
                        };
                        model.Qualifications.Add(quali);
                    }

                }
                if (staff.Jobs != null || staff.Jobs.Any())
                {
                    foreach (StaffJob job in staff.Jobs)
                    {
                        StaffJobVM j = new StaffJobVM
                        {
                            Id = job.Id,
                            StaffId = job.StaffId,
                            DateEmployed = job.DateEmployed,
                            Department = job.Department,
                            Faculty = job.Faculty,
                            IsCurrent = job.IsCurrent,
                            Name = job.Rank.Name,
                            DateLeft = job.DateLeft
                        };
                        model.Jobs.Add(j);
                    }
                }
                string toastMessage = "";
                if (TempData.ContainsKey("Toast"))
                    toastMessage = TempData["Toast"].ToString();
                ViewData["Toast"] = toastMessage;
            }
            catch (Exception rx)
            {

            }

            return View(model);
        }

        //Add Qualification
        public async Task<IActionResult> UpdateEducation(EducationViewModel model)
        {
            try
            {
                Qualification item = new Qualification();
                item.StaffId = model.StaffId;
                item.Name = model.Name;
                item.Institution = model.Institution;
                item.YearObtained = model.YearObtained;
                item.NQFLevel = model.NQFLevel;
                item.CertificateDocument = new Document
                {
                    FileName = "StaffCert_" + item.StaffId + "_" + item.Name,
                    SupportingDocumentsId = null,
                    Content = await model.QualificationDocument.GetBytes()
                };

                await _qualificationRepo.Add(item);


                var quals = await _qualificationRepo.FindByIncluding(x => x.StaffId == model.StaffId, x => x.CertificateDocument);
                if (quals != null)
                {
                    List<byte[]> pdfs = new List<byte[]>();
                    foreach (var qual in quals)
                    {
                        pdfs.Add(qual.CertificateDocument.Content);
                    }
                    var combinedPdf = await PdfUtilities.CombinePDF(pdfs);
                    var supportDoc = await _supportDocumentsRepo.GetSingle(x => x.StaffId == model.StaffId);
                    if (supportDoc != null)
                    {
                        if (supportDoc.QualificationsDocumentId.HasValue)
                        {
                            supportDoc.QualificationsDocument = await _documentRepo.GetSingle(supportDoc.QualificationsDocumentId.Value);
                            supportDoc.QualificationsDocument.Content = combinedPdf;
                        }
                        else
                        {
                            supportDoc.QualificationsDocument = new Document
                            {
                                FileType = DocumentFileTypeEnum.PDF,
                                Content = combinedPdf,
                                FileName = "QualificationsDoc",
                                AddedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now
                            };
                        }
                        await _supportDocumentsRepo.Update(supportDoc);
                    }
                }
                TempData["Toast"] = "New Qualification Details has succesfully been added";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Profile", new { staffId = model.StaffId });
            }
            return RedirectToAction("Profile", new { staffId = model.StaffId });
        }
        public async Task<IActionResult> DeleteQualification(int qualificationId)
        {
            int staffId = 0;
            try
            {
                var qual = await _qualificationRepo.GetSingle(qualificationId);
                if (qual != null)
                {
                    staffId = qual.StaffId;
                    await _qualificationRepo.Delete(qualificationId);
                    //Todo CombineQualifications Document
                    var quals = await _qualificationRepo.FindByIncluding(x => x.StaffId == staffId, x => x.CertificateDocument);
                    if (quals != null)
                    {
                        List<byte[]> pdfs = new List<byte[]>();
                        if (quals.Any())
                        {
                            foreach (var ql in quals)
                            {
                                pdfs.Add(ql.CertificateDocument.Content);
                            }

                            var combinedPdf = await PdfUtilities.CombinePDF(pdfs);
                            var supportDoc = await _supportDocumentsRepo.GetSingle(x => x.StaffId == staffId);
                            if (supportDoc != null)
                            {
                                if (supportDoc.QualificationsDocumentId.HasValue)
                                {
                                    supportDoc.QualificationsDocument = await _documentRepo.GetSingle(supportDoc.QualificationsDocumentId.Value);
                                    supportDoc.QualificationsDocument.Content = combinedPdf;
                                }
                                else
                                {
                                    supportDoc.QualificationsDocument = new Document
                                    {
                                        FileType = DocumentFileTypeEnum.PDF,
                                        Content = combinedPdf,
                                        FileName = "QualificationsDoc",
                                        AddedDate = DateTime.Now,
                                        ModifiedDate = DateTime.Now
                                    };
                                }
                                await _supportDocumentsRepo.Update(supportDoc);
                            }
                        }
                        else
                        {
                            var supportDoc = await _supportDocumentsRepo.GetSingle(x => x.StaffId == staffId);
                            if(supportDoc.QualificationsDocumentId.HasValue)
                            {
                                await _documentRepo.Delete(supportDoc.QualificationsDocumentId.Value);
                                supportDoc.QualificationsDocumentId = null;
                                await _supportDocumentsRepo.Update(supportDoc);
                            }
                        }
                    }
                }

                TempData["Toast"] = "Qualification has succesfully been deleted";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Profile", new { staffId = staffId });
            }
            return RedirectToAction("Profile", new { staffId = staffId });
        }
        public async Task<IActionResult> UpdatePublication(PublicationViewModel model)
        {
            try
            {
                Publication item = new Publication();
                item.StaffId = model.StaffId;
                item.Name = model.Name;
                item.Authors = model.Authors;
                item.YearObtained = model.YearObtained;
                item.PublicationType = model.PublicationType;

                await _publicationRepo.Add(item);
                TempData["Toast"] = "New Publication Details has succesfully been added";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Profile", new { staffId = model.StaffId });
            }
            return RedirectToAction("Profile", new { staffId = model.StaffId });
        }
        public async Task<IActionResult> DeletePublication(int publicationId)
        {
            int staffId = 0;
            try
            {
                var qual = await _publicationRepo.GetSingle(publicationId);
                if (qual != null)
                {
                    staffId = qual.StaffId;
                    await _publicationRepo.Delete(publicationId);
                    TempData["Toast"] = "Publication has succesfully been deleted";
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Profile", new { staffId = staffId });
            }
            return RedirectToAction("Profile", new { staffId = staffId });
        }
        public async Task<IActionResult> UpdatePersonalDetails(PersonalDetailViewModel model)
        {
            try
            {
                Staff staff = await _staffRepo.GetSingle(model.StaffId);
                if (staff != null)
                {
                    staff.User = await _userManager.FindByIdAsync(staff.UserId);

                    if (model.StaffNr != "" || model.StaffNr != null)
                    {
                        staff.StaffNr = model.StaffNr;
                    }
                    if (model.FirstName != null || model.FirstName != "")
                    {
                        staff.User.FirstName = model.FirstName;
                    }
                    if (model.Surname != null || model.Surname != "")
                    {
                        staff.User.LastName = model.Surname;
                    }
                    if (model.Email != null || model.Email != "")
                    {
                        staff.User.Email = model.Email;
                    }
                    if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                    {
                        staff.User.ProfileImage = await model.ProfileImage.GetBytes();
                    }
                    if (model.Title > 0)
                    {
                        staff.User.Title = model.Title;
                    }
                    TempData["Toast"] = "Personal Details has succesfully been updated";
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Profile", new { staffId = model.StaffId });
            }
            return RedirectToAction("Profile", new { staffId = model.StaffId });
        }
    }
}
