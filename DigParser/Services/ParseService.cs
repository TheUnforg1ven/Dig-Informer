using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace DIGStatus
{
    class ParseService
    {
		// serialization member
		private SerializationService _serializationService;

		// amount of account games
		public int GameCountParse { get; set; }

		// steam user name
		public string UserNameParse { get; set; }

		// user picture
		public string UserAvatarParse { get; set; }

		// user country code 
		public string UserCountryCodeParse { get; set; }

		// user status
		public string UserProfileStatusParse { get; set; }

		// user page
		public string UserProfileStateParse { get; set; }

		// steam user ID
		public long UserSteamID { get; set; }

		// collection of all user steam games
		public ObservableCollection<string> UserGames { get; set; } = new ObservableCollection<string>();

		// simple constructor
		public ParseService() => _serializationService = new SerializationService();

		/// <summary>
		/// Get collection of user games from parsed json
		/// </summary>
		/// <returns> true if operation is successful </returns>
		public bool GetUserGamesInfo()
		{
			UserGames.Clear();

			try
			{
				var urlAddressGame = $"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=<your_steam_key>&steamid={UserSteamID}&include_appinfo=1&format=json";
				var jObjectGames = GetAsync(urlAddressGame).Result;

				foreach (var dataItem in jObjectGames["response"]["games"].Children())
					UserGames.Add((string)dataItem["name"]);
			}
			catch (ArgumentException) { UserNameParse = "No such user"; }
			catch (Exception)
			{
				MessageBox.Show("Your link is not available", "Link error");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Get all user info from json
		/// </summary>
		/// <param name="enteredUserName"> link on user steam profile </param>
		/// <returns> true if operation is successful </returns>
		public bool GetUserAccountInfo(string enteredUserName)
		{
			UserGames.Clear();

			try
			{
				var urlAddressUserVanilla = $"http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key=<your_steam_key>&vanityurl={enteredUserName}";
				var jObjectUservanilla = GetAsync(urlAddressUserVanilla).Result;

				long userSteamID = 0;

				if (Regex.IsMatch(enteredUserName, @"^\d+$"))
				{
					userSteamID = Convert.ToInt64(enteredUserName);
				}

				else if ((int)jObjectUservanilla["response"]["success"] == 1)
				{
					userSteamID = (long)jObjectUservanilla["response"]["steamid"];
				}
	
				else
				{
					UserNameParse = "No such user";
					MessageBox.Show("No such user", "User error");
					return false;
				}

				UserSteamID = userSteamID;

				var urlAddressUser = $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=<your_steam_key>&steamids={userSteamID}";
				var urlAddressGame = $"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=<your_steam_key>&steamid={userSteamID}&include_appinfo=1&format=json";

				var jObjectUser = GetAsync(urlAddressUser).Result;
				var jObjectGame = GetAsync(urlAddressGame).Result;

				UserNameParse = (string)jObjectUser["response"]["players"][0]["personaname"];

				GameCountParse = (int)jObjectGame["response"]["game_count"];

				UserCountryCodeParse = (string)jObjectUser["response"]["players"][0]["loccountrycode"];

				UserAvatarParse = (string)jObjectUser["response"]["players"][0]["avatarfull"];

				var userState = (int)jObjectUser["response"]["players"][0]["personastate"];

				switch (userState)
				{
					case 0:
						UserProfileStateParse = "Offline"; break;
					case 1:
						UserProfileStateParse = "Online"; break;
					case 2:
						UserProfileStateParse = "Busy"; break;
					case 3:
						UserProfileStateParse = "Away"; break;
					case 4:
						UserProfileStateParse = "Snooze"; break;
					case 5:
						UserProfileStateParse = "Looking to trade"; break;
					case 6:
						UserProfileStateParse = "Looking to play"; break;
					default:
						throw new ArgumentException();
				}

				// get user status
				var userStatus = (int)jObjectUser["response"]["players"][0]["communityvisibilitystate"];
				UserProfileStatusParse = userStatus == 3 ? "Public" : "Private";

				if (string.IsNullOrEmpty(UserNameParse))
				{
					UserNameParse = "No such user";
					MessageBox.Show("Streamer offline", "Offline error");
					return false;
				}
			}
			catch (ArgumentException) { UserNameParse = "No such user"; }
			catch (Exception)
			{
				MessageBox.Show("Your link is not available", "Link error");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Method to parse given url
		/// </summary>
		/// <param name="uri"> url to parse </param>
		/// <returns> json object with parsed content </returns>
		public async Task<JObject> GetAsync(string uri)
		{
			var httpClient = new HttpClient();
			var content = await httpClient.GetStringAsync(uri);
			var jObject = JObject.Parse(content);
			return jObject;
		}
	}
}
