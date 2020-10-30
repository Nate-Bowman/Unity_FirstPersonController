using UnityEngine;

namespace Interaction {
	public class AnimatorToggle : InteractiveObject
	{
		[SerializeField]
		private Animator _Animator;

		[SerializeField]
		private string AnimationTriggerName = "Toggle";
	
		private void Start()
		{
			_Animator = GetComponent<Animator>();
		}

		/// <inheritdoc cref="InteractiveObject.DoAction" />
		public override void DoAction(Transform caller)
		{
			_Animator.SetTrigger(AnimationTriggerName);
		}
	}
}