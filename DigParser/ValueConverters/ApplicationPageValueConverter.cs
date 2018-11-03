using System;
using System.Diagnostics;
using System.Globalization;

namespace DIGStatus
{
	/// <summary>
	/// Converts the "ApplicationPage" to an actual view/page
	/// </summary>
	public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//find the appropriate page
			switch ((ApplicationPage)value)
			{
				case ApplicationPage.User:
					return new UserInfoPage();

				case ApplicationPage.DigGame:
					return new DigGames();

				case ApplicationPage.MainPage:
					return new MainPage();

				case ApplicationPage.ChartPage:
					return new ChartPage();

				default:
					Debugger.Break();
					return null;
			}
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
