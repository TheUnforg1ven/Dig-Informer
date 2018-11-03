using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DIGStatus
{
    class ChartViewModel : BaseViewModel
    {
		// current page
		private Page _page;

		#region Public Properties

		// collection to create chart
		public SeriesCollection SeriesCollection { get; set; }

		// tip string
		public string Tip => "Press 'Start' to draw chart\nPress 'Clear' to clear chart";

		// labels array for chart
		public string[] Labels { get; set; }

		// Func to Format chart values
		public Func<double, string> Formatter { get; set; }

		/// <summary>
		/// Username prop
		/// </summary>
		public string UserName { get; private set; }

		/// <summary>
		/// Amount of all games from dig
		/// </summary>
		public int AllGamesHelper { get; private set; }

		/// <summary>
		/// Amount of all available games for user
		/// </summary>
		public int AllGamesCardsHelper { get; private set; }
		
		/// <summary>
		/// Price of all dig games
		/// </summary>
		public double AllGamesPriceHelper { get; private set; }

		/// <summary>
		/// Price of all dig games with cards
		/// </summary>
		public double AllGamesPriceCardsHelper { get; private set; }

		/// <summary>
		/// Amount of all available games for user
		/// </summary>
		public int AllGamesUserHelper { get; private set; }

		/// <summary>
		/// Amount of all available games with cards for user
		/// </summary>
		public int AllGamesCardsUserHelper { get; private set; }

		/// <summary>
		/// Price of all games available for user
		/// </summary>
		public double AllGamesUserPriceHelper { get; private set; }

		/// <summary>
		/// Price of all games with cards available for user
		/// </summary>
		public double AllGamesPriceCardsUserHelper { get; private set; }

		#endregion

		// command to draw chart
		public ICommand StartCommand { get; set; }

		// command to clear chart
		public ICommand ClearCommand { get; set; }

		// command to get using tips
		public ICommand TipsCommand { get; set; }

		// simle constructor
		public ChartViewModel(Page page)
		{
			_page = page;

			StartCommand = new RelayCommand(() => Task.Factory.StartNew(InjectColumn));

			ClearCommand = new RelayCommand(() => 
			{
				if (SeriesCollection != null)
					SeriesCollection.Clear();
			});

			TipsCommand = new RelayCommand(() => MessageBox.Show(Tip, "Tips message"));
		}

		/// <summary>
		/// Set class properties to show chart later
		/// </summary>
		private void SetValues()
		{
			UserName = ViewModelHelper.UserName;

			AllGamesHelper = ViewModelHelper.AllGamesHelper;
			AllGamesCardsHelper = ViewModelHelper.AllGamesCardsHelper;
			AllGamesPriceHelper = ViewModelHelper.AllGamesPriceHelper;
			AllGamesPriceCardsHelper = ViewModelHelper.AllGamesPriceCardsHelper;

			AllGamesUserHelper = ViewModelHelper.AllGamesUserHelper;
			AllGamesCardsUserHelper = ViewModelHelper.AllGamesCardsUserHelper;
			AllGamesUserPriceHelper = ViewModelHelper.AllGamesUserPriceHelper;
			AllGamesPriceCardsUserHelper = ViewModelHelper.AllGamesPriceCardsUserHelper;
		}

		/// <summary>
		/// Inject colums into chart
		/// </summary>
		private void InjectColumn()
		{
			SetValues();

			if (AllGamesHelper != 0)
			{
				_page.Dispatcher.Invoke(() =>
				{
					SeriesCollection = new SeriesCollection
				{
					// dig column
					new ColumnSeries
					{
						Title = "DIG",
						Values = new ChartValues<double>
						{
							AllGamesHelper,
							AllGamesCardsHelper,
							AllGamesPriceHelper,
							AllGamesPriceCardsHelper,
						}
					},

					// user column
					new ColumnSeries
					{
						Title = "User",
						Values = new ChartValues<double>
						{
							AllGamesUserHelper,
							AllGamesCardsUserHelper,
							AllGamesUserPriceHelper,
							AllGamesPriceCardsUserHelper,
						}
					},
				};

					// initialize Labels array
					Labels = new[] { "Games", "Games with cards", "Games price", "Games with cards price" };
				});
			}
			else
				MessageBox.Show("No available info!", "No info message");
			
		}
	}
}
