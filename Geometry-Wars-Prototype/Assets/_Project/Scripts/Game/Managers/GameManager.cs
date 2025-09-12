using System;
using System.IO;
using Arosoul.Essentials;
using UnityEngine;

public class SaveData {
    public int Highscore;
    public int HighestMultiplier;
}

public class GameManager : Singleton<GameManager> {
    private const string SAVE_DATA_PATH = "save.json";
    public SaveData RuntimeSaveData { get; private set; }

    // Events
    public event EventHandler<int> OnPlayerLivesChanged;
    public event EventHandler<int> OnCurrentScoreChanged;
    public event EventHandler<int> OnScoreMultiplierChanged;

    public event EventHandler<float> OnGameTimerTick; 
    public event EventHandler OnGameOver;

    // Player Lives
    private int _playerLives;
    public int PlayerLives { 
        get { return _playerLives;
        } 
        private set {
            _playerLives = value;

            CheckPlayerLives();
            OnPlayerLivesChanged?.Invoke(this, PlayerLives);
        }
    }

    public void AddToPlayerLives(int amount) {
        PlayerLives += amount;
    }

    private void CheckPlayerLives() {
        if (_playerLives <= 0) {
            TriggerGameOver();
        }
    }


    // Score & Highscore
    private int _currentScore;
    public int CurrentScore { 
        get { return _currentScore; } 
        private set {
            _currentScore = value;
            if (CurrentScore > RuntimeSaveData.Highscore) {
                RuntimeSaveData.Highscore = CurrentScore;
            }
            
            CheckScore();
            OnCurrentScoreChanged?.Invoke(this, _currentScore);
        } 
    }

    public void AddScoreWithMulti(int scoreToAdd) {
        CurrentScore += scoreToAdd * ScoreMultiplier;
    }



    private int _nextLifeScore = 10000;
    private void CheckScore() {
        // Add 1 life at every 100x incriment (10.000, 100.000, 1.000.000, 10.000.000 ...)
        if (CurrentScore >= _nextLifeScore) {
            PlayerLives++;
            _nextLifeScore *= 10;
        }
    }

    // Multiplier
    private int _scoreMultiplier = 1;
    public int ScoreMultiplier { 
        get { return _scoreMultiplier;
        } 
        private set {
            _scoreMultiplier = value;
            if (_scoreMultiplier <= 0) _scoreMultiplier = 1;
            if (_scoreMultiplier > RuntimeSaveData.HighestMultiplier) RuntimeSaveData.HighestMultiplier = _scoreMultiplier;

            OnScoreMultiplierChanged?.Invoke(this, ScoreMultiplier);
        }
    }


    public void SetScoreMultiplier(int multiplier) {
        _scoreMultiplier = multiplier;
    }


    // Timer Logic
    public float GameTimer { get; private set; }
    private void GameTimerTick() {
        GameTimer += Time.deltaTime;
        OnGameTimerTick(this, GameTimer);
    }


    // Game Over
    public bool GameOver { get; private set; }
    public void TriggerGameOver() {
        GameOver = true;
        KillAllEnemies();
        SaveSaveData();

        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    private void KillAllEnemies() {
        EnemyBase[] enemies = FindObjectsByType<EnemyBase>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (EnemyBase enemy in enemies) { enemy.DestroySelf(); }
    }




    protected override void OnSingletonEnable() {
        LoadSaveData();
    }

    void Start() {
        InitializeValues();
    }

    private void InitializeValues() {
        CurrentScore = 0;
        ScoreMultiplier = 1;
        PlayerLives = 3;
    }

    void Update() {
        if (!GameOver) GameTimerTick();
    }




#region Save & Load
    private void SaveSaveData() {
        string path = Path.Combine(Application.persistentDataPath, SAVE_DATA_PATH);
        string json = JsonUtility.ToJson(RuntimeSaveData, false);
        File.WriteAllText(path, json);
    }

    private void LoadSaveData() {
        string path = Path.Combine(Application.persistentDataPath, SAVE_DATA_PATH);
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            RuntimeSaveData = JsonUtility.FromJson<SaveData>(json);
        }
        else {
            RuntimeSaveData = new SaveData();
        }
    }
#endregion
}
