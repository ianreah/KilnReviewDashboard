using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace KilnReviews
{
	public class Changeset 
	{
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
	}
}