// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-08-20       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    |_ | _|			                *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GEngine.MapEditor
{
	[Serializable]
	public class MapObjectsDesc
	{
		public MapObjects[] mapObjects;
	}
	
	[Serializable]
	public class MapObjects
	{
		public string tabname;
		public int count;
		public MapObject[] objects; 
	}
	
	[Serializable]
	public class MapObject
	{
		public string name;
		public string guid;
		public Vector3 T;
		public Vector3 R;
		public Vector3 S;
	}
}