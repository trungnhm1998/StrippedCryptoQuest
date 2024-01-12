using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicStoneDatabase : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public string stone_id;
		public string name_key;
	}
}

