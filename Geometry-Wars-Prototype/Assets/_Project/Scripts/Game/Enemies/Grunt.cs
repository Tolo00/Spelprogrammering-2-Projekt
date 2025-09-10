using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Grunt : EnemyBase {
    [Header("Grunt Settings")]
    [SerializeField] float _acceleration = 3f;
    
    Vector2 _direction;
    float _speed = 0;

    Rigidbody2D _rb;
    Player _player;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _player = FindAnyObjectByType<Player>();
    }

    void FixedUpdate() {
        ExecuteBehaviour();
    }

    private void ExecuteBehaviour() {
        _direction = (_player.transform.position - transform.position).normalized;
        _speed += _acceleration * Time.fixedDeltaTime;

        _rb.MovePosition(transform.position + (Vector3)(_speed * Time.fixedDeltaTime * _direction));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Vector2 normal = collision.GetContact(0).normal;
        _direction = Vector2.Reflect(_direction, normal);
    }

}
