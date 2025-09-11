using Arosoul.Essentials;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField] float _forwardSpeed;
    [SerializeField] float _turnSpeed;


    private Rigidbody2D _rb;
    private Player _player;

    void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
    }

    void Update() {
        if (!_player.InputEnabled) return;

        Vector2 moveDir = GameInput.Move;
        Vector2 forwardDir = transform.up;
        if (moveDir == Vector2.zero) return;

        // Move forward
        _rb.linearVelocity = _forwardSpeed * moveDir;

        // Turn to move direction
        float cross = (moveDir.x*forwardDir.y) - (moveDir.y*forwardDir.x); // Calculate direction using 2D cross product
        int dirToTurn = cross >= 0 ? -1 : 1;

        float dot = Vector2.Dot(moveDir, forwardDir);
        if (Vector2.Angle(moveDir, forwardDir) < 1f) return; // Stop rotating when really close to target angle

        float turnVelocity = _turnSpeed * dot.MapValue(-1, 1, 1, 0.4f);
        
        transform.Rotate(dirToTurn * turnVelocity * Time.deltaTime * Vector3.forward);
    }
}
