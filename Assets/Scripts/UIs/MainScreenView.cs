using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI 
{
    public class MainScreenView : MonoBehaviour
    {
        [Header("UI State Game Objects")]
        [SerializeField] private GameObject _introScreenGO;
        [SerializeField] private GameObject _gameModeScreenGO;
        [SerializeField] private GameObject _creditsScreenGO;
        [SerializeField] private int _levelSceneIndex;
        
        [Header("Controller Buttons")]
        [SerializeField] private ControllerButtonDirection _startKey = ControllerButtonDirection.South;
        [SerializeField] private ControllerButtonDirection _creditsKey = ControllerButtonDirection.East;

        private UIState _uiState;

        private void Start()
        {
            SwitchUIState(UIState.Intro);
        }

        private void Update()
        {
            if (InputUtility.IsButtonDown(0, _startKey) || InputUtility.IsButtonDown(1, _startKey))
            {
                OnTwoPlayerGameStartsClicked();
            }
            else if (InputUtility.IsButtonDown(0, _creditsKey) || InputUtility.IsButtonDown(1, _creditsKey))
            {
                if(_uiState == UIState.Intro)
                    OnCreditsButtonClicked();
                else
                    OnBackButtonClicked();
            }
        }

        private void SwitchUIState(UIState state)
        {
            _uiState = state;
            ToggleGameObjects();
        }

        private void ToggleGameObjects()
        {
            _introScreenGO.SetActive(_uiState == UIState.Intro);
            _gameModeScreenGO.SetActive(_uiState == UIState.GameMode);
            _creditsScreenGO.SetActive(_uiState == UIState.Credits);
        }

        public void OnStartButtonClicked()
        {
            SwitchUIState(UIState.GameMode);
        }

        public void OnCreditsButtonClicked()
        {
            Debug.LogWarning("OnCreditsButtonClicked calling");
            SwitchUIState(UIState.Credits);
        }

        public void OnBackButtonClicked()
        {
            SwitchUIState(UIState.Intro);
        }
        
        public void OnTwoPlayerGameStartsClicked()
        {
            SceneManager.LoadScene(_levelSceneIndex);
        }
        
        private enum UIState
        {
            Intro,
            GameMode,
            Credits
        }
    }
}