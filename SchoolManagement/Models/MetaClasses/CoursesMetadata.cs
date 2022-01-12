using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    //Esta clase se crea ya que cada vez que se compile el modelo con los cambios en bd ,se borrarán estos cambios
    public class CoursesMetadata
    {
        [StringLength(50)]
        public string Title { get; set; }
        [Range(1,8)]
        public int Credits { get; set; }
    }
}