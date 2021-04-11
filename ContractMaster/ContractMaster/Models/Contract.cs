using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ValidationLibrary;

namespace ContractMaster.models
{
    public partial class Contract
    {
        public string ContractId { get; set; }
        
        [DataType(DataType.Date)]
        [DateNotInFuture]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime AddDate { get; set; }

        public ContractDetail ContractDetail { get; set; }
        public ProjectData ProjectData { get; set; }
    }
}
