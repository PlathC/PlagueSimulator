using Model.Agents;
using UnityEngine;

namespace Model.Data
{
    public struct StorageData
    {
        public CitizenBody.SicknessState State;
        public Vector3 Position;

        public StorageData(CitizenBody.SicknessState state, Vector3 position)
        {
            State = state;
            Position = position;
        }
    }
}
