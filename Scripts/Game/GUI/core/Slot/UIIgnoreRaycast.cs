using UnityEngine;

namespace SlimeRPG.UI
{
	public class UIIgnoreRaycast : MonoBehaviour, ICanvasRaycastFilter
	{
		public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			return false;
		}
	}
}
