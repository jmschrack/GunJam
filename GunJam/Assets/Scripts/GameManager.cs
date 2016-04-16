using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{

	private int zombiesKilled = 0;
	private List<Enemy> currentZombies;

	public List<Transform> ZombieSpawns;
	public List<PracticeTarget> PracticeTargets;

	public Transform ZombiePrefab;
	public Transform Player;
	public bool TargetsDone = false;
	public bool DoneSpawning = false;
	public int MaxSpawnedZombies = 5;
	public int TotalZombies = 20;

	// Use this for initialization
	void Start()
	{
		if (TargetsDone)
		{
			StartCoroutine(SpawnZombies());
		}
	}

	// Update is called once per frame
	void Update()
	{
		bool allDone = true;

		if (!TargetsDone)
		{
			foreach (PracticeTarget target in PracticeTargets)
			{
				if (target != null && !target.Shot)
				{
					allDone = false;
					break;
				}
			}

			if (allDone)
			{
				TargetsDone = true;
				StartCoroutine(SpawnZombies());
			}
		}

		if (DoneSpawning)
		{
			Application.LoadLevel(0);
		}
	}

	private IEnumerator SpawnZombies()
	{
		currentZombies = new List<Enemy>();

		int spawnPointIndex = 0;
		while (!DoneSpawning)
		{
			zombiesKilled += currentZombies.RemoveAll(e => e == null);

			if (zombiesKilled >= TotalZombies)
			{
				DoneSpawning = true;
			}

			if (currentZombies.Count < MaxSpawnedZombies && zombiesKilled + currentZombies.Count < TotalZombies)
			{
				Transform spawn = ZombieSpawns[spawnPointIndex];
				Transform zombie = Instantiate(ZombiePrefab, spawn.position, spawn.rotation) as Transform;

				Enemy enemyComp = zombie.GetComponent<Enemy>();

				enemyComp.target = Player;
				currentZombies.Add(enemyComp);
				spawnPointIndex = (spawnPointIndex + 1) % ZombieSpawns.Count;
			}

			yield return new WaitForSeconds(2f);
		}

		yield return null;
	}
}
