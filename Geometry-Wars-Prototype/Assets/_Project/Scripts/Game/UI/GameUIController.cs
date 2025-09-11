using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour {
    [SerializeField] TMP_Text _scoreTextElement;
    [SerializeField] TMP_Text _highscoreTextElement;
    [SerializeField] TMP_Text _multiplierTextElement;
    [SerializeField] TMP_Text _timerTextElement;
    [Space]
    [SerializeField] GameObject livesIconContainer;
    [SerializeField] GameObject[] livesIcons;
    [Space]
    [SerializeField] GameObject FourOrMoreLivesContainer;
    [SerializeField] TMP_Text FourOrMoreLivesTextElement;

    void OnEnable() {
        Debug.Assert(GameManager.Inst != null, "No GameManager in scene.");

        GameManager.Inst.OnCurrentScoreChanged += OnScoreChanged;
        GameManager.Inst.OnScoreMultiplierChanged += OnScoreMultiplierChanged;
        GameManager.Inst.OnPlayerLivesChanged += OnPlayerLivesChanged;

        GameManager.Inst.OnGameTimerTick += OnGameTimerTick;
    }

    void OnDisable() {
        if (GameManager.Inst == null) return;

        GameManager.Inst.OnCurrentScoreChanged -= OnScoreChanged;
        GameManager.Inst.OnScoreMultiplierChanged -= OnScoreMultiplierChanged;
        GameManager.Inst.OnPlayerLivesChanged -= OnPlayerLivesChanged;

        GameManager.Inst.OnGameTimerTick -= OnGameTimerTick;

    }



    private void OnPlayerLivesChanged(object sender, int lives) {
        SetPlayerLivesCount(lives);
    }

    private void OnScoreChanged(object sender, int score) {
        string formattedScore = score.ToString("N0");
        string formattedHighscore = GameManager.Inst.Highscore.ToString("N0");

        _scoreTextElement.text = $"SCORE\n{formattedScore}";
        _highscoreTextElement.text = $"HIGHSCORE\n{formattedHighscore}";
    }   

    private void OnScoreMultiplierChanged(object sender, int multiplier) {
        _multiplierTextElement.text = $"x{multiplier}";
    }

    private void OnGameTimerTick(object sender, float time) {
        System.TimeSpan formattedTime = System.TimeSpan.FromSeconds(time);
        _timerTextElement.text = formattedTime.ToString(@"mm\:ss");
    }


    private void SetPlayerLivesCount(int count) {
        if (count <= 3) {
            // 1-3 lives
            livesIconContainer.SetActive(true);
            FourOrMoreLivesContainer.SetActive(false);

            for (int i = 0; i < 3; i++) {
                livesIcons[i].SetActive(i<count);
            }
        }
        else {
            // 4+ lives
            livesIconContainer.SetActive(false);
            FourOrMoreLivesContainer.SetActive(true);

            FourOrMoreLivesTextElement.text = count.ToString();
        }
    } 



    // Debug
    private int debugLives = 3;
    [ContextMenu("Increase Lives (Debug)")]
    public void IncreaseLives() {
        debugLives++;
        SetPlayerLivesCount(debugLives);
    }

    [ContextMenu("Decrease Lives (Debug)")]
    public void DecreaseLives() {
        debugLives--;
        SetPlayerLivesCount(debugLives);
    } 
}
