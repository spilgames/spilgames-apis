using System;

namespace Spil {
	/**
	 * Interface to listen the events triggered by the Ads subsystem.
	 */
	public interface SpilAdsListener {
		/**
		 * Method to call back after the ad subsystem is successfully started.
		 */
		void AdDidStart();
		
		/**
		 * Method to call back after if the ad subsystem couldn't be started due to any reason.
		 * @param	error	The reason why the ad subsystem failed to start.
		 */
		void AdDidFailToStart(string error);
		
		/**
		 * Method to call back before the next ad is going to be displayed. This method is called
		 * every time the timer reach 0, regardless if the ad should be shown or not (enableAds is set to NO).
		 */
		void AdWillAppear();
		
		/**
		 * Method to call back after the ad is displayed. This method is only called if the ads are enabled to 
		 * be displayed (enableAds:YES).
		 */
		void AdDidAppear();
		
		/**
		 * Method to call back if the ad couldn't be displayed due to any reason.
		 * @param	error	The reason why the ad failed to be displayed.
		 */
		void AdDidFailToAppear(string error);
		
		/**
		 * Method to call back before the next more games' screen is going to be shown.
		 */
		void AdMoreGamesWillAppear();
		
		/**
		 * Method to call back after the more games' screen is displayed.
		 */
		void AdMoreGamesDidAppear();
		
		/**
		 * Method to call back if the more games' screen couldn't be displayed due to any reason.
		 * @param	error	The reason why the more games' screen failed to be displayed.
		 */
		void AdMoreGamesDidFailToAppear(string error);
		
		/**
		 * Method to call back if the more games' screen was dismissed.
		 */
		void AdMoreGamesDidDismiss();
		
		/**
		 * Method to call back if the ad' popup was dismissed.
		 */
		void AdPopupDidDismiss();
	}
}

