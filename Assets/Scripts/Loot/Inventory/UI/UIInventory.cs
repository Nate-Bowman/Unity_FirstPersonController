namespace Loot.Inventory.UI {
	using UnityEngine;

	public class UIInventory : MonoBehaviour
	{
		public GameObject Container;
		public GameObject inventoryItemPrefab;
		public GameObject ItemContainer;

		private bool visibleState;

		// Start is called before the first frame update
		private void Start()
		{
			Container.SetActive(false);
		}

		// Update is called once per frame
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				Container.SetActive(!visibleState);
				visibleState = !visibleState;
			}

			// update while visible
			// TODO: Throttle updates or only update when inventory changes
			if (visibleState)
			{
				UpdateItems();
			}
		}

		private void UpdateItems()
		{
			//grab the players inventory
			// TODO: Cache inventory
			var inventory = FindObjectOfType<PlayerInventory>();

			// remove any excess inventory ui items
			while (ItemContainer.transform.childCount > inventory.InventorySize)
			{
				Destroy(
				        ItemContainer.transform.GetChild(0)
				                     .gameObject
				       );
			}

			// add and missing ui inventory items
			while (ItemContainer.transform.childCount < inventory.InventorySize)
			{
				Instantiate(inventoryItemPrefab, ItemContainer.transform);
			}

			// populate the items with the inventory, clear the ones that are used
			var items = ItemContainer.GetComponentsInChildren<UIInventoryItem>();
			for (var i = 0; i < items.Length; i++)
			{
				if (inventory.LootDataList.Count > i)
				{
					items[i]
						.SetData(
						         inventory.LootDataList[i]
						                  .LootSprite, inventory.LootDataList[i]
						                                        .LootCount
						        );
				}
				else
				{
					items[i]
						.SetData(null, 0);
				}
			}
		}
	}
}