using UnityEngine;

namespace Interaction {
	/// <summary>
	///  An interactive object that can be grabbed and dropped
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class GrabableObject : InteractiveObject
	{
		[SerializeField, Tooltip("The force at which the object is thrown upon release")]
		private float _ejectionForce = 2f;

		/// <summary>
		///  Which entity has picked up this object
		/// </summary>
		private Transform _grabbedBy;

		/// <summary>
		///  The Rigidbody belonging to this object
		/// </summary>
		private Rigidbody _rigidBody;

		/// <inheritdoc cref="InteractiveObject.DoAction" />
		public override void DoAction(Transform caller)
		{
			if (_grabbedBy == null)
			{
				_grabbedBy = caller;
				caller.SendMessage("CarryObject", transform);
			}
			else
			{
				caller.SendMessage("DropObject", transform);
				_grabbedBy = null;
				_rigidBody.AddForce(transform.forward * _ejectionForce);
			}
		}

		private void Start()
		{
			_rigidBody = GetComponent<Rigidbody>();
		}
	}
}