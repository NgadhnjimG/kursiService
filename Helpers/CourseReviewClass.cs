using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shkolla
{
	public class CourseReviewDataObject
	{
		public int CourseId { get; set; }
		public double StarReview { get; set; } = 0.0;
		public List<string> Comments { get; set; } = new List<string>() { { "" } };
	}
}
