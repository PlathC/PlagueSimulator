using UnityEngine;

namespace Model.Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/SimulationData")]
    public class SimulationData : ScriptableObject
    {
        public uint populationDensity = 1000;
        public float infectivity = 0.2f;
        public float launchSickNumber = 0.2f;
        public float launchImmunedNumber = 0f;
        public uint diseaseDuration = 100;
        public float deathStatistic = 0.5f;
        public float diseaseTransmissionDistance = 2;
    }
}