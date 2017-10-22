﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class ForecastDatum
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ObjectID { get; set; }

        public DateTime BusinessDate { get; set; }

        [StringLength(3)]
        public string StoreNumber { get; set; }

        public decimal SalesForecast { get; set; }

        public decimal ActualPriorYear { get; set; }

        public decimal AvgPrior4Weeks { get; set; }

        public decimal LaborForecast { get; set; }
    }
}
