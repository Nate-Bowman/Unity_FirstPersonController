namespace Loot.Items
{
	using global::Interaction;
	using UnityEngine;

	[RequireComponent(typeof(LootItem))]
	public class LootPickup : InteractiveObject
	{
		[SerializeField]
		private float ejectionForce = 10f;

		[SerializeField]
		private LootItem lootItem;

		/// <summary>
		///  Activate the action of this InteractiveObject
		/// </summary>
		/// <param name="caller">The Transform of the object that initiated DoAction</param>
		public override void DoAction(Transform caller)
		{
			caller.SendMessage("PickupLoot", lootItem);
			Destroy(gameObject);
		}

		public void Spawn(Vector3 position)
		{
			transform.position = position;

			// get a random direction
			var spawnDirection = Random.onUnitSphere;

			// always eject upwards
			spawnDirection.y = 1;

			spawnDirection *= ejectionForce;

			var rb = gameObject.GetComponent<Rigidbody>();
			if (rb != null)
			{
				// use ForceMode.VelocityChange to ignore mass;
				rb.AddForce(spawnDirection, ForceMode.VelocityChange);
			}
		}
	}
}