using UnityEngine;

/// <summary>
///  Abstract base class for Interactive Objects
/// </summary>
public abstract class InteractiveObject : MonoBehaviour
{
	[SerializeField, Tooltip("Desctiption of the Actions available for this object")]
	public string actionDescription = "";

	[SerializeField, Tooltip("Description of this object")]
	private string description = "";

	/// <summary>
	///  Display the details of this object on the HUD
	/// </summary>
	public virtual void DisplayDetailUI()
	{
		Debug.Log($"Description:\n {description}\n{actionDescription}");
		HUD.Instance.SetOutputText($"{description}\n{actionDescription}");
	}

	/// <summary>
	///  Activate the action of this InteractiveObject
	/// </summary>
	/// <param name="caller">The Transform of the object that initiated DoAction</param>
	public virtual void DoAction(Transform caller)
	{
		Debug.Log("Activate Action");
	}
}