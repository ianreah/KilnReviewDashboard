using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace KilnReviews
{
	public class Changeset 
	{
        public string rev { get; set; }
        public string dt { get; set; }
		public string sAuthor { get; set; }
		public int[] ixBugs { get; set; }

	    public DateTime DateTime
		{
			get
			{
				var fudgedDateTimeText = dt.Replace("T", " ").Replace("ZZ", "");
				return DateTime.Parse(fudgedDateTimeText, CultureInfo.InvariantCulture, DateTimeStyles.None);
			}
		}

	    public void FindXamlFiles(WebClient webClient, string kilnUrlBase, HttpCookie token, int ixRepo)
	    {
	        var requestString = string.Format("{0}Api/2.0/Repo/{1}/History/{2}?token={3}", kilnUrlBase, ixRepo, rev, token.Value);
	        var rawChangesetsWithDiffs = webClient.DownloadString(requestString);
            var changesetsWithDiffs = JsonConvert.DeserializeObject<ChangesetWithDiffs>(rawChangesetsWithDiffs);

            ContainsXamlFiles = changesetsWithDiffs.diffs.Any(diff => Path.GetExtension(DecodeBytePath(diff.bpPath)) == ".xaml");
	    }

	    private static string DecodeBytePath(string encodedPath)
	    {
	        var sb = new StringBuilder();
            for (var i = 0; i < encodedPath.Length; i += 2)
            {
                var hs = encodedPath.Substring(i, 2);
                sb.Append(Convert.ToChar(Convert.ToUInt32(hs, 16)));
            }

	        return sb.ToString();
	    }

	    public bool ContainsXamlFiles { get; private set; }
	}
}