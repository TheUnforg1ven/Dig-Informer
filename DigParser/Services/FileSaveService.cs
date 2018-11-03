using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace DIGStatus
{
    class FileSaveService
    {
		// page member
		private Page _page;

		// dict to write into file
		private Dictionary<string, double> _dictToSave;

		// simple initialize constructor
		public FileSaveService(Page page)
		{
			_page = page;
			_dictToSave = new Dictionary<string, double>();
		}

		/// <summary>
		/// Save all info into fiel
		/// </summary>
		/// <param name="dist"> Dictionary with user games names and prices </param>
		public void Save(Dictionary<string, double> dist)
		{
			_dictToSave = dist;
			SaveIntoFile(_dictToSave);
		}

		/// <summary>
		/// All file saving logic here
		/// </summary>
		/// <param name="dist"> Dictionary with user games names and prices </param>
		private void SaveIntoFile(Dictionary<string, double> dist)
		{
			using (var sfd = new SaveFileDialog())
			{
				sfd.Filter = "Text file (*.txt)|*.txt";
				sfd.FilterIndex = 2;

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					Task.Factory.StartNew(() => 
					{
						using (var writer = new StreamWriter(sfd.OpenFile()))
							foreach (var line in dist)
								writer.WriteLine($"Game: {line.Key} | Price: {line.Value}");
					});
				}
			}
		}
    }
}
