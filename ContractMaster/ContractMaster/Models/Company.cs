using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContractMaster.models
{
    public partial class Company
    {

        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string AccountStatus { get; set; }
        public string Address { get; set; }

        [RegularExpression("^[A-Za-z]$")]
        public string City { get; set; }

        [RegularExpression("^[A-Za-z]$")]
        public string Province { get; set; }

        [RegularExpression(@"^[A-Za-z]\d[A-Za-z] ?\d[A-Za-z]\d$")]
        public string PositalCode { get; set; }

        public string ContactPerson { get; set; }

        public Client Client { get; set; }
        public Contractor Contractor { get; set; }
        //public ICollection<ContactPerson> ContactPersonNavigation { get; set; }
        public ContactPerson ContactPersonNavigation { get; set; }
    }
}
