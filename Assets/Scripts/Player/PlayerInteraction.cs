using UnityEngine;

namespace Player {
	public class PlayerInteraction : MonoBehaviour
	{
		[SerializeField]
		private Camera _camera;

		/// <summary>
		///  The object carried by the player
		/// </summary>
		private Transform _carriedObject;

		/// <summary>
		///  The distance to hold carried items
		/// </summary>
		[SerializeField]
		private float _carryDistance = 1.5f;

		/// <summary>
		///  The Cached environment hit
		/// </summary>
		private RaycastHit _hit;

		/// <summary>
		///  The maximum distance to allow interaction
		/// </summary>
		[SerializeField]
		private float _interactionDistance = 2f;

		/// <summary>
		///  The currently selected (hovered) item
		/// </summary>
		private Transform _selectedObject;

		/// <summary>
		///  Try to Carry an object
		/// </summary>
		/// <param name="obj">The Transform of the object to carry</param>
		public void CarryObject(Transform obj)
		{
			if (_carriedObject != null)
			{
				// we are carying something alreeady
				return;
			}

			obj.GetComponent<Rigidbody>()
			   .isKinematic = true;
			_carriedObject = obj.transform;
		}

		/// <summary>
		///  Try to drop an object
		/// </summary>
		/// <param name="obj">The Transform of the object to drop</param>
		public void DropObject(Transform obj)
		{
			if (_carriedObject == obj)
			{
				// Drop the object
				obj.GetComponent<Rigidbody>()
				   .isKinematic = false;
				_carriedObject = null;
			}
		}

		// Start is called before the first frame update
		private void Start()
		{
			if (_camera == null)
			{
				_camera = GetComponentInChildren<Camera>();
			}
		}

		// Update is called once per frame
		private void Update()
		{
			// if we are carying something, then just carry it
			if (_carriedObject != null)
			{
				// execute any action on the carried object
				if (Input.GetButtonDown("Action"))
				{
					_carriedObject.transform.SendMessageUpwards("DoAction", transform);
				}

				UpdateCarriedObject();

				return;
			}

			// get a ray from the center of the viewport
			var ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

			//Debug.DrawRay(ray.origin, ray.direction,Color.red,0.1f);

			// Raycast against the interactable layer
			if (Physics.Raycast(ray, out _hit, _interactionDistance, LayerMask.GetMask("Interactable")))
			{
				// if we have a hit, then do some processing
				// cache the object
				_selectedObject = _hit.transform;

				// send a message to the interactable object
				_selectedObject.SendMessageUpwards("DisplayDetailUI", SendMessageOptions.DontRequireReceiver);

				if (Input.GetButtonDown("Action"))
				{
					_selectedObject.transform.SendMessageUpwards("DoAction", transform);
				}
			}
			else
			{
				_selectedObject = null;
			}
		}

		/// <summary>
		///  Move the carried object towards the carry position.
		/// </summary>
		private void UpdateCarriedObject()
		{
			if (_carriedObject != null)
			{
				var camTransform = _camera.transform;
				var pos = camTransform.position + (camTransform.forward * _carryDistance);
				var rot = Quaternion.LookRotation(camTransform.forward, Vector3.up);
				var rot2 = Quaternion.Euler(0, rot.eulerAngles.y, 0);

				if (_carriedObject.gameObject.GetComponent<Rigidbody>()
				                  .isKinematic)
				{
					_carriedObject.position = Vector3.Lerp(_carriedObject.position, pos, Time.deltaTime * 100.0f);
					_carriedObject.rotation = Quaternion.Lerp(_carriedObject.rotation, rot2, Time.deltaTime * 100.0f);
				}
			}
		}
	}
}