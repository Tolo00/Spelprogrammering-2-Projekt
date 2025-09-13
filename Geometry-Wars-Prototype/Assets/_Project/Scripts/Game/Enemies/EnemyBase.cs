using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IHittable {
    [Header("Enemy Settings")]
    [SerializeField] int _scoreToAdd = 100;

    protected int Health;

    public event EventHandler OnDestroyEvent;


    public void AddToHealth(int amount) {
        Health += amount;
        if (Health <= 0) {
            KillEnemy();
        }
    }

    private void KillEnemy() {
        DestroySelf();
        GameManager.Inst.AddScoreWithMulti(_scoreToAdd);
    }

    public virtual void DestroySelf() {
        Destroy(gameObject);
        OnDestroyEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnHit() {
        AddToHealth(-1);
    }
}