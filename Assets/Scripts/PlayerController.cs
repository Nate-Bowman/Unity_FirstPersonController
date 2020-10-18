using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	private const float Gravity = 9.8f;
	private CharacterController _characterController;
	private Vector3 _characterVelocity;
	private bool _isSprinting;
	
	[SerializeField][Tooltip("The force applied to the character when jumping")]
	private float JumpForce = 10f;

	[SerializeField][Tooltip("The camera for the first person controller")]
	public Camera _Camera;

	[SerializeField]
	[Tooltip("The fall speed modifier, increases gravity")]
	private float GravityMultiplier = 2f;
	
	[SerializeField][Tooltip("Walking speed of the character")]
	private Vector3 WalkingSpeed = new Vector3(2f,0f,4f); 
	
	[SerializeField][Tooltip("Minimum angle the camera can look up/down")]
	private float MinCameraVerticalAngle = -70;
	[SerializeField][Tooltip("Maximum angle the camera can look up/down")]
	private float MaxCameraVerticalAngle = 55;
	
	[SerializeField][Tooltip("The speed at which the camera moves")]
	private Vector2 CameraSpeed = new Vector2(.5f,0.1f);
	
	// Start is called before the first frame update
	private void Start()
	{
		// cache the character controller component
		_characterController = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	private void Update()
	{

		HandleCameraMovement();
		
		// Was i grounded last frame?
		var wasGrounded = _characterController.isGrounded;

		//TODO: Crouch

		if (_characterController.isGrounded)
		{
			//ground movement controls
			HandleGroundMovement();
					
			if (Input.GetButtonDown("Jump"))
			{
				// remove vertical velocity
				_characterVelocity.y = 0;

				// add jump velocity
				_characterVelocity += Vector3.up * JumpForce;
			}

		}
		else
		{
			// add gravitational acceleration this frame
			_characterVelocity += Vector3.down * (Gravity * GravityMultiplier * Time.deltaTime);

			//TODO: Add air strafe
		}

		// Apply calculated movement
		_characterController.Move(_characterVelocity * Time.deltaTime);
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
			// TODO: Add sprint speed
		}

		// transform the movement vector into world space relative to the player
		Vector3 worldSpaceMovement = transform.TransformVector(inputAsMovement);
		_characterVelocity = worldSpaceMovement;
	}

	private float _cameraVerticalAngle;
	
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
}