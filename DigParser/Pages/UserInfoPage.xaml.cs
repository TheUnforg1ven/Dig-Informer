using System.Windows.Controls;
using System.Windows.Navigation;

namespace DIGStatus
{
	public partial class UserInfoPage : Page
	{
		public UserInfoPage()
		{
			InitializeComponent();
			DataContext = new UserInfoViewModel(this);
		}
	}
}
