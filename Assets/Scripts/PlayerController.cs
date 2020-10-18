using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	private const float Gravity = 9.8f;
	private CharacterController _characterController;
	private Vector3 _characterVelocity;

	[SerializeField][Tooltip("The camera for the first person controller")]
	public Camera _Camera;

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

		if (_characterController.isGrounded)
		{
			//ground movement controls
			HandleGroundMovement();
		}
		else
		{
			// add gravitational acceleration this frame
			_characterVelocity += Vector3.down * (Gravity * Time.deltaTime);

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
		
		// transform the movement vector into world space relative to the player
		Vector3 worldSpaceMovement = transform.TransformVector(inputAsMovement);
		_characterVelocity = worldSpaceMovement;
	}

	private float _cameraVerticalAngle;
	
	private void HandleCameraMovement()
	{
		// cache the camera input axes
		Vector3 inputAxes = new Vector2(Input.GetAxisRaw("Horizontal_Camera"),Input.GetAxisRaw("Vertical_Camera"));
		//Debug.Log($"Hor:{Input.GetAxisRaw("Horizontal_Camera")} Ver:{Input.GetAxisRaw("Vertical_Camera")}");
		
		// horizontal rotation - rotate the character
		transform.Rotate(Vector3.up, inputAxes.x,Space.Self);
		
		// vertical rotation - rotate the camera
		
		// add new input to the accumulated angle
		_cameraVerticalAngle += inputAxes.y;
		
		// apply to the camera
		_Camera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle,0f,0f);
	}
}