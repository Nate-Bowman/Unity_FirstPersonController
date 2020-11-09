namespace Loot.Inventory.UI
{
	using UnityEngine;
	using UnityEngine.UI;

	public class UIInventoryItem : MonoBehaviour
	{
		[SerializeField]
		public Image SpriteComponent;

		[SerializeField]
		public Text ValueText;

		public void SetData(Sprite sprite, int count)
		{
			// turn off the background sprite and text if we send a null sprite
			if (sprite == null)
			{
				if (SpriteComponent != null)
				{
					SpriteComponent.enabled = false;
				}

				if (ValueText != null)
				{
					ValueText.text = "";
				}

				return;
			}

			if (SpriteComponent != null)
			{
				SpriteComponent.enabled = true;
				SpriteComponent.sprite = sprite;
			}

			if (ValueText != null)
			{
				ValueText.text = count.ToString();
			}
		}
	}
}