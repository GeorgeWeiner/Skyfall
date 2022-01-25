using UnityEngine;

namespace _3rdPartyAssets.Plugins.Pixelplacement.iTween.Sample
{
	public class MoveSample : MonoBehaviour
	{	
		void Start(){
			iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
		}
	}
}

