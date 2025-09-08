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
        _rb.linearVelocity = _forwardSpeed * moveDir;

        // Turn to move direction
        float cross = (moveDir.y*transform.up.x) - (moveDir.x*transform.up.y); // Calculate direction using 2D cross product
        if (cross > -0.01 && cross < 0.01) return; // Stop rotating at really low values
        
        transform.Rotate(_turnSpeed * cross * Time.deltaTime * Vector3.forward);
    }
}
