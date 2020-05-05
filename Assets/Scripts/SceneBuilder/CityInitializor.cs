using UnityEngine;
using System.Collections;

public class CityInitializor : MonoBehaviour
{
    [SerializeField]
    private SimulationData m_simulationData = null;

    [SerializeField]
    private Environment m_environment = null;

#region PrefabsFields

    [SerializeField]
    private GameObject m_housePrefab = null;

    [SerializeField]
    private GameObject m_citizenPrefab = null;

#endregion //PrefabsFields

    void Start()
    {
        
        m_simulationData = ScriptableObject.FindObjectOfType<SimulationData>();
        
        var map = GameObject.FindGameObjectWithTag("Map");
        
        uint height = 0, width = 0;
        
        if (map.GetComponent<Renderer>() != null)
        {
            var size = map.GetComponent<Renderer>().bounds.size;
            width = (uint)size.x;
            height = (uint)size.z;
        }

        for (uint i = 0; i < m_simulationData.populationDensity; i++)
        {

            // Instantiate House prefab
            var position = new Vector3(map.transform.position.x + Random.Range(0, width) - width / 2,
                                       map.transform.position.y + 0,
                                       map.transform.position.z + Random.Range(0, height) - height / 2);
            var house = Instantiate(m_housePrefab,
                                    position, 
                                    Quaternion.identity);
            house.transform.parent = map.transform;

            var citizen = Instantiate(m_citizenPrefab,
                                      position,
                                      Quaternion.identity);
            citizen.transform.parent = map.transform;
        }

        m_environment.UpdateAgentList();
    }

    void Update()
    {

    }
}
