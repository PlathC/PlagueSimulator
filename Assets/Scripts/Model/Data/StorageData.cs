using Model.Agents;
using UnityEngine;

namespace Model.Data
{
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
}
