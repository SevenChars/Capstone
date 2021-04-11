using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContractMaster.models
{
    public partial class ProjectData
    {
        public string ContractId { get; set; }
        public string TenderTitle { get; set; }
        public string Description { get; set; }
        public string PromisedDays { get; set; }
        public string ActualDays { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ActualStartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ActualCompeletionDate { get; set; }
        public decimal? ContractAwardAmount { get; set; }
        public decimal? ContractCompletionAmount { get; set; }

        public Contract Contract { get; set; }
    }
}
