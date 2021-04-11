using System;
using System.Collections.Generic;

namespace ContractMaster.models
{
    public partial class MainCategory
    {
        public MainCategory()
        {
            ContractDetail = new HashSet<ContractDetail>();
            SubCategory = new HashSet<SubCategory>();
        }

        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        public ICollection<ContractDetail> ContractDetail { get; set; }
        public ICollection<SubCategory> SubCategory { get; set; }
    }
}
