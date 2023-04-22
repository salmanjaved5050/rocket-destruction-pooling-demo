using UnityEngine;

namespace RocketDestruction.Art._3d.unity_chan_.Unity_chan__Model.SplashScreen.Scripts
{
	[ExecuteInEditMode]
	public class SplashScreen : MonoBehaviour
	{
		void NextLevel ()
		{
			Application.LoadLevel (Application.loadedLevel + 1);
		}
	}
}