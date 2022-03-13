using System;
using System.Collections.Generic;

#nullable disable

namespace Shkolla.Models
{
    public partial class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Number { get; set; }
        public int Departments { get; set; }

        public virtual Department Department { get; set; }
        public virtual UniversityName University { get; set; }
    }
}
