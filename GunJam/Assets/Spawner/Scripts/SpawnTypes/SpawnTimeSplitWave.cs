using UnityEngine;
using System.Collections;
/// <summary>
/// Spawn time split wave. Spawns a new wave after the previous wave has been destroyed and a specified amount of time
/// has passed since the last wave was killed.
/// </summary>
[AddComponentMenu("Spawner/Spawn Time Split Wave")]
public class SpawnTimeSplitWave : SpawnTimedWave
{
	Spawner spawn;
	public override IEnumerator DoSpawn (Spawner spawner)
	{
		spawn = spawner;
		while (spawner.spawn)
		{
			if (WavesLeft > 0)
			{
				if (waveSpawn)
				{
					yield return new WaitForSeconds (timeBetweenSpawns);
					spawner.SpawnUnit ();
					if ((spawner.TotalUnitCount / (numberOfWavesSpawned + 1)) == unitCount)
					{
						waveSpawn = false;
						checkEnemyCount = true;
					}
				}
				else
				{
					if (checkEnemyCount)
					{
						if (spawner.SpawnedUnitCount <= 0)
						{
							numberOfWavesSpawned++;
							checkEnemyCount = false;
							spawner.ResetSpawnedUnits ();
							lastWaveSpawnTime = Time.time;
							yield return new WaitForSeconds (waveTimer);
							waveSpawn = true;
						}
					}
				}
			}
			else
			{
				spawner.Disable ();
			}
			yield return new WaitForEndOfFrame ();
		}
		yield return null;
	}

	/// <summary>
	/// The time till the next wave
	/// </summary>
	public override float TimeTillWave
	{
		get
		{
			if (WavesLeft <= 0)
			{
				return 0f;
			}

			if (waveSpawn || spawn.SpawnedUnitCount > 0)
			{
				return 0f;
			}
			
			float time = (lastWaveSpawnTime + waveTimer) - Time.time;
			return (time >= 0f) ? time : 0f;
		}
	}
}