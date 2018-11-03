using System.Windows.Controls;

namespace DIGStatus
{
    public partial class DigGames : Page
    {
        public DigGames()
        {
            InitializeComponent();
			DataContext = new DigGamesViewModel(this);
		}
    }
}
