using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IHittable {
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
    }

    public virtual void DestroySelf() {
        Destroy(gameObject);
        OnDestroyEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnHit() {
        AddToHP(-1);
    }
}