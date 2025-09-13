using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Tank : EnemyBase {
    [Header("Tank Settings")]
    [SerializeField] int _health = 50;
    [SerializeField] float _acceleration = 1.5f;
    [SerializeField] float _maxSpeed = 3f;
    
    Vector2 _direction;
    float _speed = 0;

    Rigidbody2D _rb;
    Player _player;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);
    
        Health = _health;
    }

    void FixedUpdate() {
        if (!_player.gameObject.activeInHierarchy) return;

        ExecuteBehaviour();
    }

    private void ExecuteBehaviour() {
        _direction = (_player.transform.position - transform.position).normalized;
        _speed += _acceleration * Time.fixedDeltaTime;

        _speed = Mathf.Min(_speed, _maxSpeed);

        _rb.MovePosition(transform.position + (Vector3)(_speed * Time.fixedDeltaTime * _direction));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Vector2 normal = collision.GetContact(0).normal;
        _direction = Vector2.Reflect(_direction, normal);
    }

}
