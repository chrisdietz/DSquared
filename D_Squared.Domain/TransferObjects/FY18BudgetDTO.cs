using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class FY18BudgetDTO
    {
        public FY18BudgetDTO(List<FY18Budget> budgets, DateTime currentDay)
        {
            if (budgets.Count != 0)
            {
                if (currentDay.Month == 1)
                {
                    Account60205 = budgets.Where(b => b.Account == 60205).FirstOrDefault().Jan;
                    Account60206 = budgets.Where(b => b.Account == 60206).FirstOrDefault().Jan;
                    Account60210 = budgets.Where(b => b.Account == 60210).FirstOrDefault().Jan;
                    Account60211 = budgets.Where(b => b.Account == 60211).FirstOrDefault().Jan;
                    Account60225 = budgets.Where(b => b.Account == 60225).FirstOrDefault().Jan;
                }
                else if (currentDay.Month == 2)
                {
                    Account60205 = budgets.Where(b => b.Account == 60205).FirstOrDefault().Feb;
                    Account60206 = budgets.Where(b => b.Account == 60206).FirstOrDefault().Feb;
                    Account60210 = budgets.Where(b => b.Account == 60210).FirstOrDefault().Feb;
                    Account60211 = budgets.Where(b => b.Account == 60211).FirstOrDefault().Feb;
                    Account60225 = budgets.Where(b => b.Account == 60225).FirstOrDefault().Feb;
                }
                else if (currentDay.Month == 3)
                {
                    Account60205 = budgets.Where(b => b.Account == 60205).FirstOrDefault().Mar;
                    Account60206 = budgets.Where(b => b.Account == 60206).FirstOrDefault().Mar;
                    Account60210 = budgets.Where(b => b.Account == 60210).FirstOrDefault().Mar;
                    Account60211 = budgets.Where(b => b.Account == 60211).FirstOrDefault().Mar;
                    Account60225 = budgets.Where(b => b.Account == 60225).FirstOrDefault().Mar;
                }
                else if (currentDay.Month == 4)
                {
                    Account60205 = budgets.Where(b => b.Account == 60205).FirstOrDefault().Apr;
                    Account60206 = budgets.Where(b => b.Account == 60206).FirstOrDefault().Apr;
                    Account60210 = budgets.Where(b => b.Account == 60210).FirstOrDefault().Apr;
                    Account60211 = budgets.Where(b => b.Account == 60211).FirstOrDefault().Apr;
                    Account60225 = budgets.Where(b => b.Account == 60225).FirstOrDefault().Apr;
                }
                else if (currentDay.Month == 5)
                {
                    Account60205 = budgets.Where(b => b.Account == 60205).FirstOrDefault().May;
                    Account60206 = budgets.Where(b => b.Account == 60206).FirstOrDefault().May;
                    Account60210 = budgets.Where(b => b.Account == 60210).FirstOrDefault().May;
                    Account60211 = budgets.Where(b => b.Account == 60211).FirstOrDefault().May;
                    Account60225 = budgets.Where(b => b.Account == 60225).FirstOrDefault().May;
                }
                else if (currentDay.Month == 6)
                {
                    Account60205 = budgets.Where(b => b.Account == 60205).FirstOrDefault().Jun;
                    Account60206 = budgets.Where(b => b.Account == 60206).FirstOrDefault().Jun;
                    Account60210 = budgets.Where(b => b.Account == 60210).FirstOrDefault().Jun;
                    Account60211 = budgets.Where(b => b.Account == 60211).FirstOrDefault().Jun;
                    Account60225 = budgets.Where(b => b.Account == 60225).FirstOrDefault().Jun;
                }
                else if (currentDay.Month == 7)
                {
                    Account60205 = budgets.Where(b => b.Account == 60205).FirstOrDefault().Jul;
                    Account60206 = budgets.Where(b => b.Account == 60206).FirstOrDefault().Jul;
                    Account60210 = budgets.Where(b => b.Account == 60210).FirstOrDefault().Jul;
                    Account60211 = budgets.Where(b => b.Account == 60211).FirstOrDefault().Jul;
                    Account60225 = budgets.Where(b => b.Account == 60225).FirstOrDefault().Jul;
                }
                else if (currentDay.Month == 8)
                {
                    Account60205 = budgets.Where(b => b.Account == 60205).FirstOrDefault().Aug;
                    Account60206 = budgets.Where(b => b.Account == 60206).FirstOrDefault().Aug;
                    Account60210 = budgets.Where(b => b.Account == 60210).FirstOrDefault().Aug;
                    Account60211 = budgets.Where(b => b.Account == 60211).FirstOrDefault().Aug;
                    Account60225 = budgets.Where(b => b.Account == 60225).FirstOrDefault().Aug;
                }
                else if (currentDay.Month == 9)
                {
                    Account60205 = budgets.Where(b => b.Account == 60205).FirstOrDefault().Sep;
                    Account60206 = budgets.Where(b => b.Account == 60206).FirstOrDefault().Sep;
                    Account60210 = budgets.Where(b => b.Account == 60210).FirstOrDefault().Sep;
                    Account60211 = budgets.Where(b => b.Account == 60211).FirstOrDefault().Sep;
                    Account60225 = budgets.Where(b => b.Account == 60225).FirstOrDefault().Sep;
                }
                else if (currentDay.Month == 10)
                {
                    Account60205 = budgets.Where(b => b.Account == 60205).FirstOrDefault().Oct;
                    Account60206 = budgets.Where(b => b.Account == 60206).FirstOrDefault().Oct;
                    Account60210 = budgets.Where(b => b.Account == 60210).FirstOrDefault().Oct;
                    Account60211 = budgets.Where(b => b.Account == 60211).FirstOrDefault().Oct;
                    Account60225 = budgets.Where(b => b.Account == 60225).FirstOrDefault().Oct;
                }
                else if (currentDay.Month == 11)
                {
                    Account60205 = budgets.Where(b => b.Account == 60205).FirstOrDefault().Nov;
                    Account60206 = budgets.Where(b => b.Account == 60206).FirstOrDefault().Nov;
                    Account60210 = budgets.Where(b => b.Account == 60210).FirstOrDefault().Nov;
                    Account60211 = budgets.Where(b => b.Account == 60211).FirstOrDefault().Nov;
                    Account60225 = budgets.Where(b => b.Account == 60225).FirstOrDefault().Nov;
                }
                else if (currentDay.Month == 12)
                {
                    Account60205 = budgets.Where(b => b.Account == 60205).FirstOrDefault().Dec;
                    Account60206 = budgets.Where(b => b.Account == 60206).FirstOrDefault().Dec;
                    Account60210 = budgets.Where(b => b.Account == 60210).FirstOrDefault().Dec;
                    Account60211 = budgets.Where(b => b.Account == 60211).FirstOrDefault().Dec;
                    Account60225 = budgets.Where(b => b.Account == 60225).FirstOrDefault().Dec;
                }
            }
        }

        public decimal Account60205 { get; set; }

        public decimal Account60206 { get; set; }

        public decimal Account60210 { get; set; }

        public decimal Account60211 { get; set; }

        public decimal Account60225 { get; set; }
    }
}
