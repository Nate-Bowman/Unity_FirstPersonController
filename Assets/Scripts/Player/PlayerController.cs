using UnityEngine;

namespace Player {
	[RequireComponent(typeof(CharacterController))]
	public class PlayerController : MonoBehaviour
	{
		private const float Gravity = 9.8f;
		private CharacterController _characterController;
		private Vector3 _characterVelocity;
		private bool _isSprinting;
		private bool _isCrouching;
		private float _cameraVerticalAngle;
	
		[SerializeField][Tooltip("The force applied to the character when jumping")]
		private float JumpForce = 10f;

		[SerializeField][Tooltip("The camera for the first person controller")]
		public Camera _Camera;

		[SerializeField]
		[Tooltip("The fall speed modifier, increases gravity")]
		private float GravityMultiplier = 2f;
	
		[SerializeField][Tooltip("Walking speed of the character")]
		private Vector3 WalkingSpeed = new Vector3(2f,0f,4f); 	
	
		[SerializeField][Tooltip("Sprinting speed of the character")]
		private Vector3 SprintSpeed = new Vector3(2f,0f,9f); 
	
		[SerializeField][Tooltip("Walking speed of the character")]
		private Vector3 AirStrafeSpeed = new Vector3(0.01f,0f,0.01f); 	
	
		[SerializeField][Tooltip("Minimum angle the camera can look up/down")]
		private float MinCameraVerticalAngle = -70;
		[SerializeField][Tooltip("Maximum angle the camera can look up/down")]
		private float MaxCameraVerticalAngle = 55;
	
		[SerializeField][Tooltip("The speed at which the camera moves")]
		private Vector2 CameraSpeed = new Vector2(0.5f,0.5f);

		[SerializeField][Tooltip("Does the sprint button need to be held? false = toggle")]
		private bool HoldToSprint;

		[SerializeField][Tooltip("Does the crouch button need to be held? false = toggle")]
		private bool HoldToCrouch;

		[SerializeField]
		private float crouchedHeight = 0.7f;

		private float _standingHeight;

		// Start is called before the first frame update
		private void Start()
		{
			// cache the character controller component
			_characterController = GetComponent<CharacterController>();
			_standingHeight = _characterController.height;
		}

		// Update is called once per frame
		private void Update()
		{

			HandleCameraMovement();
		
			// Was i grounded last frame?
			var wasGrounded = _characterController.isGrounded;

			// (de)activate the "wanted" crouch state 
			if (HoldToCrouch)
			{
				//crouch  hold
				SetCrouchState(Input.GetButton("Crouch"));
			}
			else
			{
				//crouch toggle
				if (Input.GetButtonDown("Crouch"))
				{
					SetCrouchState(!_isCrouching);
				}
			}
		
			if (HoldToSprint)
			{
				//sprint hold
				_isSprinting = Input.GetButton("Sprint");
			}
			else
			{
				//sprint toggle
				if (Input.GetButtonDown("Sprint"))
				{
					_isSprinting = !_isSprinting;
				}
			}

			if (_characterController.isGrounded)
			{
				//ground movement controls
				HandleGroundMovement();
					
				if (Input.GetButtonDown("Jump"))
				{
					// if we are crouched, then try to stand up
					if (_isCrouching)
					{
						SetCrouchState(false);
					}
				
					// remove vertical velocity
					_characterVelocity.y = 0;

					// add jump velocity
					_characterVelocity += Vector3.up * JumpForce;
				}

			}
			else
			{
				// add air acceleration this frame
				HandleAirMovement();
			}
		
			// add gravitational acceleration this frame
			_characterVelocity += Vector3.down * (Gravity * GravityMultiplier * Time.deltaTime);
		
			// Apply calculated movement
			_characterController.Move(_characterVelocity * Time.deltaTime);
		}

		private bool SetCrouchState(bool crouched)
		{
		
			// if we are already crouching and want to crouch, exit
			// if we are already standing and want to stand, exit
			if (_isCrouching && crouched || !_isCrouching && !crouched)
			{
				return false;
			}
		
			// if we want to be crouched, then crouch
			if (crouched)
			{
				// set the correct controller height for crouch
				_characterController.height = crouchedHeight;
			}
			// else see if we are allowed to stand up
			else 
			{ 
				var controllerRadius = _characterController.radius;
				var transformUp = transform.up;
				var position = transform.position;
			
				// get the top and bottom sphere centers of the capsule when standing
				var topHemisphere = position + (transformUp * controllerRadius);
				var bottomHemisphere = position + (transformUp * (_standingHeight - controllerRadius));

				// see if the capsule will overlap anything that isn't itself when standing up
				// get all the collisions
				var overlapping = Physics.OverlapCapsule(bottomHemisphere, topHemisphere, controllerRadius, -1, QueryTriggerInteraction.Ignore);

				// iterate over the collisions to see if we would hit something
				foreach (var coll in overlapping)
				{
					if (coll != _characterController)
					{
						return false;
					}
				}
			
				// if we are here then we can stand up, change character height to standing height
				_characterController.height = _standingHeight;
			}
		
			// set the crouched status
			_isCrouching = crouched;
			return true;
		}

		private void HandleAirMovement() 
		{
			// cache the input axes
			Vector3 inputAxes = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
		
			// convert to a vector representing movement directions
			Vector3 inputAsMovement = new Vector3(inputAxes.x, 0f, inputAxes.y);
		
			// multiply movement direction by speed  
			inputAsMovement.Scale(AirStrafeSpeed);

			// transform the movement vector into world space relative to the player
			Vector3 worldSpaceMovement = transform.TransformVector(inputAsMovement);
			_characterVelocity += worldSpaceMovement;
		}

		private void HandleGroundMovement()
		{
			// cache the input axes
			Vector3 inputAxes = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
		
			// convert to a vector representing movement directions
			Vector3 inputAsMovement = new Vector3(inputAxes.x, 0f, inputAxes.y);

			if (!_isSprinting)
			{
				// multiply movement direction by speed  
				inputAsMovement.Scale(WalkingSpeed);
			}
			else
			{
				// multiply movement direction by speed  
				inputAsMovement.Scale(SprintSpeed);
			}

			// transform the movement vector into world space relative to the player
			Vector3 worldSpaceMovement = transform.TransformVector(inputAsMovement);
			_characterVelocity = worldSpaceMovement;
		}

	
		private void HandleCameraMovement()
		{
			// cache the camera input axes
			Vector2 inputAxes = new Vector2(Input.GetAxisRaw("Horizontal_Camera"),Input.GetAxisRaw("Vertical_Camera"));
			//Debug.Log($"Hor:{Input.GetAxisRaw("Horizontal_Camera")} Ver:{Input.GetAxisRaw("Vertical_Camera")}");

			// scale the input by the camera speed
			inputAxes.Scale(CameraSpeed);
		
			// horizontal rotation - rotate the character
			transform.Rotate(Vector3.up, inputAxes.x,Space.Self);
		
			// vertical rotation - rotate the camera
			// add new input to the accumulated angle
			_cameraVerticalAngle += inputAxes.y;
		
			// clamp the vertical rotation to be between min and max angles
			_cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, MinCameraVerticalAngle, MaxCameraVerticalAngle);
		
			// apply to the camera
			_Camera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle,0f,0f);
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawSphere(_Camera.transform.position + _Camera.transform.forward,0.01f);
		}
	}
}