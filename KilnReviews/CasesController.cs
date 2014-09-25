using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace KilnReviews
{
	public class CasesController : ApiController
	{
	    private readonly CasesFetcher casesFetcher = new CasesFetcher();
		private readonly ReviewsFetcher reviewsFetcher = new ReviewsFetcher();

	    // GET api/Cases/Ready
		public Case[] GetCasesReady()
		{
			var myCasesAwaitingReview = casesFetcher.GetMyCasesAwaitingReview();
			var myReviewsStillOpen = reviewsFetcher.GetMyMatchingReviews(r => r.sStatus != "approved");

			var casesLinkedToOpenReviews = myReviewsStillOpen.SelectMany(review => review.RelatedCases);
			var casesNotLinkedToOpenReviews = myCasesAwaitingReview.Where(c => !casesLinkedToOpenReviews.Contains(c.ixBug));

			return casesNotLinkedToOpenReviews.ToArray();
		}
	}
}