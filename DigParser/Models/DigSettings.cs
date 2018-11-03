using System.Collections.ObjectModel;

namespace DIGStatus
{
    class DigSettings
    {
		/// <summary>
		/// Site url
		/// </summary>
		public string BaseURL { get; set; } = "http://dailyindiegame.com/content_marketplace_0.html";

		/// <summary>
		/// Prefix to set id for parsing
		/// </summary>
		public string Prefix { get; set; } = "{CurrentId}";

		/// <summary>
		/// Html page id to start parsing
		/// </summary>
		public int StartPoint { get; set; } = 0;

		/// <summary>
		/// Html page id to end parsing
		/// </summary>
		public int EndPoint { get; set; } = 9;
	}
}
