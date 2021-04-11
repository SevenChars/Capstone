using System;
using System.Collections.Generic;

namespace ContractMaster.models
{
    public partial class Buyer
    {
        public Buyer()
        {
            ContractDetail = new HashSet<ContractDetail>();
        }

        public string BuyerId { get; set; }
        public string BuyerName { get; set; }
        public string ClientId { get; set; }
        public string BuyerEmail { get; set; }

        public Client Client { get; set; }
        public ICollection<ContractDetail> ContractDetail { get; set; }
    }
}
