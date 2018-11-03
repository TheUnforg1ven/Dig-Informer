using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DIGStatus
{
	[Serializable]
	class UserInfoViewModel : BaseViewModel
    {
		#region Private Members

		// current page
		private Page _page;

		// user data object
		private UserData _userData;

		// uses to parse info
		private ParseService _parseService;

		// uses to save info into file
		private FileSaveService _fileSaveService;

		#endregion

		#region Public Properties

		/// <summary>
		/// Entered bu user username
		/// </summary>
		public string EnteredUserName { get; set; }

		/// <summary>
		/// Username prop
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Amount of games on user steam account
		/// </summary>
		public int UserGameCount { get; set; }

		/// <summary>
		/// User steam Avatar
		/// </summary>
		public string UserAvatar { get; set; }

		/// <summary>
		/// Country code
		/// </summary>
		public string UserCountryCode { get; set; }

		/// <summary>
		/// User steam profile status
		/// </summary>
		public string UserProfileStatus { get; set; }

		/// <summary>
		/// User steam profile state
		/// </summary>
		public string UserProfileState { get; set; }

		/// <summary>
		/// All games user can buy on dig
		/// </summary>
		public int AmountGamesAll { get; set; }

		/// <summary>
		/// All games with cards user can buy on dig
		/// </summary>
		public int AmountGamesWithCards { get; set; }

		/// <summary>
		/// Summ of all available games
		/// </summary>
		public double GamesAllSumm { get; set; }

		/// <summary>
		/// Summ of all available games with cards
		/// </summary>
		public double GamesWithCardsSumm { get; set; }

		/// <summary>
		/// Disable/enable "Available games" button
		/// </summary>
		public bool CanCloseAvailableAll { get; set; }

		/// <summary>
		/// Disable/enable "Available games with cards" button
		/// </summary>
		public bool CanCloseAvailableAllCards { get; set; }

		/// <summary>
		/// Disable/enable "Save all info" button
		/// </summary>
		public bool CanCloseSaveAll { get; set; }

		/// <summary>
		/// Disable/enable "Save cards info" button
		/// </summary>
		public bool CanCloseSaveCards { get; set; }

		/// <summary>
		/// User games collection
		/// </summary>
		public ObservableCollection<string> UserGames { get; set; }

		/// <summary>
		/// Listview to show in view
		/// </summary>
		public ObservableCollection<string> ListViewSourse { get; set; }

		/// <summary>
		/// All games prices
		/// </summary>
		public ObservableCollection<double> PricesAll { get; private set; }

		/// <summary>
		/// All games with cards prices
		/// </summary>
		public ObservableCollection<double> PricesOnlyCards { get; private set; }

		/// <summary>
		/// Dictionary with all available for user games with prices
		/// </summary>
		public Dictionary<string, double> UserAllGamesDict { get; set; }

		/// <summary>
		/// Dictionary with all available for user games with cards with prices
		/// </summary>
		public Dictionary<string, double> UserCardGamesDict { get; set; }

		/// <summary>
		/// List to save temporary info
		/// </summary>
		public List<string> TempList { get; private set; }

		#endregion

		#region Public Commands

		/// <summary>
		/// Command to find entered user
		/// </summary>
		public ICommand FindUser { get; set; }

		/// <summary>
		/// Command to find all available games
		/// </summary>
		public ICommand FindAvailableGames { get; set; }

		/// <summary>
		/// Command to find all available games with cards
		/// </summary>
		public ICommand FindAvailableGamesWithCards { get; set; }

		/// <summary>
		/// Command to save info about all games into file
		/// </summary>
		public ICommand SaveAllInfo { get; set; }

		/// <summary>
		/// Command to save info about all games with cards into file
		/// </summary>
		public ICommand SaveInfoAboutCards { get; set; }

		#endregion

		/// <summary>
		/// Simple constructor
		/// </summary>
		/// <param name="page"> Current page </param>
		public UserInfoViewModel(Page page)
		{
			_page = page;
			_fileSaveService = new FileSaveService(_page);
			

			UserGames = new ObservableCollection<string>();
			ListViewSourse = new ObservableCollection<string>();
			TempList = new List<string>();

			CanCloseAvailableAll = false;
			CanCloseAvailableAllCards = false;
			CanCloseSaveAll = false;
			CanCloseSaveCards = false;

			PricesAll = new ObservableCollection<double>();
			PricesOnlyCards = new ObservableCollection<double>();

			UserAllGamesDict = new Dictionary<string, double>();
			UserCardGamesDict = new Dictionary<string, double>();

			// initialize model data
			_userData = new UserData();
			_parseService = new ParseService();

			FindUser = new RelayCommand(() => Task.Factory.StartNew(() => GetUserInfo(EnteredUserName)));

			FindAvailableGames = new RelayCommand(() => Task.Factory.StartNew(() => GetAvailableGamesInfo()));

			FindAvailableGamesWithCards = new RelayCommand(() => Task.Factory.StartNew(() => GetAvailableGamesCardsInfo()));

			SaveAllInfo = new RelayCommand(() => UserSaveAllFile(UserAllGamesDict));

			SaveInfoAboutCards = new RelayCommand(() => UserSaveAllFile(UserCardGamesDict));
		}

		/// <summary>
		/// Save info into file
		/// </summary>
		/// <param name="distToSave"> Dictionary to save </param>
		public void UserSaveAllFile(Dictionary<string, double> distToSave)
		{
			_fileSaveService.Save(distToSave);
		}

		/// <summary>
		/// Get information abount all available games with cards
		/// </summary>
		public void GetAvailableGamesCardsInfo()
		{
			var isContinueGames = _parseService.GetUserGamesInfo();

			_page.Dispatcher.Invoke(() =>
			{
				if (UserName is null)
				{
					MessageBox.Show("No entered user", "No user error");
					return;
				}

				if (ViewModelHelper.DigGames.Count != 0)
				{
					var tempList = ViewModelHelper.DigGamesWithCards
						.Where(item => !UserGames.Contains(item))
						.ToList();

					TempList = tempList;

					var twmpList = new int[ViewModelHelper.DigGamesWithCards.Count];

					for (int i = 0; i < ViewModelHelper.DigGamesWithCards.Count; i++)
					{
						if (!UserGames.Any(x => x == ViewModelHelper.DigGamesWithCards[i]))
							twmpList[i] = i;
						else
							twmpList[i] = 0;
					}

					PricesOnlyCards.Clear();
					PricesOnlyCards.Add(0.1);
					for (int i = 0; i < twmpList.Length; i++)
						if (twmpList[i] != 0)
							PricesOnlyCards.Add(ViewModelHelper.PricesWithCards[i]);

					GamesWithCardsSumm = Math.Round(PricesOnlyCards.Sum(), 3);

					AmountGamesWithCards = TempList.Distinct().Count();

					/// write user cards info into helper class

					ViewModelHelper.AllGamesCardsUserHelper = AmountGamesWithCards;
					ViewModelHelper.AllGamesPriceCardsUserHelper = GamesWithCardsSumm;

					for (var i = 0; i < TempList.Count; i++)
						if (!UserCardGamesDict.ContainsKey(TempList[i]))
							UserCardGamesDict.Add(TempList[i], PricesOnlyCards[i]);

					TempList.Clear();
					ListViewSourse.Clear();

					foreach (var item in UserCardGamesDict)
						ListViewSourse.Add($"Game: {item.Key} | Price: ${item.Value}");

					CanCloseSaveCards = true;
				}

				else
				{
					MessageBox.Show("No games from DIG", "No dig games error");
					return;
				}
			});
		}

		/// <summary>
		/// Get information abount all available games 
		/// </summary>
		public void GetAvailableGamesInfo()
		{
			var isContinueGames = _parseService.GetUserGamesInfo();

			_page.Dispatcher.Invoke(() =>
			{
				if (isContinueGames && UserName != null)
					foreach (var parsedItem in _parseService.UserGames)
						UserGames.Add(parsedItem);
				else
				{
					MessageBox.Show("No entered user", "No user error");
					return;
				}

				if (ViewModelHelper.DigGames.Count != 0)
				{
					var tempList = ViewModelHelper.DigGames
						.Where(item => !UserGames.Contains(item))
						.ToList();

					TempList = tempList;

					var twmpList = new int[ViewModelHelper.DigGames.Count];

					for (int i = 0; i < ViewModelHelper.DigGames.Count; i++)
					{
						if (!UserGames.Any(x => x == ViewModelHelper.DigGames[i]))
							twmpList[i] = i;
						else
							twmpList[i] = 0;
					}

					PricesAll.Clear();
					PricesAll.Add(0.05);
					for (int i = 0; i < twmpList.Length; i++)
						if (twmpList[i] != 0)
							PricesAll.Add(ViewModelHelper.PricesAll[i]);

					GamesAllSumm = Math.Round(PricesAll.Sum(), 3);

					AmountGamesAll = TempList.Distinct().Count();

					for (var i = 0; i < TempList.Count; i++)
						if (!UserAllGamesDict.ContainsKey(TempList[i]))
							UserAllGamesDict.Add(TempList[i], PricesAll[i]);

					/// write user all info into helper class

					ViewModelHelper.AllGamesUserHelper = AmountGamesAll;
					ViewModelHelper.AllGamesUserPriceHelper = GamesAllSumm;

					ViewModelHelper.UserName = UserName;

					TempList.Clear();
					ListViewSourse.Clear();

					foreach (var item in UserAllGamesDict)
						ListViewSourse.Add($"Game: {item.Key} | Price: ${item.Value}");

					CanCloseSaveAll = true;
				}

				else
				{
					MessageBox.Show("No games from DIG", "No dig games error");
					return;
				}

				CanCloseAvailableAllCards = true;
			});
		}

		/// <summary>
		/// Get all information about user
		/// </summary>
		/// <param name="enteredUserName"> Entered username by user </param>
		public void GetUserInfo(string enteredUserName)
		{
			if (enteredUserName == null) return;

			enteredUserName = enteredUserName.Remove(0, enteredUserName.LastIndexOf("/") + 1);

			var isContinue = _parseService.GetUserAccountInfo(enteredUserName);

			_page.Dispatcher.Invoke(() =>
			{
				if (isContinue)
				{
					// write UserInfo data
					_userData.UserName = _parseService.UserNameParse;
					_userData.UserGameCount = _parseService.GameCountParse;
					_userData.UserCountryCode = _parseService.UserCountryCodeParse;
					_userData.UserAvatar = _parseService.UserAvatarParse;
					_userData.UserProfileStatus = _parseService.UserProfileStatusParse;
					_userData.UserProfileState = _parseService.UserProfileStateParse;

					// initialize ViewModel properties
					UserName = _userData.UserName;
					UserGameCount = _userData.UserGameCount;
					UserCountryCode = _userData.UserCountryCode;
					UserAvatar = _userData.UserAvatar;
					UserProfileStatus = _userData.UserProfileStatus;
					UserProfileState = _userData.UserProfileState;

					AmountGamesAll = 0;
					AmountGamesWithCards = 0;
					GamesAllSumm = 0;
					GamesWithCardsSumm = 0;
					PricesAll.Clear();
					PricesOnlyCards.Clear();
					UserGames.Clear();

					CanCloseAvailableAll = true;
				}
			});
		}
	}
}
