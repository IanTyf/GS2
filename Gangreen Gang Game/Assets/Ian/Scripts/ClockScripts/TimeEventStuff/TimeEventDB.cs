using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEventDB : ScriptableObject
{
    [Header("***Initial/Static Events***")]
    [SerializeField]
    public InitialTimeEventSetup[] initialEvents;

    [Header("***Dynamic Events***")]
    [SerializeField]
    public TimeEventSetup[] dynamicEvents;
}
