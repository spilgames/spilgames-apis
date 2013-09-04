using System;
using UnityEngine;

namespace Spil {
	
	public interface SpilInGameAdsListener {
	   	/**
		 * Method to call back if IGA has been loaded.
		 */
		void AdDidLoadIngameAsset(GameObject billboard);
	  
	  	/**
	   	 * Method to call back if IGA has any problem while been loaded
	   	 */
	  	void AdDidFailIngameAsset(string error);
	}
}

