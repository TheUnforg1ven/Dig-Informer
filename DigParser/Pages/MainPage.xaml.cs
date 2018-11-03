using System.Windows.Controls;

namespace DIGStatus
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

			DataContext = new MainPageViewModel(this);
        }
    }
}
