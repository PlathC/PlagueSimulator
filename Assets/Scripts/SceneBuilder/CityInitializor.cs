using Model.Agents;
using Model.Data;
using Model.Environment;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SceneBuilder
{
    public class CityInitializor : MonoBehaviour
    {
        #region PrefabsFields

        [SerializeField]
        private GameObject housePrefab = null;

        [SerializeField]
        private GameObject citizenPrefab = null;

        [SerializeField] 
        private GameObject mayorPrefab = null;

        #endregion //PrefabsFields

        private AgentEnvironment m_environment = null;
        private SimulationData m_simulationData = null;

        private void Start()
        {            
            var env = GameObject.FindGameObjectWithTag("AgentEnvironment");
            if (!env) return;
                
            var agentEnvironment = env.GetComponent<AgentEnvironment>();
            if (agentEnvironment)
                m_environment = agentEnvironment;

            m_simulationData = ScriptableObject.FindObjectOfType<SimulationData>();
            
            var map = GameObject.FindGameObjectWithTag("Map");
            uint height = 0, width = 0;
        
            if (map.GetComponent<Renderer>() != null)
            {
                var size = map.GetComponent<Renderer>().bounds.size;
                width = (uint)size.x;
                height = (uint)size.z;
            }
            var mapPosition = map.transform.position;
            m_environment.Coordinates = new AgentEnvironment.MapCoordinates(mapPosition, width, height);
            
            // Instantiate House prefab
            for (uint i = 0; i < m_simulationData.populationDensity; i++)
            {
                var position = new Vector3(mapPosition.x + Random.Range(-(float)width / 2, (float)width / 2),
                    mapPosition.y,
                    mapPosition.z + Random.Range(-(float)height / 2, (float)height / 2));
                var house = Instantiate(housePrefab,
                    position, 
                    Quaternion.identity);
                house.transform.parent = map.transform;

                var citizen = Instantiate(citizenPrefab,
                    new Vector3(position.x,
                                position.y + 0.5f,
                                position.z),
                    Quaternion.identity);
                citizen.transform.parent = map.transform;
                if (citizen.TryGetComponent<CitizenBody>(out var citizenBody))
                {
                    citizenBody.InitProximityColliderSize(new Vector3(
                        m_simulationData.diseaseTransmissionDistance, 
                        1, 
                        m_simulationData.diseaseTransmissionDistance));
                }
                    
            }

            m_environment.UpdateAgentList();

            var mayor = Instantiate(mayorPrefab);
        }
    }
}
