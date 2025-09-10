using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Wanderer : EnemyBase {
    [Header("Wanderer Settings")]
    [SerializeField] float _minSpeed = 3f;
    [SerializeField] float _maxSpeed = 5f;
    [Space]
    [SerializeField] float _angularVelRandomness = 100f;
    [Space]
    [SerializeField] float _minDelay = 1f;
    [SerializeField] float _maxDelay = 3f;
    
    Vector2 _direction;
    float _speed;

    float _newTargetDelay;
    float _newTargetTimer;

    Rigidbody2D _rb;

    void Awake() {
        _rb = GetComponent<Rigidbody2D>();

        SetNewTarget();
    }

    void FixedUpdate() {
        _newTargetTimer += Time.fixedDeltaTime;
        if (_newTargetTimer >= _newTargetDelay) {
            SetNewTarget();
            _newTargetTimer = 0;
        }

        ExecuteBehaviour();
    }


    private void SetNewTarget() {        
        int nextState = Random.Range(0, 2); // 50% chance it idles for delay time

        if (nextState != 0) {
            // New direction, start wandering
            float randomAngle = Random.Range(0, 360);
            _direction = randomAngle.DegreesToVector2();

            _speed = Random.Range(_minSpeed, _maxSpeed);  
            _rb.angularVelocity = Random.Range(-_angularVelRandomness, _angularVelRandomness);

        }
        else {
            // Idle
            _speed = 0;
        }

        _newTargetDelay = Random.Range(_minDelay, _maxDelay);
    }

    private void ExecuteBehaviour() {
        _rb.MovePosition(transform.position + (Vector3)(_speed * Time.fixedDeltaTime * _direction));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Vector2 normal = collision.GetContact(0).normal;
        _direction = Vector2.Reflect(_direction, normal);
    }

}
