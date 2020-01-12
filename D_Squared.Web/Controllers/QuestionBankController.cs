using D_Squared.Data.Context;
using D_Squared.Data.Queries;
using D_Squared.Domain.Entities;
using D_Squared.Web.Helpers;
using D_Squared.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ROLES = D_Squared.Domain.DomainConstants.RoleNames;

namespace D_Squared.Web.Controllers
{
    [AuthorizeGroup(ROLES.DSquaredAdminGroup)]
    public class QuestionBankController : BaseController
    {
        private readonly D_SquaredDbContext db;
        private readonly QuestionBankQueries qq;

        public QuestionBankController()
        {
            db = new D_SquaredDbContext();
            qq = new QuestionBankQueries(db);
        }
        // GET: QuestionBank
        public ActionResult CategoryList()
        {
            return View("CategoryList", db.QuestionCategories.ToList());
        }

        public ActionResult CreateCategory()
        {
            return View(new QuestionCategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(QuestionCategoryViewModel model)
        {
            string username = User.TruncatedName;

            if (qq.CheckForExistingCategory(model.QuestionCategory.Category))
            {
                QuestionCategory existingCategory = qq.GetQuestionCategory(model.QuestionCategory.Category);

                Warning("This Category already exists -- you have been redirected to the edit page.");
                return RedirectToAction("Edit", new { id = existingCategory.Id });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    model.QuestionCategory.CreatedBy = username;
                    model.QuestionCategory.CreatedDate = DateTime.Now;
                    model.QuestionCategory.UpdatedBy = username;
                    model.QuestionCategory.UpdatedDate = DateTime.Now;
                    qq.InsertQuestionCategory(model.QuestionCategory);

                    Success("Success:  Your information was saved!");
                    return RedirectToAction("CategoryList");
                }


                Error("Error: Unable to save the information, please check the values entered!");
                return View(model);
            }
        }

        public ActionResult EditCategory(int id) => View(new QuestionCategoryViewModel { QuestionCategory = qq.GetQuestionCategory(id) });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(QuestionCategoryViewModel model)
        {
            string username = User.TruncatedName;

            if (ModelState.IsValid)
            {
                model.QuestionCategory.UpdatedBy = username;
                model.QuestionCategory.UpdatedDate = DateTime.Now;
                qq.UpdateQuestionCategory(model.QuestionCategory);

                Success("Success:  Your information was updated!");
                return RedirectToAction("CategoryList");
            }


            Error("Error: Unable to update the information, please check the values entered!");
            return View(model);
        }

        // GET: QuestionBank
        public ActionResult QuestionList(int categoryId)
        {
            var qc = qq.GetQuestionCategory(categoryId);
            QuestionBankViewModel model = new QuestionBankViewModel
            {
                Questions = qq.GetQuestions(categoryId),
                QuestionCategory = qc
            };
            return View("QuestionList", model);
        }

        public ActionResult CreateQuestion(int categoryId)
        {
            var qModel = new QuestionBankViewModel { Question = new QuestionBank { QuestionCategoryId = categoryId } };
            return View(qModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateQuestion(QuestionBankViewModel model)
        {
            string username = User.TruncatedName;

            if (qq.CheckForExistingQuestion(model.Question.QuestionCategoryId, model.Question.Question))
            {
                QuestionBank existingQuestion = qq.GetQuestion(model.Question.QuestionCategoryId, model.Question.Question);

                Warning("This Question already exists -- you have been redirected to the edit page.");
                return RedirectToAction("Edit", new { id = existingQuestion.Id });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    model.Question.CreatedBy = username;
                    model.Question.CreatedDate = DateTime.Now;
                    model.Question.UpdatedBy = username;
                    model.Question.UpdatedDate = DateTime.Now;
                    qq.InsertQuestion(model.Question);

                    Success("Success:  Your information was saved!");
                    return RedirectToAction("QuestionList", new { categoryId = model.Question.QuestionCategoryId });
                }


                Error("Error: Unable to save the information, please check the values entered!");
                return View(model);
            }
        }

        public ActionResult EditQuestion(int id)
        {
            var question = qq.GetQuestion(id);
            var category = qq.GetQuestionCategory(question.QuestionCategoryId);
            return View(new QuestionBankViewModel { Question = question, QuestionCategory = category });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestion(QuestionBankViewModel model)
        {
            string username = User.TruncatedName;

            if (ModelState.IsValid)
            {
                model.Question.UpdatedBy = username;
                model.Question.UpdatedDate = DateTime.Now;
                qq.UpdateQuestion(model.Question);

                Success("Success:  Your information was updated!");
                return RedirectToAction("QuestionList", new { categoryId = model.Question.QuestionCategoryId });
            }


            Error("Error: Unable to update the information, please check the values entered!");
            return View(model);
        }
    }
}