using System.Windows.Controls;

namespace DIGStatus
{
    class MainPageViewModel : BaseViewModel
    {
		// current page
		private Page _page;

		// info for main page to show
		public string Info { get; set; }

		public MainPageViewModel(Page page)
		{
			_page = page;

			Info = "📌 Hi, this is DIG informer program\n\n" +
				"📐 Here you can: \n\n" +
				"✅ Find all available Daily Indie Game games\n" +
				"✅ Find how much games you can buy\n" +
				"✅ Find how much money you can spend to buy all games\n" +
				"✅ Save all needed info into file\n" +
				"✅ Build well designed chart, where you can visually see all info\n\n" +
				"📕 Also you can link on my github, here you can find all sourse code\n" +
				"	(if you have a bug or an idea, just open an issue)\n\n" +
				"🍀 Well, good luck, hope you'll enjoy my program ^,,,^";
		}
	}
}
