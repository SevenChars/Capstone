using System;
using System.Collections.Generic;

namespace ContractMaster.models
{
    public partial class Client
    {
        public Client()
        {
            Buyer = new HashSet<Buyer>();
            ContractDetail = new HashSet<ContractDetail>();
        }

        public string ClientId { get; set; }

        public Company ClientNavigation { get; set; }
        public ICollection<Buyer> Buyer { get; set; }
        public ICollection<ContractDetail> ContractDetail { get; set; }
    }
}
