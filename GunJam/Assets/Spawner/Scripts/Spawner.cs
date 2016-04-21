///////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Filename: Spawner.cs
//  
// Author: Garth de Wet <garthofhearts@gmail.com>
// Website: http://corruptedsmilestudio.blogspot.com/
// Date Modified: 15 November 2013
//
// Copyright (c) 2012 Garth de Wet
// 
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
using CorruptedSmileStudio.Spawner;
using UnityEngine;

/// <summary>
/// Spawns prefabs, either in waves, at once or continually till all enemies are spawned.
/// </summary>
/// <description>
/// Controls the spawning of selected perfabs, useful for making enemy spawn points.<br />
/// It supports a variety of spawn modes, which allows you to bend the system to fit your needs.<br />
/// This class is required for the system to work, you will need to place this class on a GameObject with
/// a tag of Spawner (However, this is changeable within the SpawnAI class).
/// </description>
[AddComponentMenu("Spawner/Spawner")]
public class Spawner : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The list of units to spawn.
    /// </summary>
    public SpawnableClass[] unitList = new SpawnableClass[4];
    /// <summary>
    /// The current number of spawned enemies.
    /// </summary>
    private int numberOfSpawnedUnits = 0;
    /// <summary>
    /// The total number of spawned enemies. Not just alive.
    /// </summary>
    private int totalSpawnedUnits = 0;
    /// <summary>
    /// The ID of the spawner.
    /// </summary>
    public int spawnID = 0;
    /// <summary>
    /// Determines whether the spawn should spawn.
    /// </summary>
    public bool spawn = true;
    /// <summary>
    /// The location of where to spawn units.
    /// </summary>
    public Transform[] spawnLocations;
    /// <summary>
    /// Whether to spawn randomly across the spawn locations or in the order that they are listed.
    /// </summary>
    public bool spawnRandomly = true;
    /// <summary>
    /// The current spawn location
    /// </summary>
    private int currentSpawnLocation = 0;
    /// <summary>
    /// The type of the spawn.
    /// </summary>
    public ISpawnType spawnType;
    #endregion

    void Awake()
    {
        if (spawnLocations.Length == 0)
        {
            spawnLocations = new Transform[1];
            spawnLocations[0] = (transform);
        }
    }

    void Start()
    {
        spawnType = GetComponent<ISpawnType>();
        if (spawnType == null)
        {
            Debug.LogError("No Spawn Mode attached to Spawner: " + spawnID);
            Disable();
            enabled = false;
            return;
        }
        for (int i = 0; i < unitList[spawnType.UnitLevel].units.Length; i++)
        {
            InstanceManager.ReadyPreSpawn(unitList[spawnType.UnitLevel].units[i].transform,
                                           (spawnType.unitCount / unitList[spawnType.UnitLevel].units.Length));
        }
        StartCoroutine(spawnType.DoSpawn(this));
    }
    /// <summary>
    /// Spawns a unit based on the unit level of the spawnType
    /// </summary>
    public void SpawnUnit()
    {
        int unitToSpawn = Random.Range(0, unitList[spawnType.UnitLevel].units.Length);
        if (unitList[spawnType.UnitLevel].units[unitToSpawn] != null)
        {
            int locationToSpawn = 0;
            if (spawnRandomly)
            {
                locationToSpawn = Random.Range(0, spawnLocations.Length);
            }
            else
            {
                if (++currentSpawnLocation == spawnLocations.Length)
                {
                    currentSpawnLocation = 0;
                }
                locationToSpawn = currentSpawnLocation;
            }
            if (spawnLocations[locationToSpawn] != null)
            {
                Transform unit = InstanceManager.Spawn(unitList[spawnType.UnitLevel].units[unitToSpawn].transform,
                                                   spawnLocations[locationToSpawn].position, spawnLocations[locationToSpawn].rotation);
                unit.GetComponent<SpawnAI>().SetOwner(this);
                // Increase the total number of enemies spawned and the number of spawned enemies
                numberOfSpawnedUnits++;
                totalSpawnedUnits++;
            }
            else
            {
                Debug.LogError("Spawn location: " + locationToSpawn + " on spawner: " + spawnID + " is null");
            }
        }
        else
        {
            Debug.LogError("Error trying to spawn unit of level " + spawnType.UnitLevel.ToString() + " on spawner " + spawnID + " - No unit set");
            if (unitList[spawnType.UnitLevel].units.Length == 1)
            {
                Disable();
            }
        }
    }
    /// <summary>
    /// Enable the spawner by ID
    /// </summary>
    /// <param name="sID">The spawner's ID</param>
    public void EnableSpawner(int sID)
    {
        if (spawnID == sID)
        {
            spawn = true;
            for (int i = 0; i < unitList[spawnType.UnitLevel].units.Length; i++)
            {
                InstanceManager.ReadyPreSpawn(unitList[spawnType.UnitLevel].units[i].transform,
                                               (spawnType.unitCount / unitList[spawnType.UnitLevel].units.Length));
            }
            StartCoroutine(spawnType.DoSpawn(this));
        }
    }
    /// <summary>
    /// Disable the spawner by ID
    /// </summary>
    /// <param name="sID">The spawner's ID</param>
    public void DisableSpawner(int sID)
    {
        if (spawnID == sID)
        {
            spawn = false;
            if (spawnType != null)
                spawnType.StopSpawn();
        }
    }

    /// <summary>
    /// Enables the spawner. Useful for triggers.
    /// </summary>
    public void Enable()
    {
        EnableSpawner(spawnID);
    }
    /// <summary>
    /// Disables the spawner. Useful for triggers.
    /// </summary>
    public void Disable()
    {
        DisableSpawner(spawnID);
    }
    /// <summary>
    /// Resets all the private variables to their original state.
    /// </summary>
    public void Reset()
    {
        spawnType.Reset();
        totalSpawnedUnits = 0;
    }
    /// <summary>
    /// Returns the number of waves left
    /// </summary>
    public int WavesLeft
    {
        get
        {
            if (typeof(SpawnWave).IsAssignableFrom(spawnType.GetType()))
            {
                return ((SpawnWave)spawnType).WavesLeft;
            }
            return 0;
        }
    }
    /// <summary>
    /// This removes an "unit" in order to allow waves and such that depend on the number of enemies decreasing
    /// to properly start a new spawn.
    /// </summary>
    public void RemoveUnit()
    {
        numberOfSpawnedUnits--;
    }
    /// <summary>
    /// Gets the spawned unit count.
    /// </summary>
    /// <value>The spawned unit count.</value>
    public int SpawnedUnitCount
    {
        get
        {
            return numberOfSpawnedUnits;
        }
    }
    /// <summary>
    /// Gets the total unit count.
    /// </summary>
    /// <value>The total unit count.</value>
    public int TotalUnitCount
    {
        get
        {
            return totalSpawnedUnits;
        }
    }
    /// <summary>
    /// Resets the spawned units.
    /// </summary>
    public void ResetSpawnedUnits()
    {
        numberOfSpawnedUnits = 0;
    }
    /// <summary>
    /// Gets the time till wave.
    /// </summary>
    /// <value>The time till wave.</value>
    public float TimeTillWave
    {
        get
        {
            if (typeof(SpawnTimedWave).IsAssignableFrom(spawnType.GetType()))
            {
                return ((SpawnTimedWave)spawnType).TimeTillWave;
            }
            return 0f;
        }
    }
}