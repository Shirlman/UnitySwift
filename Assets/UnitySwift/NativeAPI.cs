using System.Runtime.InteropServices;
using UnityEngine;

public class NativeAPI 
{
	[DllImport("__Internal")]
	private static extern void presentNativeViewController();

	public static void presentNativeView() {
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			presentNativeViewController();
		}
	}
}

