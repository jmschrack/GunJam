///////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Filename: SpawnEnums.cs
//  
// Author: Garth de Wet <garthofhearts@gmail.com>
// Website: http://corruptedsmilestudio.blogspot.com/
// Date Modified: 22 Nov 2012
//
// Copyright (c) 2012 Garth de Wet
// 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace CorruptedSmileStudio.Spawner
{
	/// <summary>
	/// The unit levels allowed.
	/// </summary>
	public enum UnitLevels
	{
		/// <summary>
		/// Easy unit
		/// </summary>
		Easy = 0,
		/// <summary>
		/// Medium unit
		/// </summary>
		Medium,
		/// <summary>
		/// Hard unit
		/// </summary>
		Hard,
		/// <summary>
		/// Boss unit
		/// </summary>
		Boss
	}
	/// <summary>
	/// Represents a spawnable unit class
	/// </summary>
	[System.Serializable]
	public class SpawnableClass
	{
		/// <summary>
		/// The level that the spawn able unit is.
		/// </summary>
		public UnitLevels level = UnitLevels.Easy;
		/// <summary>
		/// The units associated with that level.
		/// </summary>
		public UnityEngine.GameObject[] units = new UnityEngine.GameObject[0];
	}
}