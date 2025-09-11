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
        _playerHealth.OnPlayerDeath += OnPlayerDeath;
        _playerHealth.OnPlayerRespawn += OnPlayerRespawn;

        _playerHealth.OnInvulnerableChanged += OnInvulnerableChanged;
    }

    

    private void OnPlayerDeath(object sender, EventArgs e) {
        // Death particles
        if (_deathParticles != null) {
            ParticleSystem inst = Instantiate(_deathParticles, null);
            inst.transform.position = transform.position;
        }

        _shipRenderer.enabled = false;
    }

    private void OnPlayerRespawn(object sender, EventArgs e) {
        _shipRenderer.enabled = true;
    }


    private void OnInvulnerableChanged(object sender, bool invulnerable) {
        _shieldRenderer.enabled = invulnerable;
    }
}