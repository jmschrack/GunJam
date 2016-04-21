using UnityEngine;
using System.Collections;
/// <summary>
/// Represents the abstract spawn type. Concrete spawn types can use this as a base from which to extend.
/// </summary>
public abstract class ISpawnType : MonoBehaviour
{
	/// <summary>
	/// The unit level to spawn
	/// </summary>
	public CorruptedSmileStudio.Spawner.UnitLevels unitLevel;
	/// <summary>
	/// The time between each unit spawn
	/// </summary>
	public float timeBetweenSpawns = 0.5f;
	/// <summary>
	/// The number of units to spawn
	/// </summary>
	public int unitCount = 5;
	/// <summary>
	/// Performs the spawn checking
	/// </summary>
	/// <returns></returns>
	/// <param name="spawner">The spawner that is owns this spawn type</param>
	public abstract IEnumerator DoSpawn (Spawner spawner);
	/// <summary>
	/// Stops the spawn.
	/// </summary>
	public void StopSpawn ()
	{
		StopCoroutine ("DoSpawn");
	}
	/// <summary>
	/// Reset this instance.
	/// </summary>
	public abstract void Reset ();
	/// <summary>
	/// Gets the unit level as an int
	/// </summary>
	/// <value>The unit level.</value>
	public int UnitLevel
	{
		get
		{
			return (int)unitLevel;
		}
	}
}