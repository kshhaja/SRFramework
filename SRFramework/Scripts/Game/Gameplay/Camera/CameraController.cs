using UnityEngine;
using UnityEngine.InputSystem;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace SlimeRPG
{
	public class CameraController : MonoBehaviour
	{
		public GameObject target;
		
		public float minDistance = 0.5f;
		public float maxDistance = 5.0f;

		public Vector3 targetOffset = Vector3.zero;
		public Vector3 cameraOffset = new Vector3(0f, 6.0f, -5.0f);

		public float zoomAmount = 0.1f;
		public float smoothingSpeed = 2.0f;

		private bool inputMouseScrollUp;
		private bool inputMouseScrollDown;
		private float magnitude = 1.0f;


		private void Start()
		{
			if (target == null) 
				target = GameObject.FindWithTag("Player");
		}

		private void FixedUpdate()
		{
			if (!target) 
				return;

			inputMouseScrollUp = Mouse.current.scroll.ReadValue().y > 0f;
			inputMouseScrollDown = Mouse.current.scroll.ReadValue().y < 0f;	

			if (inputMouseScrollUp)
                magnitude += zoomAmount;
			else if (inputMouseScrollDown)
				magnitude -= zoomAmount;

			magnitude = Mathf.Max(0, magnitude);
			
			var internalTargetOffset = target.transform.position + targetOffset;
			Vector3 pos = new Vector3(cameraOffset.x, cameraOffset.y * magnitude, cameraOffset.z * magnitude);
			transform.position = Vector3.Lerp(transform.position, internalTargetOffset + pos, smoothingSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(internalTargetOffset - transform.position), Time.deltaTime * smoothingSpeed);
		}
	}
}