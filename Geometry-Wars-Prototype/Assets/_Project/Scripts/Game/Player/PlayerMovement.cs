using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField] float _forwardSpeed;
    [SerializeField] float _turnSpeed;


    private Rigidbody2D _rb;

    void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        Vector2 moveDir = GameInput.Move;
        if (moveDir == Vector2.zero) return;

        // Move forward
        _rb.linearVelocity = transform.up * _forwardSpeed;

        // Turn
    }
}
