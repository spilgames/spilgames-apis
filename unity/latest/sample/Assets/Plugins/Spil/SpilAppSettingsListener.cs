using System;
using LitJson;

namespace Spil{
	/**
	 * Interface to listen the responses from the App Settings subsystem
	 */
	public interface SpilAppSettingsListener {
		/**
		 * Method to call back when the settings are finally loaded.
		 * This methods will receive the settings loaded in the form of a dictionary.
		 * The developers should know the structure of the dictionary since they created the default settings file.
		 * @param	data	The settings loaded. The format and the values are defined by the developer of the app.
		 */
		void AppSettingsDidLoad(JsonData data);
		
		/**
		 * Method to call back in case the settings couldn't be loaded.
		 * Usually the reasons to call this method will be:
		 * - if there is any parsing error in the remote settings and in the local settings.
		 * - if there is a connection error, and the file of the defaults cannot be found locally.
		 * @param	error	Error describing what was wrong.
		 */
		void AppSettingsDidFailWithError(string error);
		
		/**
		 * Method to call back when the download of the settings has been started.
		 * Can be use to notify the user or do other tasks until everything is downloaded.
		 */
		void AppSettingsDidStartDownload();
	}
}

