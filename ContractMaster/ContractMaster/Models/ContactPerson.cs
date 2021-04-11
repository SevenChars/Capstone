using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContractMaster.models
{
    public partial class ContactPerson
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CompanyId { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public Company Company { get; set; }
    }
}
