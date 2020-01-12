using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class QuestionCategoryDTO
    {
        public int Id { get; set; }

        [Display(Name = "Question Category")]
        public string Category { get; set; }

        [Display(Name = "Allow Modifying Questions")]
        public bool AllowQuestionsModification { get; set; }
    }
}
