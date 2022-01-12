using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Models
{
    public class StudentsMetadata
    {
        public int StudentId { get; set; }
        [StringLength(50)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [StringLength(50)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Date of enrollment")]
        public Nullable<System.DateTime> EnrollmentDate { get; set; }
        [StringLength(50)]
        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }
        [Display(Name = "Date of birth")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
    }
}