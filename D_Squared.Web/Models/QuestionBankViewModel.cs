using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Models
{
    public class QuestionBankViewModel
    {
        public QuestionBankViewModel()
        {
            LoadYesNoDropdownList();
        }
        public QuestionCategory QuestionCategory { get; set; }
        public List<QuestionBank> Questions { get; set; }
        public QuestionBank Question { get; set; }
        public List<SelectListItem> YesNoDropdownList { get; set; }
        public bool DefaultYesNoDropdownValue = false;
        public void LoadYesNoDropdownList()
        {
            YesNoDropdownList = new List<SelectListItem>();
            YesNoDropdownList.Add(new SelectListItem { Value = "", Text = "" });
            YesNoDropdownList.Add(new SelectListItem { Value = "true", Text = "Yes" });
            YesNoDropdownList.Add(new SelectListItem { Value = "false", Text = "No" });
        }
    }
}