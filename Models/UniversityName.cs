using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Shkolla.Models
{
    public partial class UniversityName
    {
        public UniversityName()
        {
            Courses = new HashSet<Course>();
            Universities = new HashSet<University>();
        }
        [Key]
        public int UniversityNameId { get; set; }
        public int InstitutionType { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<University> Universities { get; set; }
    }
}
