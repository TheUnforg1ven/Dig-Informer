using System.Windows;
using DIGStatus;

namespace DIGStatus
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			this.DataContext = new WindowViewModel(this);
		}
	}
}
