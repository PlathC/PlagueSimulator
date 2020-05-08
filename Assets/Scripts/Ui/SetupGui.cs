using System.Collections;
using Model.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui
{
    public class SetupGui : MonoBehaviour
    {
        
        #region UiObjects
        [SerializeField]
        private string simulationSceneName;
        [SerializeField]
        private Button launchSimulationScene;

        [SerializeField]
        private Slider populationDensitySlider;
        [SerializeField]
        private Text populationDensityValue;

        [SerializeField]
        private Slider infectivitySlider;
        [SerializeField]
        private Text infectivityValue;

        [SerializeField]
        private Slider stressLevelSlider;
        [SerializeField]
        private Text stressLevelValue;
        #endregion // UiObjects
        
        private AsyncOperation m_openSimulationScene;
        private SimulationData simulationData;

        // Use this for initialization
        private void Start()
        {
            launchSimulationScene.onClick.AddListener(() => LoadButton());
            populationDensitySlider.onValueChanged.AddListener(f => UpdatePopulationDensityValue(f));
            UpdatePopulationDensityValue(populationDensitySlider.value);

            infectivitySlider.onValueChanged.AddListener(f => UpdatInfectivityValue(f));
            UpdatInfectivityValue(infectivitySlider.value);

            stressLevelSlider.onValueChanged.AddListener(f => UpdateStressLevelValue(f));
            UpdateStressLevelValue(stressLevelSlider.value);
        }

        private void UpdatePopulationDensityValue(float value)
        {
            populationDensityValue.text = value.ToString();
        }

        private void UpdatInfectivityValue(float value)
        {
            infectivityValue.text = value.ToString();
        }

        private void UpdateStressLevelValue(float value)
        {
            stressLevelValue.text = value.ToString();
        }

        private void LoadButton()
        {
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            simulationData = ScriptableObject.CreateInstance<SimulationData>();
            simulationData.populationDensity = (uint) populationDensitySlider.value;
            simulationData.infectivity = infectivitySlider.value;
            simulationData.stressLevel = stressLevelSlider.value;

            yield return null;

            m_openSimulationScene = SceneManager.LoadSceneAsync(simulationSceneName);
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
}
