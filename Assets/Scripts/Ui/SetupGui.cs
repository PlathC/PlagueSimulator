using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SetupGui : MonoBehaviour
{
#region UiObjects
    [SerializeField]
    private string m_simulationSceneName;
    [SerializeField]
    private Button m_launchSimulationScene;

    [SerializeField]
    private Slider m_populationDensitySlider;
    [SerializeField]
    private Text m_populationDensityValue;

    [SerializeField]
    private Slider m_infectivitySlider;
    [SerializeField]
    private Text m_infectivityValue;

    [SerializeField]
    private Slider m_stressLevelSlider;
    [SerializeField]
    private Text m_stressLevelValue;
    #endregion // UiObjects

    [SerializeField]
    private SimulationData m_simulationData;

    private AsyncOperation m_openSimulationScene;

    // Use this for initialization
    void Start()
    {
        m_launchSimulationScene.onClick.AddListener(LoadButton);
        m_populationDensitySlider.onValueChanged.AddListener(UpdatePopulationDensityValue);
        UpdatePopulationDensityValue(m_populationDensitySlider.value);

        m_infectivitySlider.onValueChanged.AddListener(UpdatInfectivityValue);
        UpdatInfectivityValue(m_infectivitySlider.value);

        m_stressLevelSlider.onValueChanged.AddListener(UpdateStressLevelValue);
        UpdateStressLevelValue(m_stressLevelSlider.value);
    }

    private void UpdatePopulationDensityValue(float value)
    {
        m_populationDensityValue.text = value.ToString();
    }

    private void UpdatInfectivityValue(float value)
    {
        m_infectivityValue.text = value.ToString();
    }

    private void UpdateStressLevelValue(float value)
    {
        m_stressLevelValue.text = value.ToString();
    }

    public void LoadButton()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        m_simulationData = ScriptableObject.CreateInstance<SimulationData>();
        m_simulationData.populationDensity = (uint) m_populationDensitySlider.value;
        m_simulationData.infectivity = m_infectivitySlider.value;
        m_simulationData.stressLevel = m_stressLevelSlider.value;

        yield return null;

        m_openSimulationScene = SceneManager.LoadSceneAsync(m_simulationSceneName);
        m_openSimulationScene.allowSceneActivation = false;

        while (!m_openSimulationScene.isDone)
        {
            //Output the current progress
            Debug.Log("Loading progress: " + (m_openSimulationScene.progress * 100) + "%");

            // Check if the load has finished
            if (m_openSimulationScene.progress >= 0.9f)
            {
                m_openSimulationScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
