using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	private const float Gravity = 9.8f;
	private CharacterController _characterController;
	private Vector3 _characterVelocity;

	// Start is called before the first frame update
	private void Start()
	{
		// cache the character controller component
		_characterController = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	private void Update()
	{
		// Was i grounded last frame?
		var wasGrounded = _characterController.isGrounded;

		if (_characterController.isGrounded)
		{
			_characterVelocity = Vector3.zero;

			//TODO: Add movement controls
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
}