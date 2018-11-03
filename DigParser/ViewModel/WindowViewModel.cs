using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DIGStatus
{
	public class WindowViewModel : BaseViewModel
	{
		#region Private Members

		// simple window variable
		private Window _window;

		// margin around the window for a shadow drop
		private int outerMarginSize = 10;

		// radius if the edges of the window
		private int windowRadius = 10;

		// The last known dock position
		private WindowDockPosition _dockPosition = WindowDockPosition.Undocked;

		// The page with user's steam account info
		private Page UserInfoPage;

		// The page for parcing Dig
		private Page DigGamesPage;

		// The main prog page
		private Page MainPage;

		// The chart page
		private Page ChartPage;

		#endregion

		#region Public Properties

		// the smalles width window can go to
		public double WindowMinimumWidth { get; set; } = 400;

		// the smalles height window can go to
		public double WindowMinimumHeight { get; set; } = 400;

		// True if the window should e borderless, casue its docked or maximized
		public bool Borderless { get => (_window.WindowState == WindowState.Maximized || _dockPosition != WindowDockPosition.Undocked); }

		// The size of the resize border around the window
		public int ResizeBorder { get => Borderless ? 0 : 6; }

		// The size of the resize border around the window, taking the outer margin
		public Thickness ResizeBorderThickness { get => new Thickness(ResizeBorder + OuterMarginSize); }

		// The padding of the inner content of the main window
		public Thickness InnerContentPadding { get; set; } = new Thickness(0);

		// Margin around the window to allow for a shadow drop
		public int OuterMarginSize
		{
			get => _window.WindowState == WindowState.Maximized ? 0 : outerMarginSize;

			set => outerMarginSize = value;
		}

		// Margin around the window to allow for a shadow drop
		public Thickness OuterMarginSizeThickness => new Thickness(OuterMarginSize); 

		// Radius if the edges of the window
		public int WindowRadius
		{
			get => _window.WindowState == WindowState.Maximized ? 0 : windowRadius;

			set => windowRadius = value;
		}

		// Radius if the edges of the window
		public CornerRadius WindowCornerRadius { get => new CornerRadius(WindowRadius); }

		// The height of the title bar/caption of the window
		public int TitleHeight { get; set; } = 42;

		// Radius if the edges of the window
		public GridLength TitleHeightGridLength { get => new GridLength(TitleHeight + ResizeBorder); }

		// Current page available for user
		public Page CurrentPage { get; set; }

		// speed while changing frames
		public double FrameOpasity { get; set; }

		#endregion

		#region Public Commands

		// the command to minimize the window
		public ICommand MinimizeCommand { get; set; }

		// the command to maximize the window
		public ICommand MaximizeCommand { get; set; }

		// the command to close the window
		public ICommand CloseCommand { get; set; }

		// the command to show the system menu of the window
		public ICommand MenuCommand { get; set; }

		// open UserInfo Page
		public ICommand UserInfoCommand { get; set; }
		
		// open DigGames Page
		public ICommand FindDigGamesCommand { get; set; }

		// open MainPage Page
		public ICommand MainPageCommand { get; set; }

		// open ChartPage Page
		public ICommand ChartPageCommand { get; set; }

		// open sourse code on github
		public ICommand OpenGitHub { get; set; }

		// open Daily Indie Game site
		public ICommand OpenDailyIndieGame { get; set; }


		#endregion

		/// <summary>
		/// Simple constructor
		/// </summary>
		/// <param name="window"> Current window </param>
		public WindowViewModel(Window window)
		{
			_window = window;

			// listen out for the window resizing
			_window.StateChanged += (sender, e) => 
			{
				// Fire off events for all properties that are affected by a resize
				OnPropertyChanged(nameof(ResizeBorderThickness));
				OnPropertyChanged(nameof(OuterMarginSize));
				OnPropertyChanged(nameof(OuterMarginSizeThickness));
				OnPropertyChanged(nameof(WindowRadius));
				OnPropertyChanged(nameof(WindowCornerRadius));
			};

			// initialize custom 'UserInfoPage'
			UserInfoPage = new UserInfoPage();

			// initialize custom 'DigGamesPage'
			DigGamesPage = new DigGames();

			// initialize custom 'MainPage'
			MainPage = new MainPage();

			// initialize custom 'ChartPage'
			ChartPage = new ChartPage();

			FrameOpasity = 1;

			CurrentPage = MainPage;

			UserInfoCommand = new RelayCommand(() => SlowOpasity(UserInfoPage));
			FindDigGamesCommand = new RelayCommand(() => SlowOpasity(DigGamesPage));
			MainPageCommand = new RelayCommand(() => SlowOpasity(MainPage));
			ChartPageCommand = new RelayCommand(() => SlowOpasity(ChartPage));

			// Create commands
			MinimizeCommand = new RelayCommand(() => _window.WindowState = WindowState.Minimized);
			MaximizeCommand = new RelayCommand(() => _window.WindowState ^= WindowState.Maximized);
			CloseCommand = new RelayCommand(() => _window.Close());
			MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(_window, _window.PointToScreen(Mouse.GetPosition(_window))));

			OpenGitHub = new RelayCommand(() => Process.Start(new ProcessStartInfo("https://github.com/TheUnforg1ven/Dig-Informer")));
			OpenDailyIndieGame = new RelayCommand(() => Process.Start(new ProcessStartInfo("http://dailyindiegame.com/content_marketplace_0.html")));

			// Fix window resize issue (found this class on the ashes of internet)
			var resizer = new WindowResizer(_window);
		}

		/// <summary>
		/// Method to slow opasity while changing pages
		/// </summary>
		/// <param name="page"> PAge to show </param>
		private async void SlowOpasity(Page page)
		{
			await Task.Factory.StartNew(() => 
			{
				for (var i = 1.0; i > 0.0; i -= 0.1)
				{
					FrameOpasity = i;
					Thread.Sleep(50);
				}

				CurrentPage = page;

				for (var i = 0.0; i < 1.1; i += 0.1)
				{
					FrameOpasity = i;
					Thread.Sleep(50);
				}
			});
		}
	}
}
