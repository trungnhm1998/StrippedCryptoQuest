using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentIdToName : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public string equipment_id;
		public string localize_key;
	}
}