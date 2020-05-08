using Model.Data;
using Model.Environment;
using UnityEngine;
using UnityEngine.Serialization;

namespace SceneBuilder
{
    public class CityInitializor : MonoBehaviour
    {
        [SerializeField]
        private SimulationData simulationData = null;

        [SerializeField]
        private AgentEnvironment environment = null;

        #region PrefabsFields

        [SerializeField]
        private GameObject housePrefab = null;

        [SerializeField]
        private GameObject citizenPrefab = null;

        #endregion //PrefabsFields

        private void Start()
        {
        
            simulationData = ScriptableObject.FindObjectOfType<SimulationData>();
        
            var map = GameObject.FindGameObjectWithTag("Map");
        
            uint height = 0, width = 0;
        
            if (map.GetComponent<Renderer>() != null)
            {
                var size = map.GetComponent<Renderer>().bounds.size;
                width = (uint)size.x;
                height = (uint)size.z;
            }

            for (uint i = 0; i < simulationData.populationDensity; i++)
            {

                // Instantiate House prefab
                var position = new Vector3(map.transform.position.x + Random.Range(0, width) - width / 2,
                    map.transform.position.y + 0,
                    map.transform.position.z + Random.Range(0, height) - height / 2);
                var house = Instantiate(housePrefab,
                    position, 
                    Quaternion.identity);
                house.transform.parent = map.transform;

                var citizen = Instantiate(citizenPrefab,
                    position,
                    Quaternion.identity);
                citizen.transform.parent = map.transform;
            }

            environment.UpdateAgentList();
        }

        void Update()
        {

        }
    }
}
