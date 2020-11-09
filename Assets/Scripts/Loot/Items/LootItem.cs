namespace Loot.Items
{
	using UnityEngine;

	public class LootItem : MonoBehaviour
	{
		[SerializeField]
		private LootData LootData = new LootData();

		public LootData Data { get => LootData; private set => LootData = value; }
	}
}