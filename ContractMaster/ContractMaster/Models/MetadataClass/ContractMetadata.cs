using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContractMaster.Models
{
    [ModelMetadataType(typeof(ContractMetadata))]
    public partial class Contract
    {
    }

    public class ContractMetadata
    {
        [Display(Name = "Contract Id")]
        public string ContractId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString ="{0: dd MM yyyy}")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0: dd MM yyyy}")]
        public DateTime EndDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Add Date")]
        [DisplayFormat(DataFormatString = "{0: dd MM yyyy}")]
        public DateTime AddDate { get; set; }
    }
}
