using System;
using UnityEngine;

public class GridPoint {
    
    public Vector2 Position;
    public Vector2 Velocity;
    public float InverseMass;

    private Vector2 _acc;
    private float _damping = 0.98f;

    public GridPoint(Vector2 position, float inverseMass) {
        Position = position;
        InverseMass = inverseMass;
    }
    

    public void AddForce(Vector2 force) {
        Velocity += force * InverseMass;
    }

    public void IncreaseDamping(float factor) {
        _damping *= factor;
    }

    public void Tick() {
        Velocity += _acc;
        Position += Velocity;
        _acc = Vector2.zero;

        if (Velocity.sqrMagnitude < 0.001 * 0.001f) {
            Velocity = Vector3.zero;
        }

        Velocity *= _damping;
        _damping = 0.98f;
    }
}