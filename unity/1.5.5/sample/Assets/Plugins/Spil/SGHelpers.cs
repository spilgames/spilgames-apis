using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Spil{
	public class SGHelpers{
		[DllImport ("__Internal")]
		private static extern System.IntPtr getUDID();
			
		[DllImport ("__Internal")]
		private static extern System.IntPtr getAppName();
				
		[DllImport ("__Internal")]
		private static extern System.IntPtr getAppVersion();		
		
		/**
		 * Get the UDID generated for this device.
		 * @return	The UDID generated for this device.
		 */
		public static string GetUDID(){
			if(Application.platform == RuntimePlatform.IPhonePlayer){
				return Marshal.PtrToStringAnsi(getUDID());
			}else{
				return "XXXXXXXXXXXX";
			}
		}
		
		/**
		 * Get the current version of the app from the Info.plist
		 * @return The CFBundleVersion entry in the Info.plist
		 */
		public static string GetAppVersion(){
			if(Application.platform == RuntimePlatform.IPhonePlayer){
				return Marshal.PtrToStringAnsi(getAppVersion());
			}else{
				return null;
			}
		}
		
		/**
		 * Get the current name of the app from the Info.plist
		 * @return The CFBundleName entry in the Info.plist
		 */
		public static string GetAppName(){
			if(Application.platform == RuntimePlatform.IPhonePlayer){
				return Marshal.PtrToStringAnsi(getAppName());
			}else{
				return null;
			}
		}
	}
}

