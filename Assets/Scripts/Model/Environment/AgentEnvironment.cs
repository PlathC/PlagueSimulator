using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AgentEnvironment : MonoBehaviour
{
    private List<CitizenBody> m_citizenList = new List<CitizenBody>();

    private int sickNumber = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NotifyAgentModification(StorageData old)
    {
        if (old.State == SicknessState.Infected)
            sickNumber++;

        Debug.Log(sickNumber);
    }

    public bool GetVirusContagiousity()
    {
        return true;
    }

    public void UpdateAgentList()
    {
        m_citizenList.Clear();

        GameObject[] agents = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject agent in agents)
        {
            m_citizenList.Add(agent.GetComponent<CitizenBody>());
            if (agent.GetComponent<CitizenBody>().State == SicknessState.Infected)
                sickNumber++;
        }
    }
}
