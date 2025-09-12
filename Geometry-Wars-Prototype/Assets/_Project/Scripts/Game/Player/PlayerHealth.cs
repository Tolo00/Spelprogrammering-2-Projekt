using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [SerializeField] float _respawnDelay = 2f;
    [SerializeField] float _invulnerableTime = 3f;

    private bool _invulnerable;
    public bool Invulnerable { 
        get { return _invulnerable; }
        set {
            _invulnerable = value;
            OnInvulnerableChanged?.Invoke(this, _invulnerable);
        }
    }

    private bool _respawing = false;

    private Vector2 _spawnPosition;


    // Events
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerRespawn;

    public event EventHandler<bool> OnInvulnerableChanged;


    void Start() {
        _spawnPosition = transform.position;
    }


    void OnCollisionStay2D(Collision2D collision) {
        if (_respawing) return;

        if (collision.collider.TryGetComponent(out EnemyBase enemy)) {
            RespawnSequence();
        }
    }

    public async void RespawnSequence() {
        _respawing = true;
        
        // Kill Player
        gameObject.SetActive(false);
        GameManager.Inst.AddToPlayerLives(-1);
        GameManager.Inst.SetScoreMultiplier(1);

        OnPlayerDeath?.Invoke(this, EventArgs.Empty);

        await Awaitable.WaitForSecondsAsync(_respawnDelay);
        
        if (GameManager.Inst.GameOver) return; // Don't respawn if game has ended

        // Respawn
        transform.position = _spawnPosition;
        Invulnerable = true;

        gameObject.SetActive(true);
        OnPlayerRespawn?.Invoke(this, EventArgs.Empty);

        await Awaitable.WaitForSecondsAsync(_invulnerableTime);

        // Stop invulnerability
        Invulnerable = false;

        _respawing = false;
    }
}