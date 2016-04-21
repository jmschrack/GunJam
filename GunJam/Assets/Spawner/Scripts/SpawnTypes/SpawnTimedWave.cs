using UnityEngine;
using System.Collections;
/// <summary>
/// Spawn timed wave. Spawns waves after a certain amount of time has passed regardless of
/// if the wave has been destroyed or not.
/// </summary>
[AddComponentMenu("Spawner/Spawn Timed Wave")]
public class SpawnTimedWave : SpawnWave
{
	/// <summary>
	/// The time between each wave when spawn type is set to wave.
	/// </summary>
	public float waveTimer = 30.0f;

	public override IEnumerator DoSpawn (Spawner spawner)
	{
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
					}
				}
				else
				{
					waveSpawn = true;
					numberOfWavesSpawned++;
					spawner.ResetSpawnedUnits ();
					lastWaveSpawnTime = Time.time;
					yield return new WaitForSeconds (waveTimer);
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
	public virtual float TimeTillWave
	{
		get
		{
			if (numberOfWavesSpawned >= totalWavesToSpawn)
			{
				return 0f;
			}
			
			float time = (lastWaveSpawnTime + waveTimer) - Time.time;
			return (time >= 0f) ? time : 0f;
		}
	}
}