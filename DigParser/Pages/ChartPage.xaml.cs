using System.Windows.Controls;

namespace DIGStatus
{
	public partial class ChartPage : Page
	{
		public ChartPage()
		{
			InitializeComponent();

			DataContext = new ChartViewModel(this);
		}
	}
}
