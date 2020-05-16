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
        
        [SerializeField]
        private Slider launchSickNumberSlider;
        [SerializeField]
        private Text launchSickNumberValue;
        
        [SerializeField]
        private Slider launchImmunedNumberSlider;
        [SerializeField]
        private Text launchImmunedNumberValue;

        [SerializeField]
        private Slider diseaseDurationSlider;
        [SerializeField]
        private Text diseaseDurationValue;
        
        [SerializeField]
        private Slider deathStatisticSlider;
        [SerializeField]
        private Text deathStatisticValue;

        #endregion // UiObjects
        
        private AsyncOperation m_openSimulationScene;
        private SimulationData simulationData;

        // Use this for initialization
        private void Start()
        {
            launchSimulationScene.onClick.AddListener(LoadButton);
            populationDensitySlider.onValueChanged.AddListener(UpdatePopulationDensityValue);
            UpdatePopulationDensityValue(populationDensitySlider.value);

            infectivitySlider.onValueChanged.AddListener(UpdatInfectivityValue);
            UpdatInfectivityValue(infectivitySlider.value);

            stressLevelSlider.onValueChanged.AddListener(UpdateStressLevelValue);
            UpdateStressLevelValue(stressLevelSlider.value);
            
            launchSickNumberSlider.onValueChanged.AddListener(UpdateSickNumberValue);
            UpdateSickNumberValue(launchSickNumberSlider.value);
            
            launchImmunedNumberSlider.onValueChanged.AddListener(UpdateImmunedNumberValue);
            UpdateImmunedNumberValue(launchImmunedNumberSlider.value);
            
            diseaseDurationSlider.onValueChanged.AddListener(UpdateDiseaseDurationValue);
            UpdateDiseaseDurationValue(diseaseDurationSlider.value);
            
            deathStatisticSlider.onValueChanged.AddListener(UpdateDeathStatisticValue);
            UpdateDeathStatisticValue(deathStatisticSlider.value);
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

        private void UpdateSickNumberValue(float value)
        {
            launchSickNumberValue.text = value.ToString();
        }
        
        private void UpdateImmunedNumberValue(float value)
        {
            launchImmunedNumberValue.text = value.ToString();
        }
        
        private void UpdateDiseaseDurationValue(float value)
        {
            diseaseDurationValue.text = value.ToString();
        }
        
        private void UpdateDeathStatisticValue(float value)
        {
            deathStatisticValue.text = value.ToString();
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
            simulationData.launchSickNumber = launchSickNumberSlider.value;
            simulationData.launchImmunedNumber = launchImmunedNumberSlider.value;
            simulationData.diseaseDuration = (uint) diseaseDurationSlider.value;
            simulationData.deathStatistic = deathStatisticSlider.value;

            yield return null;

            m_openSimulationScene = SceneManager.LoadSceneAsync(simulationSceneName);
            m_openSimulationScene.allowSceneActivation = false;

            while (!m_openSimulationScene.isDone)
            {
                //Output the current progress
                Debug.Log("Loading progress: " + (m_openSimulationScene.progress * 100).ToString() + "%");

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
