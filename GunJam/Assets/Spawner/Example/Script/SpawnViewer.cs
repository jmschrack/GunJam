///////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Filename: SpawnViewer.cs
//  
// Author: Garth de Wet <garthofhearts@gmail.com>
// Website: http://corruptedsmilestudio.blogspot.com/
// Date Modified: 15 November 2013
//
// Copyright (c) 2012 Garth de Wet
// 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

/// <summary>
/// A simple script to show the current spawn state.
/// </summary>
[RequireComponent(typeof(Spawner))]
public class SpawnViewer : MonoBehaviour
{
	private Spawner spawn;

	void Start ()
	{
		spawn = gameObject.GetComponent<Spawner> ();
	}

	void OnGUI ()
	{
		GUILayout.Label ("Current unit level: " + spawn.spawnType.unitLevel.ToString ());
		GUILayout.Label ("Current mode: " + spawn.spawnType.GetType ().ToString ());
		if (spawn.TimeTillWave != 0f)
			GUILayout.Label ("Time till next wave: " + spawn.TimeTillWave.ToString ("F1"));
		if (spawn.WavesLeft != 0)
			GUILayout.Label ("Waves left: " + spawn.WavesLeft);
	}
}