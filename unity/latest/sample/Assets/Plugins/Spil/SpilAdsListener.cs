using System;

namespace Spil {
	/**
	 * Interface to listen the events triggered by the Ads subsystem.
	 */
	public interface SpilAdsListener {
		/**
		 * Method to call back after the ads subsystem is successfully started.
		 * Can be used to notify the game that can request ads.
		 */
		void AdDidStart();
		
		/**
		 * Method to call back after if the ad subsystem couldn't be started due to any reason.
		 * Can be used to track the problem, or to disable the some functionalities of the game, or
		 * to use some placeholder images instead.
		 * @see SpilUnity.trackEvent()
		 * @param	error	The reason why the ad subsystem failed to start.
		 */
		void AdDidFailToStart(string error);
		
		/**
		 * Method to call back before the next ad is going to be displayed. 
		 * If there is no ad to show, this method won't be called back.
		 * Can be used to pause the game or run some other tasks.
		 */
		void AdWillAppear();
		
		/**
		 * Method to call back after the ad is displayed. This method is only called if the ads are enabled to 
		 * be displayed (AdsEnabled(true)).
		 * Can be use to track some action.
		 * @see Spil.AdsEnabled()
		 */
		void AdDidAppear();
		
		/**
		 * Method to call back if the ad couldn't be displayed due to any reason.
		 * Can be used to track the problem. This method will be called,
		 * if there is no ads available to show. This means the system is working fine,
		 * just that the app consumed all the ads available or there is no campaigns configured
		 * for this app yet.
		 * @see SpilUnity.trackEvent()
		 * @param	error	The reason why the ad failed to be displayed.
		 */
		void AdDidFailToAppear(string error);
		
		/**
		 * Method to call back before the next more games' screen is going to be shown.
		 * Can be used to stop the sound or pause the game.
		 */
		void AdMoreGamesWillAppear();
		
		/**
		 * Method to call back after the more games' screen is displayed.
		 */
		void AdMoreGamesDidAppear();
		
		/**
		 * Method to call back if the more games' screen couldn't be displayed due to any reason.
		 * Can be used to track the problem.
		 * @see SpilUnity.trackEvent()
		 * @param	error	The reason why the more games' screen failed to be displayed.
		 */
		void AdMoreGamesDidFailToAppear(string error);
		
		/**
		 * Method to call back if the more games popup showed was dismissed.
		 * Can be used to resume the sound or resume the game.
		 */
		void AdMoreGamesDidDismiss();
		
		/**
		 * Method to call back if the ad popup showed was dismissed.
		 * Can be used to resume the sound or resume the game.
		 */
		void AdPopupDidDismiss();
	}
}

