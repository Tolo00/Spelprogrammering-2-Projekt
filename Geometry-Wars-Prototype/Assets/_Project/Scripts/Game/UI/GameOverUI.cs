using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField] GameObject _container;
    
    [Header("UI Elements")]
    [SerializeField] TMP_Text _valuesTextElement;

    [SerializeField] Button _restartButton;
    [SerializeField] Button _quitButton;

    void Awake() {
        _container.SetActive(false);

        // Events / Callbacks
        _restartButton.onClick.AddListener(RestartButton_OnClick);
        _quitButton.onClick.AddListener(QuitButton_OnClick);

        GameManager.Inst.OnGameOver += GameManager_OnGameOver;
    }


    private void GameManager_OnGameOver(object sender, EventArgs e) {
        string formattedScore = GameManager.Inst.CurrentScore.ToString("N0");
        string formattedMulti = $"x{GameManager.Inst.ScoreMultiplier}";

        _valuesTextElement.text = formattedScore + "\n" +
                                  formattedMulti;
    
        _container.SetActive(true);
        _restartButton.Select();
    }



    // UI Callbacks
    private void RestartButton_OnClick() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void QuitButton_OnClick() {
        SceneManager.LoadScene(0); // First scene = main menu
    }
}
