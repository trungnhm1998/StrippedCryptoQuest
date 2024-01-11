using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;


public class EquipmentIdToName : ScriptableObject
{
    [FormerlySerializedAs("param")]
    public List<Param> Params = new List<Param>();

    [System.SerializableAttribute]
    public class Param
    {
        public string equipment_id;
        public string localize_key;
    }
}