using System;
using System.Collections.Generic;

#nullable disable

namespace Shkolla.Models
{
    public partial class University
    {
        public int Id { get; set; }
        public int Shkolla { get; set; }
        public int UniversityName { get; set; }
        public string Address { get; set; }
        public int Departments { get; set; }
        public int EmployeeNumber { get; set; }

        public virtual Department Department { get; set; }
        public virtual UniversityName UniversityNames { get; set; }
    }
}
