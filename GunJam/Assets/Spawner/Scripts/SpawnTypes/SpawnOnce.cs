using UnityEngine;
using System.Collections;
/// <summary>
/// Spawn Once. Spawns upto the max number of units then stops spawning.
/// </summary>
[AddComponentMenu("Spawner/Spawn Once")]
public class SpawnOnce : ISpawnType
{
	public override IEnumerator DoSpawn (Spawner spawner)
	{
		while (spawner.spawn)
		{
			if (spawner.TotalUnitCount >= unitCount)
			{
				spawner.Disable ();
			}
			else
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