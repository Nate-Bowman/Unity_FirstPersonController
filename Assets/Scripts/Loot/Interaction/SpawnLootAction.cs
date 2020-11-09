namespace Loot.Interaction
{
	using System.Collections;
	using Items;
	using UnityEngine;

	public class SpawnLootAction : MonoBehaviour
	{
		public int AmountOfLootToSpawn = 6;

		[SerializeField]
		private GameObject lootPrefab;

		[SerializeField]
		private Vector3 spawnOffset = Vector3.zero;

		public void SpawnLoot()
		{
			StartCoroutine(SpawnLootItems());
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			var position = transform.position;
			position += transform.rotation * spawnOffset;
			Gizmos.DrawWireSphere(position, 0.1f);
		}
#endif

		private void SpawnLootItem()
		{
			var position = transform.position;
			position += transform.rotation * spawnOffset;
			var go = Instantiate(lootPrefab);
			var lootPickup = go.GetComponent<LootPickup>();
			lootPickup.Spawn(position);
		}

		private IEnumerator SpawnLootItems()
		{
			for (var i = 0; i < AmountOfLootToSpawn; i++)
			{
				SpawnLootItem();
				yield return new WaitForSeconds(Random.Range(.3f, 1.2f));
			}
		}
	}
}