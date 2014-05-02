﻿using System;
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
	public class ReviewsController : ApiController
	{
	    private static readonly int ageOfReviewInDeletedRepo = TimeSpan.MaxValue.Days;
	    private static bool NotInDeletedRepo(Review review)
	    {
	        return review.DaysOld != ageOfReviewInDeletedRepo;
	    }

		// GET api/Reviews/Todo
		public Review[] GetReviewsTodo()
		{
			var response = GetReviews();
			var reviews = JsonConvert.DeserializeObject<Reviews>(response);

			var reviewsToReturn = reviews.reviewsAwaitingReview.Where(r => r.sStatus != "needswork").ToArray();
			AddDatesForReviews(reviewsToReturn);

            return reviewsToReturn.Where(NotInDeletedRepo).ToArray();
        }

		// GET api/Reviews/Rejected
		public Review[] GetRejectedReviews()
		{
			var response = GetReviews();
			var reviews = JsonConvert.DeserializeObject<Reviews>(response);

			var reviewsToReturn = reviews.reviewsAuthor.Where(r => r.sStatus == "rejected" || r.sStatus == "needswork").ToArray();

			AddDatesForReviews(reviewsToReturn);

            return reviewsToReturn.Where(NotInDeletedRepo).ToArray();
        }

		// GET api/Reviews/Waiting
		public Review[] GetReviewsWaiting()
		{
			var response = GetReviews();
			var reviews = JsonConvert.DeserializeObject<Reviews>(response);

			var reviewsToReturn = reviews.reviewsAuthor.Where(r => r.sStatus == "active").ToArray();
			AddDatesForReviews(reviewsToReturn);

            return reviewsToReturn.Where(NotInDeletedRepo).ToArray();
        }

		// TODO: caching - http://www.strathweb.com/2012/05/output-caching-in-asp-net-web-api/
		private static string GetReviews()
		{
			using (var webClient = new WebClient())
			{
				// TODO: Error handling - failure to get kilnToken or kilnUrlBase
				var token = HttpContext.Current.Request.Cookies["kilnToken"];
				var kilnUrlBase = ConfigurationManager.AppSettings["kilnUrlBase"];

				if (token == null || String.IsNullOrEmpty(kilnUrlBase))
				{
					return string.Empty;
				}

			    return webClient.DownloadString(string.Format("{0}Api/2.0/Reviews?token={1}", kilnUrlBase, token.Value));
			}
		}

		private void AddDatesForReviews(IEnumerable<Review> reviews)
		{
			var token = HttpContext.Current.Request.Cookies["kilnToken"];
			var kilnUrlBase = ConfigurationManager.AppSettings["kilnUrlBase"];

			if (token == null || String.IsNullOrEmpty(kilnUrlBase))
			{
				return;
			}

            foreach (var review in reviews)
			{
				using (var webClient = new WebClient())
				{
					var response = webClient.DownloadString(string.Format("{0}Api/2.0/Review/{1}?token={2}", kilnUrlBase, review.sReview, token.Value));
					var reviewWithChangesets = JsonConvert.DeserializeObject<ReviewWithChangesets>(response);

                    if (reviewWithChangesets.changesets == null)    // indicative of a review of changesets from a repo that's since been deleted
				    {
                        review.DaysOld = ageOfReviewInDeletedRepo;
				    }
				    else
				    {
                        var changesetAge = DateTime.Now - reviewWithChangesets.changesets.Max(c => c.DateTime);
                        review.DaysOld = (int)changesetAge.TotalDays;

                        review.Reviewers = review.reviewers.Select(x => GravitarUrl(x.sEmail)).ToArray();

                        review.Authors = reviewWithChangesets.changesets.Select(x => GravitarUrl(GetEmail(x.sAuthor))).Distinct().ToArray();
                    }
				}	
			}
		}

		private string GetEmail(string sAuthor)
		{
			var r = new Regex("<([^>]*)>");

			if (r.IsMatch(sAuthor))
			{
				return r.Match(sAuthor).Groups[1].Value;
			}
			else
			{
				return "unknown";
			}
		}

		private static string GravitarUrl(string email)
		{
			var hash = CalculateMD5Hash(email.ToLowerInvariant());
			return string.Format("https://secure.gravatar.com/avatar/{0}?s=32", hash);
		}

		private static string CalculateMD5Hash(string input)
		{
			var md5 = MD5.Create();
			var inputBytes = Encoding.ASCII.GetBytes(input);
			var hash = md5.ComputeHash(inputBytes);

			var sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString().ToLower();
		}
	}
}