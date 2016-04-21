using UnityEngine;
using System.Collections;
/// <summary>
/// Spawns units in waves, once the last unit of a wave is dead, a new wave is spawned.
/// </summary>
[AddComponentMenu("Spawner/Spawn Wave")]
public class SpawnWave : ISpawnType
{
	/// <summary>
	/// Used to determine if there is actual spawning to occur.
	/// </summary>
	protected bool waveSpawn = true;
	/// <summary>
	/// The time the last wave was spawned
	/// </summary>
	protected float lastWaveSpawnTime = 0.0f;
	/// <summary>
	/// The total number of waves to spawn.
	/// </summary>
	public int totalWavesToSpawn = 5;
	/// <summary>
	/// The number of waves that has spawned.
	/// </summary>
	protected int numberOfWavesSpawned = 0;
	/// <summary>
	/// Set to check the enemy count in DoSpawn
	/// </summary>
	protected bool checkEnemyCount = true;

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
					checkEnemyCount = true;

					if ((spawner.TotalUnitCount / (numberOfWavesSpawned + 1)) == unitCount)
					{
						waveSpawn = false;
					}
				}
				if (checkEnemyCount)
				{
					if (spawner.SpawnedUnitCount <= 0)
					{
						checkEnemyCount = false;
						waveSpawn = true;
						spawner.ResetSpawnedUnits ();
						numberOfWavesSpawned++;
						lastWaveSpawnTime = Time.time;
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
	/// How many waves are left.
	/// </summary>
	/// <value>The waves left.</value>
	public int WavesLeft
	{
		get
		{
			return (totalWavesToSpawn - numberOfWavesSpawned);
		}
	}
	/// <summary>
	/// The last time a wave was spawned.
	/// </summary>
	/// <value>The last wave time.</value>
	public float LastWaveTime
	{
		get
		{
			return lastWaveSpawnTime;
		}
	}

	public override void Reset ()
	{
		waveSpawn = false;
		checkEnemyCount = true;
		numberOfWavesSpawned = 0;
		lastWaveSpawnTime = Time.time;
	}
}