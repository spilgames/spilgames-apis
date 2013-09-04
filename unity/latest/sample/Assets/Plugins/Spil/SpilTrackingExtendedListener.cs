using System;
using LitJson;

namespace Spil {
	/**
	 * Interface to listen the responses from the Extended Tracking subsystem
	 */
	public interface SpilTrackingExtendedListener{
		/**
		 * Method to call back when the any of the extended trackers are started.
		 * If the camera tracker is set up this method is called after the confirmation pop up is done, and 
		 * if there is at least one tracker active. When the camera tracker is not set up, this method is
		 * called when any of the other are activated.
		 */
		void TrackExtendedDidStart();
		
		/**
		 * Method to call back when ALL the extended trackers are stopped.
		 * This is an informative call.
		 */
		void TrackExtendedDidStop();
	}
}

