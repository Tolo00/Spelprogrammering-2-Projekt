using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IHittable {
    [Header("Enemy Settings")]
    [SerializeField] int _scoreToAdd = 10;

    private int _hp;

    public event EventHandler OnDestroyEvent;


    public void AddToHP(int amount) {
        _hp += amount;
        if (_hp <= 0) {
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
        AddToHP(-1);
    }
}