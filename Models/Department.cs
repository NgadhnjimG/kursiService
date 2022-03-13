using System;
using System.Collections.Generic;

#nullable disable

namespace Shkolla.Models
{
    public partial class Department
    {
        public Department()
        {
            Courses = new HashSet<Course>();
            Spitals = new HashSet<University>();
        }

        public int DepartmentId { get; set; }
        public string Departments { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<University> Spitals { get; set; }
    }
}
