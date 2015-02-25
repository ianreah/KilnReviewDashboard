﻿using System.Collections.Generic;

namespace KilnReviews
{
	public class Review
	{
        public Reviewer[] reviewers { get; set; }
		public string sReview { get; set; }
		public string sStatus { get; set; }
		public string sTitle { get; set; }
		public string[] Reviewers { get; set; }
		public string[] Authors { get; set; }
		public int DaysOld { get; set; }
		public int[] RelatedCases { get; set; }
	}
}