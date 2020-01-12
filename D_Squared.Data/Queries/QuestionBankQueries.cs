using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Queries
{
    public class QuestionBankQueries
    {
        private readonly D_SquaredDbContext db;

        public QuestionBankQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public bool CheckForExistingCategory(string category)
        {
            return db.QuestionCategories.ToList().Exists(c => c.Category.ToUpper() == category.ToUpper());
        }
        public List<QuestionCategory> GetQuestionCategories()
        {
            return db.QuestionCategories.ToList();
        }

        public QuestionCategory GetQuestionCategory(string category)
        {
            return db.QuestionCategories.Where(c => c.Category.ToUpper() == category.ToUpper()).FirstOrDefault();
        }

        public QuestionCategory GetQuestionCategory(int categoryId)
        {
            return db.QuestionCategories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public void InsertQuestionCategory(QuestionCategory qc)
        {
            db.QuestionCategories.Add(qc);
            db.SaveChanges();
        }

        public void UpdateQuestionCategory(QuestionCategory qc)
        {
            db.Entry(qc).State = EntityState.Modified;
            db.SaveChanges();
        }

        public List<QuestionBank> GetQuestions(int categoryId)
        {
            return db.QuestionBank.Where(q => q.QuestionCategoryId == categoryId).ToList();
        }

        public List<QuestionBank> GetQuestions(string category)
        {
            var qc = db.QuestionCategories.Where(c => c.Category == category).FirstOrDefault();
            return db.QuestionBank.Where(q => q.QuestionCategoryId == qc.Id && q.IsActive).ToList();
        }

        public bool CheckForExistingQuestion(int categoryId, string question)
        {
            return db.QuestionBank.ToList().Exists(q => q.QuestionCategoryId == categoryId && q.Question.ToUpper() == question.ToUpper());
        }

        public QuestionBank GetQuestion(int categoryId, string question)
        {
            return db.QuestionBank.Where(q => q.QuestionCategoryId == categoryId && q.Question.ToUpper() == question.ToUpper()).FirstOrDefault();
        }

        public QuestionBank GetQuestion(int questionBankId)
        {
            return db.QuestionBank.Where(q => q.Id == questionBankId).FirstOrDefault();
        }

        public void InsertQuestion(QuestionBank qb)
        {
            db.QuestionBank.Add(qb);
            db.SaveChanges();
        }

        public void UpdateQuestion(QuestionBank qb)
        {
            db.Entry(qb).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
