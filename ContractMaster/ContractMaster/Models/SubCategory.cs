using System;
using System.Collections.Generic;

namespace ContractMaster.models
{
    public partial class SubCategory
    {
        public SubCategory()
        {
            ContractDetail = new HashSet<ContractDetail>();
        }

        public string SubcategoryId { get; set; }
        public string CategoryId { get; set; }
        public string SubcategoryName { get; set; }

        public MainCategory Category { get; set; }
        public ICollection<ContractDetail> ContractDetail { get; set; }
    }
}
