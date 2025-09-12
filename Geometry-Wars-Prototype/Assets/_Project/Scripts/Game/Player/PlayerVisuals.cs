using System;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour {
    [Header("References")]
    [SerializeField] PlayerHealth _playerHealth;
    [Space]
    [SerializeField] SpriteRenderer _shipRenderer;
    [SerializeField] SpriteRenderer _shieldRenderer;

    [Header("Particles")]
    [SerializeField] ParticleSystem _deathParticles;

    void Awake() {
        _shieldRenderer.enabled = false;

        // Events
        _playerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        _playerHealth.OnPlayerRespawn += PlayerHealth_OnPlayerRespawn;

        _playerHealth.OnInvulnerableChanged += PlayerHealth_OnInvulnerableChanged;

        GameManager.Inst.OnGameOver += GameManager_OnGameOver;
    }

    

    private void PlayerHealth_OnPlayerDeath(object sender, EventArgs e) {
        // Death particles
        if (_deathParticles != null) {
            ParticleSystem inst = Instantiate(_deathParticles, null);
            inst.transform.position = transform.position;
        }

        _shipRenderer.enabled = false;
    }

    private void PlayerHealth_OnPlayerRespawn(object sender, EventArgs e) {
        _shipRenderer.enabled = true;
    }

    private void PlayerHealth_OnInvulnerableChanged(object sender, bool invulnerable) {
        _shieldRenderer.enabled = invulnerable;
    }

    
    private void GameManager_OnGameOver(object sender, EventArgs e) {
        _shipRenderer.enabled = false;
        _shieldRenderer.enabled = false;
    }
}