using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContractMaster.Models
{
    [ModelMetadataType(typeof(ContactPersonMetadata))]
    public partial class ContactPerson
    {
    }

    public class ContactPersonMetadata
    {
        [Display(Name = "Company Name")]
        public string Name { get; set; }
        [Display(Name = "Company Id")]
        public string CompanyId { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
        public string Phone { get; set; }
    }
}
