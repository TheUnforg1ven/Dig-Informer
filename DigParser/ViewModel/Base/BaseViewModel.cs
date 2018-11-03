using System.ComponentModel;

namespace DIGStatus
{
	/// <summary>
	/// A base view model that fires property changed event as needed 
	/// </summary>
	public class BaseViewModel : INotifyPropertyChanged
	{
		// event fired, when any child property changes its value
		public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

		// call this to fire a "PropertyChanged" event
		public void OnPropertyChanged(string name)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
	}
}
