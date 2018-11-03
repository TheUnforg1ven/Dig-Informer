using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DIGStatus
{
	[Serializable]
	class SerializationService
    {
		// binary formatter
		private BinaryFormatter _formatter;

		// path to save file
		private readonly string path = @"userdata.dat";

		// simple constructor
		public SerializationService() => _formatter = new BinaryFormatter();

		/// <summary>
		/// Serialize given list with games
		/// </summary>
		/// <param name="listToSave"> list with games to serialize </param>
		public void SerializeSettings(ObservableCollection<string> listToSave)
		{
			try
			{
				using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
				{
					foreach (var line in listToSave)
					{
						_formatter.Serialize(fs, line);
					}
					
				}
			}

			catch (IOException ex) { Console.WriteLine(ex); }
		}

		/// <summary>
		/// DeSerialize needed list with games
		/// </summary>
		/// <returns> After deserializing returning list </returns>
		public ObservableCollection<string> DeSerializeSettings()
		{
			var returnListSetting = new ObservableCollection<string>();

			try
			{
				using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
				{
					returnListSetting.Add((string)_formatter.Deserialize(fs));
				}
			}

			catch (IOException ex) { Console.WriteLine(ex); }

			return returnListSetting;
		}
	}
}
