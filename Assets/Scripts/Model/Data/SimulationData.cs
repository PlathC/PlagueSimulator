using UnityEngine;

namespace Model.Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/SimulationData")]
    public class SimulationData : ScriptableObject
    {
        public uint populationDensity = 1000;
        public float infectivity = 0.2f;
        public float stressLevel = 0.2f;
    }
}