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

    
    private int _playerLives;
    public int PlayerLives { 
        get { return _playerLives;
        } 
        private set {
            _playerLives = value;
            OnPlayerLivesChanged?.Invoke(this, PlayerLives);
        }
    }

    private int _currentScore;
    public int CurrentScore { 
        get { return _currentScore; } 
        private set {
            _currentScore = value;
            if (CurrentScore > Highscore) {
                Highscore = CurrentScore;
            }
            OnCurrentScoreChanged?.Invoke(this, CurrentScore);
        } 
    }
    public int Highscore { get; private set; }

    private int _scoreMultiplier;
    public int ScoreMultiplier { 
        get { return _scoreMultiplier;
        } 
        private set {
            _scoreMultiplier = value;
            OnScoreMultiplierChanged?.Invoke(this, ScoreMultiplier);
        }
    }


    protected override void OnSingletonEnable() {
        LoadGameData();
    }

    void Start() {
        InitializeValues();
    }

    private void InitializeValues() {
        CurrentScore = 0;
        PlayerLives = 3;
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
