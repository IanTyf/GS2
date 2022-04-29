using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TimeEventDBAsset : MonoBehaviour
{
    [MenuItem("Assets/Create/ScriptableObjects/TimeEventDB")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<TimeEventDB>();
    }
}
