using UnityEngine;
using System.Collections;
/// <summary>
/// Spawn Normal. Spawns continuously, attempting to keep the spawned units at unitCount
/// </summary>
[AddComponentMenu("Spawner/Spawn Normal")]
public class SpawnNormal : ISpawnType
{
	public override IEnumerator DoSpawn (Spawner spawner)
	{
		while (spawner.spawn)
		{
			if (spawner.SpawnedUnitCount < unitCount)
			{
				yield return new WaitForSeconds (timeBetweenSpawns);
				spawner.SpawnUnit ();
			}
			yield return new WaitForEndOfFrame ();
		}
		yield return null;
	}

	public override void Reset ()
	{
	}
}