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
        private Button launchFirstScenario;
        [SerializeField] 
        private Button launchSecondScenario;
        [SerializeField] 
        private Button launchThirdScenario;

        [SerializeField]
        private Slider populationDensitySlider;
        [SerializeField]
        private Text populationDensityValue;

        [SerializeField]
        private Slider infectivitySlider;
        [SerializeField]
        private Text infectivityValue;

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

        private void Start()
        {
            launchSimulationScene.onClick.AddListener(LoadButton);
            launchFirstScenario.onClick.AddListener(LaunchFirstScenario);
            launchSecondScenario.onClick.AddListener(LaunchSecondScenario);
            launchThirdScenario.onClick.AddListener(LaunchThirdScenario);
            
            populationDensitySlider.onValueChanged.AddListener(UpdatePopulationDensityValue);
            UpdatePopulationDensityValue(populationDensitySlider.value);

            infectivitySlider.onValueChanged.AddListener(UpdateInfectivityValue);
            UpdateInfectivityValue(infectivitySlider.value);

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

        private void UpdateInfectivityValue(float value)
        {
            infectivityValue.text = value.ToString();
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

        private void LaunchFirstScenario()
        {
            populationDensitySlider.value = 1000;
            infectivitySlider.value = 0.5f;
            launchSickNumberSlider.value = 0.1f;
            launchImmunedNumberSlider.value = 0.5f;
            diseaseDurationSlider.value = 100;
            deathStatisticSlider.value = 0.1f;
            
            StartCoroutine(LoadScene());
        }
        
        private void LaunchSecondScenario()
        {
            populationDensitySlider.value = 1000;
            infectivitySlider.value = 1f;
            launchSickNumberSlider.value = 0.15f;
            launchImmunedNumberSlider.value = 0;
            diseaseDurationSlider.value = 20;
            deathStatisticSlider.value = 0.6f;
            
            StartCoroutine(LoadScene());
        }
        
        private void LaunchThirdScenario()
        {
            populationDensitySlider.value = 2000;
            infectivitySlider.value = 0.2f;
            launchSickNumberSlider.value = 0.1f;
            launchImmunedNumberSlider.value = 0;
            diseaseDurationSlider.value = 60;
            deathStatisticSlider.value = 0.4f;
            
            StartCoroutine(LoadScene());
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
