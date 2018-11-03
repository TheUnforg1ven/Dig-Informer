using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DIGStatus
{
    class DigGamesViewModel : BaseViewModel
    {
		// the reference to the current Page
		private Page _page;

		// settings for parsing dig page
		private DigSettings _digSettings;

		// token for cancellation 'Start' task
		private CancellationTokenSource _tokenSource;

		// sum of all found games
		public double TotalSumm { get; set; }

		// sum of all found games, containing cards
		public double TotalSummWithCards { get; set; }

		// amount of all games
		public int AllGames { get; set; }

		// amount of all games with cards
		public int AllGamesWithCards { get; set; }

		// disable/enable "Get all games" button
		public bool CanCloseGetAll { get; set; }

		// disable/enable "Clear" button
		public bool CanCloseClear { get; set; }

		// disable/enable "Stop" button
		public bool CanCloseStop { get; set; }

		// ObservableCollection of all found dig games
		public ObservableCollection<string> DigGames { get; set; }

		// ObservableCollection of all found dig games with cards
		public ObservableCollection<string> DigGamesWithCrads { get; set; }
		
		// ObservableCollection with prices of all dig games
		public ObservableCollection<double> Prices { get; set; }

		// ObservableCollection with prices of all dig games with cards
		public ObservableCollection<double> PricesOnlyCards { get; set; }

		// Command to start parsing dig
		public ICommand StartCommand { get; set; }

		// Command to stop parsing dig
		public ICommand StopCommand { get; set; }

		// Command to clear current page
		public ICommand ClearCommand { get; set; }

		public DigGamesViewModel(Page page)
		{
			_page = page;

			// initialize model data
			_digSettings = new DigSettings();

			// bool commands for buttons
			CanCloseGetAll = true;
			CanCloseClear = true;
			CanCloseStop = false;

			DigGames = new ObservableCollection<string>();
			DigGamesWithCrads = new ObservableCollection<string>();

			Prices = new ObservableCollection<double>();
			PricesOnlyCards = new ObservableCollection<double>();

			// initialize token and start parsing
			StartCommand = new RelayCommand(() => 
			{
				CanCloseStop = true;
				CanCloseGetAll = false;
				CanCloseClear = false;
				_tokenSource = new CancellationTokenSource();
				Task.Factory.StartNew(() => Parse(_tokenSource.Token));
			});

			StopCommand = new RelayCommand(() => 
			{
				CanCloseGetAll = true;
				CanCloseClear = true;
				_tokenSource.Cancel();
			});

			ClearCommand = new RelayCommand(() => Clear());
		}

		/// <summary>
		/// Parse needed info from DIG
		/// </summary>
		/// <param name="token"> token to check if task was cancelled </param>
		public void Parse(CancellationToken token)
		{
			_page.Dispatcher.Invoke(() => 
			{
				DigGames.Clear();
				DigGamesWithCrads.Clear();
				Prices.Clear();
				PricesOnlyCards.Clear();
				TotalSumm = 0;
				TotalSummWithCards = 0;
				AllGames = 0;
				AllGamesWithCards = 0;
			});

			for (int i = _digSettings.StartPoint; i <= _digSettings.EndPoint; i++)
			{
				var document = ParseSettingsAsync(i).Result;

				if (document is null) return;

				if (token.IsCancellationRequested) return; //token.ThrowIfCancellationRequested();

				var menuItems = document.QuerySelectorAll("tr").Where(item => item.ClassName != null && item.ClassName.Contains("DIG3_14_Gray"));

				foreach (var item in menuItems)
				{
					FindAllGames(item.TextContent);

					if (item.TextContent.Contains("Yes"))
						FindCardGames(item.TextContent);					
				}
			}

			TotalSumm = Math.Round(Prices.Sum(), 3);
			AllGames = DigGames.Count;

			TotalSummWithCards = Math.Round(PricesOnlyCards.Sum(), 3);
			AllGamesWithCards = DigGamesWithCrads.Count;

			/// write into helper class amount and values
			ViewModelHelper.AllGamesHelper = AllGames;
			ViewModelHelper.AllGamesCardsHelper = AllGamesWithCards;

			ViewModelHelper.AllGamesPriceHelper = TotalSumm;
			ViewModelHelper.AllGamesPriceCardsHelper = TotalSummWithCards;

			var nameDigGames = new ObservableCollection<string>();
			var nameDigGamesWithCards = new ObservableCollection<string>();

			foreach (var item in DigGames)
				nameDigGames.Add(item.Remove(item.LastIndexOf("STEAM")).TrimEnd());

			foreach (var item in DigGamesWithCrads)
			{
				var temp = item.Remove(item.LastIndexOf("STEAM")).TrimEnd();
				if(temp.Contains("Yes"))
					temp = item.Remove(item.LastIndexOf("Yes")).TrimEnd();
				nameDigGamesWithCards.Add(temp);
			}

			ViewModelHelper.DigGames.Clear();
			ViewModelHelper.DigGamesWithCards.Clear();
			ViewModelHelper.PricesAll.Clear();
			ViewModelHelper.PricesWithCards.Clear();

			foreach (var item in nameDigGames)
				ViewModelHelper.DigGames.Add(item);

			foreach (var item in nameDigGamesWithCards)
				ViewModelHelper.DigGamesWithCards.Add(item);

			foreach (var item in Prices)
				ViewModelHelper.PricesAll.Add(item);

			foreach (var item in PricesOnlyCards)
				ViewModelHelper.PricesWithCards.Add(item);

			CanCloseGetAll = true;
			CanCloseClear = true;
		}

		/// <summary>
		/// Clear all page
		/// </summary>
		private void Clear()
		{
			_page.Dispatcher.Invoke(() =>
			{
				DigGames.Clear();
				DigGamesWithCrads.Clear();
				Prices.Clear();
				PricesOnlyCards.Clear();
				TotalSumm = 0;
				TotalSummWithCards = 0;
				AllGames = 0;
				AllGamesWithCards = 0;
			});
		}

		/// <summary>
		/// Find all dig games with cards
		/// </summary>
		/// <param name="textItemValue"> string to parse </param>
		private void FindCardGames(string textItemValue)
		{
			var updatedResult = string.Join(" ", textItemValue.Replace("Buy key", string.Empty).Split().Where(x => x != ""));
			updatedResult = updatedResult.Remove(0, updatedResult.IndexOf(' ') + 1);
			_page.Dispatcher.Invoke(() => DigGamesWithCrads.Add(updatedResult));

			var price = updatedResult.Remove(0, updatedResult.IndexOf("$") + 1).Replace(".", ",");
			PricesOnlyCards.Add(Convert.ToDouble(price));
		}

		/// <summary>
		/// Find all dig games
		/// </summary>
		/// <param name="textItemValue"> string to parse </param>
		private void FindAllGames(string textItemValue)
		{
			var updatedResult = string.Join(" ", textItemValue.Replace("Buy key", string.Empty).Split().Where(x => x != ""));
			updatedResult = updatedResult.Remove(0, updatedResult.IndexOf(' ') + 1);
			_page.Dispatcher.Invoke(() => DigGames.Add(updatedResult));

			var price = updatedResult.Remove(0, updatedResult.IndexOf("$") + 1).Replace(".", ",");
			Prices.Add(Convert.ToDouble(price));
		}

		/// <summary>
		/// Method to parse Daily Indie Game
		/// </summary>
		/// <param name="index"> Page index </param>
		/// <returns> Parsed task with IHtmlDocument </returns>
		private async Task<IHtmlDocument> ParseSettingsAsync(int index)
		{
			var _client = new HttpClient();
			var _domParser = new HtmlParser();

			var response = await _client.GetAsync($"http://dailyindiegame.com/content_marketplace_{index}.html");
			string sourse = null;

			if (response != null && response.StatusCode == HttpStatusCode.OK)
				sourse = await response.Content.ReadAsStringAsync();

			var document = _domParser.Parse(sourse);

			return document;
		}
	}
}
