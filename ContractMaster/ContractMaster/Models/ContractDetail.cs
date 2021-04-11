using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ValidationLibrary;

namespace ContractMaster.models
{
    public partial class ContractDetail
    {
        public string ContractId { get; set; }
        public string ContractName { get; set; }
        public string CategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public string ClientId { get; set; }
        public string ContractorId { get; set; }
        public string BuyerId { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DateNotInFuture]
        public DateTime? BidDate { get; set; }
        public string BidNumber { get; set; }

        public Buyer Buyer { get; set; }
        public MainCategory Category { get; set; }
        public Client Client { get; set; }
        public Contract Contract { get; set; }
        public Contractor Contractor { get; set; }
        public SubCategory SubCategory { get; set; }
        //public Company ClientNav { get; set; }
        //public Company ContractorNav { get; set; }

    }
}
