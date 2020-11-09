namespace Loot.Items
{
	using System;
	using UnityEngine;

	[Serializable]
	public class LootData
	{
		[SerializeField]
		public bool LootCanStack = true;

		[SerializeField]
		public int LootCount = 1;

		[SerializeField]
		public int LootID;

		[SerializeField]
		public int LootMaxCount = 20;

		[SerializeField]
		public string LootName;

		[SerializeField]
		public Sprite LootSprite;
	}
}