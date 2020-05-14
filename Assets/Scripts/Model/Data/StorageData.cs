using Model.Agents;
using UnityEngine;

namespace Model.Data
{
    public struct StorageData
    {
        public CitizenBody.PositionState positionState;
        public CitizenBody.SicknessState sicknessState;
        public Vector3 position;

        public StorageData(CitizenBody.PositionState nPositionState, CitizenBody.SicknessState nSicknessState, Vector3 nPosition)
        {
            positionState = nPositionState;
            sicknessState = nSicknessState;
            position = nPosition;
        }
    }
}
