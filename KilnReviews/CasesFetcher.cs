using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace KilnReviews
{
	public class CasesFetcher
	{
		private const string query = "status:\"{0}\" assignedto:\"me\"";
		private const string cols = "ixBug,sTitle";

		public Case[] GetMyCasesAwaitingReview()
		{
			using (var webClient = new WebClient())
			{
				// TODO: Error handling - failure to get kilnToken or kilnUrlBase
				var token = HttpContext.Current.Request.Cookies["kilnToken"];
				var fogBugzUrlBase = ConfigurationManager.AppSettings["fogBugzUrlBase"];

				if (token == null || String.IsNullOrEmpty(fogBugzUrlBase))
				{
					return new Case[0];
				}

				var xmlCases = webClient.DownloadString(string.Format(
					"{0}api.asp?token={1}&cmd=search&q={2}&cols={3}",
					fogBugzUrlBase,
					token.Value,
					string.Format(query, ConfigurationManager.AppSettings["fogBugzAwaitingReviewStatus"]),
					cols));

				return XDocument.Parse(xmlCases)
					.Descendants("case")
					.Select(node => new Case
					{
						ixBug = int.Parse(CaseElement(node, "ixBug")),
						sTitle = CaseElement(node, "sTitle")
					})
					.ToArray();
			}
		}

		private static string CaseElement(XContainer caseNode, string elementName)
		{
			return caseNode.Descendants(elementName)
				.First()
				.Value;
		}
	}
}