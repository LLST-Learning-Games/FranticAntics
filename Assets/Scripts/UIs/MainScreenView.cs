using UnityEngine;

public class MainScreenView : MonoBehaviour
{
    [Header("UI State Game Objects")]
    [SerializeField] private GameObject _introScreenGO;
    [SerializeField] private GameObject _gameModeScreenGO;
    [SerializeField] private GameObject _creditsScreenGO;

    private UIState _uiState;

    private void Start()
    {
        SwitchUIState(UIState.Intro);
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
        Debug.LogWarning("OnTwoPlayerGameStartsClicked calling");
    }
    
    private enum UIState
    {
        Intro,
        GameMode,
        Credits
    }
}
