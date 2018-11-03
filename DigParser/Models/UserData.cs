namespace DIGStatus
{
    class UserData
    {
		/// <summary>
		/// Current User ID
		/// </summary>
		public int UserID { get; set; }

		/// <summary>
		/// Current User Name
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Current User Avatar
		/// </summary>
		public string UserAvatar { get; set; }

		/// <summary>
		/// Current User Country Code
		/// </summary>
		public string UserCountryCode { get; set; }

		/// <summary>
		/// Amount of games on user account
		/// </summary>
		public int UserGameCount { get; set; }

		/// <summary>
		/// Current User Status
		/// </summary>
		public string UserProfileStatus { get; set; }

		/// <summary>
		/// Current User State
		/// </summary>
		public string UserProfileState { get; set; }
	}
}
