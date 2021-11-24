using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
using PromotionsWebApp.Models;
using PromotionsWebApp.Models.Promotion;
using PromotionsWebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Controllers
{
    public class PromotionsController : BaseController
    {
        private readonly ILogger<PromotionsController> _logger;

        public PromotionsController(ILogger<PromotionsController> logger, IServiceScopeFactory scope) : base(scope)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string userId = null,
            string currentFilter = null,
                                                string searchString = null,
                                                int? pageNumber = null)
        {
            PaginatedList<Promotion> model;
            if (userId == null)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                    userId = user.Id;
            }
            try
            {
                var promotions = await _promotionRepo.AllIncluding(x => x.Evaluations, x => x.Staff, x => x.Rank);

                List<Promotion> proms = promotions.ToList();
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    
                    foreach (Promotion pro in proms)
                    {
                        pro.Staff.User = await _userManager.FindByIdAsync(pro.Staff.UserId);
                        pro.Staff.User.Faculty = await _facultyRepo.GetSingle(pro.Staff.User.FacultyId.Value);
                        pro.Staff.User.Department = await _departmentRepo.GetSingle(pro.Staff.User.DepartmentId.Value);
                        var pub = await _publicationRepo.FindByIncluding(x => x.StaffId == pro.Staff.Id);
                        pro.Staff.Publications = pub.ToList();
                        var qual = await _qualificationRepo.FindByIncluding(x => x.StaffId == pro.Staff.Id);
                        var sup = await _supportDocumentsRepo.GetSingle(x => x.StaffId == pro.Staff.Id);
                        pro.Staff.Qualifications = qual.ToList();
                        pro.Staff.SupportingDocuments = sup;
                        var eval = await _promotionDecisionRepo.FindByIncluding(x => x.Id == pro.Id);
                        var jobs = await _jobRepo.FindByIncluding(x => x.Id == pro.Staff.Id, x => x.Rank);
                        pro.Staff.Jobs = jobs.ToList();
                        pro.Evaluations = eval.ToList();

                    }
                    switch (user.Role)
                    {
                        case UserRoleEnum.Staff:
                            var staff = await _staffRepo.GetSingle(x => x.UserId == user.Id);
                            proms = proms.Where(x => x.StaffId == staff.Id).ToList();
                            break;
                        case UserRoleEnum.HOD:
                            proms = proms.Where(x => x.Staff.User.DepartmentId == user.DepartmentId).ToList();
                            break;
                        case UserRoleEnum.Dean:
                            proms = proms.Where(x => x.Staff.User.FacultyId == user.FacultyId).ToList();
                            break;
                        case UserRoleEnum.IFEC:
                            proms = proms.Where(x => x.Status == PromotionStatusEnum.IFEC).ToList();
                            break;
                        case UserRoleEnum.PSPC:
                            proms = proms.Where(x => x.Status == PromotionStatusEnum.PSPC).ToList();
                            break;
                        case UserRoleEnum.VC:
                            proms = proms.Where(x => x.Status == PromotionStatusEnum.VC).ToList();
                            break;
                    }
                    ViewData["CurrentFilter"] = searchString;
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        proms = proms.Where(s => s.Staff.User.ToString().ToUpper().Contains(searchString.ToUpper())).ToList();
                    }
                    model = new PaginatedList<Promotion>(proms, pageNumber ?? 1, 7);
                    string toastMessage = "";
                    if (TempData.ContainsKey("Toast"))
                        toastMessage = TempData["Toast"].ToString();
                    ViewData["Toast"] = toastMessage;
                    ViewData["UserId"] = userId;
                    return View(model);
                }


            }
            catch (Exception ex)
            {

            }
            return View(null);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int promotionId)
        {
            Promotion pro;
            try
            {
                pro = await _promotionRepo.GetSingle(x => x.Id == promotionId, x => x.Evaluations, x => x.Staff, x => x.Rank);

                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {

                    pro.Staff.User = await _userManager.FindByIdAsync(pro.Staff.UserId);
                    pro.Staff.User.Faculty = await _facultyRepo.GetSingle(pro.Staff.User.FacultyId.Value);
                    pro.Staff.User.Department = await _departmentRepo.GetSingle(pro.Staff.User.DepartmentId.Value);
                    var pub = await _publicationRepo.FindByIncluding(x => x.StaffId == pro.Staff.Id);
                    pro.Staff.Publications = pub.ToList();
                    var qual = await _qualificationRepo.FindByIncluding(x => x.StaffId == pro.Staff.Id);
                    var sup = await _supportDocumentsRepo.GetSingle(x => x.StaffId == pro.Staff.Id);
                    pro.Staff.Qualifications = qual.ToList();
                    pro.Staff.SupportingDocuments = sup;
                    var eval = await _promotionDecisionRepo.FindByIncluding(x => x.PromotionId == pro.Id);
                    var jobs = await _jobRepo.FindByIncluding(x => x.Id == pro.Staff.Id, x => x.Rank);
                    pro.Staff.Jobs = jobs.ToList();
                    pro.Evaluations = eval.ToList();
                }

                string toastMessage = "";
                if (TempData.ContainsKey("Toast"))
                    toastMessage = TempData["Toast"].ToString();
                ViewData["Toast"] = toastMessage;
                PromDetailVM model = new PromDetailVM();
                model.Promotion = pro;
                model.RejectDec = new PromDecide();
                model.RejectDec.PromotionId = pro.Id;
                model.AcceptDec = new PromDecide();
                model.AcceptDec.PromotionId = pro.Id;
                return View(model);
            }
            catch (Exception ex)
            {

            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Approve(PromDetailVM model)
        {
            try
            {
                var promotion = await _promotionRepo.GetSingle(x => x.Id == model.AcceptDec.PromotionId, x => x.Evaluations,x=>x.Staff);
                if (promotion != null)
                {
                   
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                    if (promotion.Evaluations == null)
                        promotion.Evaluations = new List<PromotionDecision>();
                    var des = new PromotionDecision
                    {
                        PromotionId = promotion.Id,
                        Decision = PromotionStageApprovalEnum.Accepted,
                        Remarks = model.AcceptDec.Remarks,
                        UserId = user.Id,
                        UserName = user.ToString(),
                        Role = user.Role.ToString()

                    };
                    if (model.AcceptDec.Submission != null)
                    {
                        des.SubmissionDocument = new Document
                        {
                            FileName = model.AcceptDec.Submission.FileName,
                            Content = await model.AcceptDec.Submission.GetBytes()
                        };
                    }
                    promotion.Evaluations.Add(des);
                    var stffJobs = await _jobRepo.FindByIncluding(x => x.StaffId == promotion.StaffId, x => x.Rank);
                    var current = stffJobs.Where(x => x.IsCurrent).FirstOrDefault();
                    int status = (int)promotion.Status;
                    if(current.Rank.Name == "Associate Professor")
                    {
                        if(promotion.Status == PromotionStatusEnum.Dean)
                        {
                            status = (int)PromotionStatusEnum.PSPC;
                        }
                        else
                        {
                            status++;
                        }
                    }
                    else 
                    {
                        if (promotion.Status == PromotionStatusEnum.Dean)
                        {
                            status = (int)PromotionStatusEnum.IFEC;
                        }
                        else if(promotion.Status == PromotionStatusEnum.IFEC)
                        {
                            status = (int)PromotionStatusEnum.VC;
                        }
                        else
                        {
                            status++;
                        }
                    }

                    promotion.Status = (PromotionStatusEnum)status;
                
                    await _promotionRepo.Update(promotion);
                    promotion.Staff.User = await _userManager.FindByIdAsync(promotion.Staff.UserId);
                    if (promotion.Status == PromotionStatusEnum.Approved)
                    {
                        var stff = await _staffRepo.GetSingle(x => x.Id == promotion.StaffId);
                        var jobs = await _jobRepo.FindBy(x=>x.StaffId==stff.Id);
                        stff.Jobs = jobs.ToList();
                        stff.User = await _userManager.FindByIdAsync(stff.UserId);
                        stff.User.Faculty = await _facultyRepo.GetSingle(stff.User.FacultyId.Value);
                        stff.User.Department = await _departmentRepo.GetSingle(stff.User.DepartmentId.Value);
                        stff.Jobs.Where(x => x.IsCurrent == true).FirstOrDefault().DateLeft = DateTime.Now;
                        stff.Jobs.Where(x => x.IsCurrent == true).FirstOrDefault().IsCurrent = false;
                        var staffJob = new StaffJob
                        {
                            IsCurrent = true,
                            RankId = promotion.RankId,
                            Department = stff.User.Department.Name,
                            Faculty = stff.User.Faculty.Name,
                            DateEmployed = DateTime.Now
                        };
                        stff.User = null;
                        stff.Jobs.Add(staffJob);

                        await _staffRepo.Update(stff);
                        var link = Url.Action("Detail", "Promotions", new { promotionId = promotion.Id }, Request.Scheme);
                        var stffUser = await _userManager.FindByIdAsync(promotion.Staff.UserId);
                        
                        await _emailSender.SendPromotionApproved(stffUser.Email, link);                       
                    }
                    if (promotion.Status != PromotionStatusEnum.Approved && promotion.Status != PromotionStatusEnum.Rejected)
                    {
                        var allUser = _userRepo.Get();
                        if (promotion.Status == PromotionStatusEnum.Dean)
                        {
                            var promStatus = promotion.Status.ToString();
                            var sendUsers = allUser.ToList().Where(x => x.Role.ToString() == promStatus);
                            var facId = promotion.Staff.User.FacultyId;
                            string email = sendUsers.ToList().Where(x => x.FacultyId == facId).FirstOrDefault().Email;
                            var linkT = Url.Action("Detail", "Promotions", new { promotionId = promotion.Id }, Request.Scheme);
                            await _emailSender.SendInboxNotification(email, linkT);
                        }
                        else
                        {
                            var promStatus = promotion.Status.ToString();
                            var sendUsers = allUser.ToList().Where(x => x.Role.ToString() == promStatus);
                            string email = sendUsers.ToList().FirstOrDefault().Email;
                            var linkT = Url.Action("Detail", "Promotions", new { promotionId = promotion.Id }, Request.Scheme);
                            await _emailSender.SendInboxNotification(email, linkT);
                        }

                    }
                    //Todo Send Email
                    ViewData["Toast"] = "Promotion has been Approved";
                    return RedirectToAction("Index", "Promotions", new { userId = user.Id });
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Detail", "Promotions", new { promotionId = model.Promotion.Id });
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int promotionId)
        {
            string userId = "";
            try
            {
                await _promotionRepo.Delete(promotionId);
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                userId = user.Id;
                ViewData["Toast"] = "Promotion has been deleted";
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index", "Promotions", new { userId = userId });
        }
        [HttpPost]
        public async Task<IActionResult> Reject(PromDetailVM model)
        {
            string userId = "";
            try
            {
                var promotion = await _promotionRepo.GetSingle(x => x.Id == model.RejectDec.PromotionId, x => x.Evaluations,x=>x.Staff);
                if (promotion != null)
                {
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                    if (promotion.Evaluations == null)
                        promotion.Evaluations = new List<PromotionDecision>();
                    var des = new PromotionDecision
                    {
                        PromotionId = promotion.Id,
                        Decision = PromotionStageApprovalEnum.Rejected,
                        Remarks = model.RejectDec.Remarks,
                        UserId = user.Id,

                    };
                    if (model.RejectDec.Submission != null)
                    {
                        des.SubmissionDocument = new Document
                        {
                            FileName = model.RejectDec.Submission.FileName,
                            Content = await model.RejectDec.Submission.GetBytes()
                        };
                    }
                    promotion.Evaluations.Add(des);
                    promotion.Status = PromotionStatusEnum.Rejected;
                    await _promotionRepo.Update(promotion);
                    //Todo Send Email
                    var link = Url.Action("Detail", "Promotions", new { promotionId = promotion.Id }, Request.Scheme);
                    var stffUser = await _userManager.FindByIdAsync(promotion.Staff.UserId);
                        await _emailSender.SendPromotionRejected(stffUser.Email, link);
                    ViewData["Toast"] = "Promotion has been rejected";
                    return RedirectToAction("Index", "Promotions", new { userId = user.Id });
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Detail", "Promotions", new { promotionId = model.RejectDec.PromotionId });
        }
        [HttpGet]
        public async Task<IActionResult> Apply(int staffId)
        {
            PromotionSubmissionVM model = new PromotionSubmissionVM();
            try
            {
                PromotionApplicationVM vm = new PromotionApplicationVM();
                var staff = await _staffRepo.GetSingle(x => x.Id == staffId, x => x.SupportingDocuments, x => x.Publications, x => x.Jobs);
                if (staff != null)
                {
                    model.StaffId = staff.Id;
                    var user = await _userManager.FindByIdAsync(staff.UserId);
                    if (user != null)
                        vm.StaffName = user.ToString();
                    if (staff.Jobs != null)
                    {
                        int rankId = staff.Jobs.Where(x => x.IsCurrent).Single().RankId;
                        rankId++;
                        var rank = await _rankRepo.GetSingle(rankId);
                        if (rank != null)
                        {
                            vm.Rank = new RankVM
                            {
                                Id = rank.Id,
                                Description = rank.Description,
                                NQFLevel = rank.NQFLevel,
                                Name = rank.Name
                            };
                        }
                    }
                    if (staff.SupportingDocuments != null)
                    {
                        vm.SupportingDocuments = new SupportingDocumentsVM
                        {
                            IdentityDocumentId = staff.SupportingDocuments.IdentityDocumentId,
                            CVId = staff.SupportingDocuments.CVId,
                            CommunityServiceFormId = staff.SupportingDocuments.CommunityServiceFormId,
                            PeerEvalFormId = staff.SupportingDocuments.PeerEvalFormId,
                            QualificationsDocumentId = staff.SupportingDocuments.QualificationsDocumentId,
                            ScholarshipInTeachingFormId = staff.SupportingDocuments.ScholarshipInTeachingFormId,
                            StudentEvalFormId = staff.SupportingDocuments.StudentEvalFormId,
                            SelfScoredEvalutionFormId = staff.SupportingDocuments.SelfScoredEvaluationFormId
                        };
                    }
                    if (staff.Publications != null)
                    {
                        foreach (Publication pub in staff.Publications)
                        {
                            vm.Publications.Add(new PublicationVM
                            {
                                Authors = pub.Authors,
                                Name = pub.Name,
                                PublicationType = pub.PublicationType,
                                StaffId = pub.StaffId,
                                YearObtained = pub.YearObtained,
                                Id = pub.Id,
                            });
                        }
                    }
                    ViewData["Model"] = vm;
                    return View(model);
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Apply(PromotionSubmissionVM model)
        {
            try
            {
                var staff = await _staffRepo.GetSingle(x => x.Id == model.StaffId, x => x.Jobs);
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                int rankId = staff.Jobs.Where(x => x.IsCurrent).Single().RankId;
                rankId++;
                Promotion prom = new Promotion
                {
                    StaffId = model.StaffId,

                    MotivationLetter = new Document
                    {
                        FileName = model.MotivationLetter.FileName,
                        Content = await model.MotivationLetter.GetBytes()
                    },
                    Status = PromotionStatusEnum.HOD,
                    Evaluations = new List<PromotionDecision>(),
                    AddedDate = DateTime.Now,
                    RankId = rankId
                };
                await _promotionRepo.Add(prom);
                TempData["Toast"] = "Promotion details has succesfully been added";
                var link = Url.Action("Detail", "Promotions", new { promotionId = prom.Id }, Request.Scheme);
                var stffUser = await _userManager.FindByIdAsync(staff.UserId);
                var allUsers = _userRepo.Get();
                string email = allUsers.Where(x => x.DepartmentId == stffUser.DepartmentId && x.Role == UserRoleEnum.HOD).FirstOrDefault()?.Email;
                if (email != null)
                    await _emailSender.SendInboxNotification(email, link);
                return RedirectToAction("Index", "Promotions", new { userId = user.Id });
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Profile", "Staff", new { staffId = model.StaffId });
        }
    }
}
