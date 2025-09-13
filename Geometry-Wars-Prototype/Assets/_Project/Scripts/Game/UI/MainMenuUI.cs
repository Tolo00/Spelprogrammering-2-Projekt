using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] float _fadeOutDuration = 1.5f;

    [Header("UI Elements")]
    [SerializeField] CanvasGroup _UIGroup;
    [SerializeField] ExplosionSpawner _explosionSpawner;
    [Space]
    [SerializeField] Button _startButton;
    [SerializeField] Button _quitButton;


    void Start() {
        _startButton.Select();

        // Register callbacks
        _startButton.onClick.AddListener(StartButton_OnClick);
        _quitButton.onClick.AddListener(QuitButton_OnClick);
    }


    private async void StartButton_OnClick() {
        _startButton.interactable = false;
        _quitButton.interactable = false;

        _explosionSpawner.enabled = false;
        
        float stepPerSecond = 1f/_fadeOutDuration;
        while (_UIGroup.alpha > 0) {
            _UIGroup.alpha = Mathf.Max(0f, _UIGroup.alpha - stepPerSecond * Time.deltaTime);
            await Awaitable.NextFrameAsync();
        }

        SceneManager.LoadScene("Game");
    }

    private void QuitButton_OnClick() {
        Application.Quit();
        Debug.Log("Quit");
    }
}


