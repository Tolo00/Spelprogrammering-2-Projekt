using System;
using System.IO;
using Arosoul.Essentials;
using UnityEngine;

public class SaveData {
    public int Highscore;
}

public class GameManager : Singleton<GameManager> {
    private const string SAVE_DATA_PATH = "save.json";
    public SaveData RuntimeSaveData { get; private set; }

    // Events
    public event EventHandler<int> OnPlayerLivesChanged;
    public event EventHandler<int> OnCurrentScoreChanged;
    public event EventHandler<int> OnScoreMultiplierChanged;
    public event EventHandler<float> OnGameTimerTick; 


    // Player Lives
    private int _playerLives;
    public int PlayerLives { 
        get { return _playerLives;
        } 
        private set {
            _playerLives = value;
            OnPlayerLivesChanged?.Invoke(this, PlayerLives);
        }
    }

    public void AddToPlayerLives(int amount) {
        PlayerLives += amount;
    }


    // Score & Highscore
    private int _currentScore;
    public int CurrentScore { 
        get { return _currentScore; } 
        private set {
            _currentScore = value;
            if (CurrentScore > Highscore) {
                Highscore = CurrentScore;
            }
            
            CheckScore();
            OnCurrentScoreChanged?.Invoke(this, _currentScore);
        } 
    }

    public void AddScoreWithMulti(int scoreToAdd) {
        CurrentScore += scoreToAdd * ScoreMultiplier;
    }

    public int Highscore { get; private set; }


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
            if (_scoreMultiplier <= 0) _scoreMultiplier = 1;
            _scoreMultiplier = value;

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



    protected override void OnSingletonEnable() {
        LoadGameData();
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
        GameTimerTick();
    }



    public void SaveHighscore() {
        if (CurrentScore > RuntimeSaveData.Highscore) {
            RuntimeSaveData.Highscore = CurrentScore;
            SaveGameData();
        }
    }

    private void SaveGameData() {
        string path = Path.Combine(Application.persistentDataPath, SAVE_DATA_PATH);
        string json = JsonUtility.ToJson(RuntimeSaveData, false);
        File.WriteAllText(path, json);
    }

    private void LoadGameData() {
        string path = Path.Combine(Application.persistentDataPath, SAVE_DATA_PATH);
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            RuntimeSaveData = JsonUtility.FromJson<SaveData>(json);
        }
        else {
            RuntimeSaveData = new SaveData();
        }

    }
}
