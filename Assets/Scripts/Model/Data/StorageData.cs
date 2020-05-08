using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StorageData
{
    public SicknessState State;
    public Vector3 Position;

    public StorageData(SicknessState state, Vector3 position)
    {
        State = state;
        Position = position;
    }
}
