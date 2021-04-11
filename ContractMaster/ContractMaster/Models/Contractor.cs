using System;
using System.Collections.Generic;

namespace ContractMaster.models
{
    public partial class Contractor
    {
        public Contractor()
        {
            ContractDetail = new HashSet<ContractDetail>();
        }

        public string ContractorId { get; set; }
        public string ProjectPrincipal { get; set; }
        public string ProjectCoordinator { get; set; }

        public Company ContractorNavigation { get; set; }
        public ICollection<ContractDetail> ContractDetail { get; set; }
    }
}
