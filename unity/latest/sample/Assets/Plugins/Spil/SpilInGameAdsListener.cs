using System;
using UnityEngine;

namespace Spil {
	
	public interface SpilInGameAdsListener {
	   	/**
		 * Method to callback when a in game ad has been retrieved from the server.
		 * This view will handle the display event and will mark the advert as shown.
		 * @param	billboard	A GameObject that will respond to the events when it's displayed and clicked.
		 */
		void AdDidLoadIngameAsset(GameObject billboard);
	  
	  	/**
		 * Method to callback when an error happened trying to retrieve the ad from the server.
		 * @param	error	An error code describing the cause of the error.
		 */
	  	void AdDidFailIngameAsset(string error);
	}
}

