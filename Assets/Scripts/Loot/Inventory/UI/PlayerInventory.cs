namespace Loot.Inventory.UI
{
	using System.Collections.Generic;
	using Items;
	using UnityEngine;

	public class PlayerInventory : MonoBehaviour
	{
		[SerializeField]
		public int InventorySize = 30;

		[SerializeField]
		public List<LootData> LootDataList;

		public void PickupLoot(LootItem lootItem)
		{
			var data = lootItem.Data;

			// keep tabs on if we add the item
			var isAdded = false;

			// if the loot is stackable, then check to see if we have a stack
			if (data.LootCanStack)
			{
				foreach (var storedItem in LootDataList)
				{
					// if we already have one add to the stack
					if (storedItem.LootID == data.LootID)
					{
						storedItem.LootCount += data.LootCount;

						isAdded = true;

						// exit the loop
						break;
					}
				}

				if (!isAdded && (LootDataList.Count < InventorySize))
				{
					LootDataList.Add(data);
					isAdded = true;
				}
			}
			else
			{
				if (LootDataList.Count < InventorySize)
				{
					LootDataList.Add(data);
					isAdded = true;
				}
			}

			// TODO: Send a message back if we successfully loot the item so it can dispose of itself
		}
	}
}