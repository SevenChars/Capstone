using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContractMaster.Models
{
    [ModelMetadataType(typeof(ContractDetailMetadata))]
    public partial class ContractDetail
    {
    }

    public class ContractDetailMetadata
    {
        [Display(Name = "Contract Id")]
        public string ContractId { get; set; }

        [Display(Name = "Company Name")]
        public string ContractName { get; set; }

        [Display(Name = "Category Id")]
        public string CategoryId { get; set; }

        [Display(Name = "Subcategory Id")]
        public string SubCategoryId { get; set; }

        [Display(Name = "Client Id")]
        public string ClientId { get; set; }

        public string ContractorId { get; set; }

        public string BuyerId { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd MM yyyy}")]
        public DateTime? BidDate { get; set; }

        public string BidNumber { get; set; }
    }
}
