using Model.Agents;
using UnityEngine;

namespace Model.Data
{
    public struct StorageData
    {
        public CitizenBody.PositionState positionState;
        public CitizenBody.SicknessState oldSicknessState;
        public CitizenBody.SicknessState sicknessState;
        public Vector3 position;
        public CitizenBody.CauseOfDeath causeOfDeath;
        public float time;
        
        public StorageData(float nTime, CitizenBody.PositionState nPositionState, CitizenBody.SicknessState oSicknessState, CitizenBody.SicknessState nSicknessState, Vector3 nPosition, CitizenBody.CauseOfDeath nCauseOfDeath)
        {
            time = nTime;
            positionState = nPositionState;
            oldSicknessState = oSicknessState;
            sicknessState = nSicknessState;
            position = nPosition;
            causeOfDeath = nCauseOfDeath;
        }

        public override string ToString()
        {
            return time + "," + positionState + "," + sicknessState + "," 
                   + position.x + "," + position.y + "," + position.z + "," + causeOfDeath;
        }
    }
}
