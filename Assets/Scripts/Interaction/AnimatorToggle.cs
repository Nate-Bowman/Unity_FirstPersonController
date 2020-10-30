namespace Interaction
{
	using UnityEngine;

	public class AnimatorToggle : InteractiveObject
	{
		[SerializeField]
		private Animator _Animator;

		[SerializeField]
		private string AnimationTriggerName = "Toggle";

		/// <inheritdoc cref="InteractiveObject.DoAction" />
		public override void DoAction(Transform caller)
		{
			_Animator.SetTrigger(AnimationTriggerName);
		}

		private void Start()
		{
			_Animator = GetComponent<Animator>();
		}
	}
}