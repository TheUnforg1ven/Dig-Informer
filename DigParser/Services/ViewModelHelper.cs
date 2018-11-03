using System.Collections.ObjectModel;

namespace DIGStatus
{
	/// <summary>
	/// Helper class to exchange info among ViewModels
	/// </summary>
    public class ViewModelHelper
    {
		/// <summary>
		/// All games from Dig
		/// </summary>
		public static ObservableCollection<string> DigGames { get; set; } = new ObservableCollection<string>();

		/// <summary>
		/// Games with Dig with cards
		/// </summary>
		public static ObservableCollection<string> DigGamesWithCards { get; set; } = new ObservableCollection<string>();

		/// <summary>
		/// Prices of all Dig games
		/// </summary>
		public static ObservableCollection<double> PricesAll { get; set; } = new ObservableCollection<double>();

		/// <summary>
		/// Prices of all games with cards
		/// </summary>
		public static ObservableCollection<double> PricesWithCards { get; set; } = new ObservableCollection<double>();

		/// <summary>
		/// Username prop
		/// </summary>
		public static string UserName { get; set; }

		/// <summary>
		/// Amount of all games from dig
		/// </summary>
		public static int AllGamesHelper { get; set; }

		/// <summary>
		/// Amount of all available games for user
		/// </summary>
		public static int AllGamesUserHelper { get; set; }

		/// <summary>
		/// Amount of all available games with cards for user
		/// </summary>
		public static int AllGamesCardsUserHelper { get; set; }

		/// <summary>
		/// Amount of all games with cards from dig
		/// </summary>
		public static int AllGamesCardsHelper { get; set; }

		/// <summary>
		/// Price of all dig games
		/// </summary>
		public static double AllGamesPriceHelper { get; set; }

		/// <summary>
		/// Price of all dig games with cards
		/// </summary>
		public static double AllGamesPriceCardsHelper { get; set; }

		/// <summary>
		/// Price of all games available for user
		/// </summary>
		public static double AllGamesUserPriceHelper { get; set; }

		/// <summary>
		/// Price of all games with cards available for user
		/// </summary>
		public static double AllGamesPriceCardsUserHelper { get; set; }
	}
}
