using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class ProjectileBase : MonoBehaviour {
    protected float Speed;
    protected void SetSpeed(float speed) {
        Speed = speed;
    }

    protected Vector2 Direction { get; private set; }
    protected void SetDirection(Vector2 direction) {
        Direction = direction;

        // Rotate projectile
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected Rigidbody2D Rigidbody;


    public void Initialize(Vector2 startPos, float initialSpeed, Vector2 initialDirection) {
        SetSpeed(initialSpeed);
        SetDirection(initialDirection);

        Rigidbody.linearVelocity = Speed * Direction;
        transform.position = startPos;

        OnInitialize();
    }


    void Awake() {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.gravityScale = 0;
        Rigidbody.linearDamping = 0;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
    void Update() => OnUpdateEvent();
    void OnCollisionEnter2D(Collision2D collision) => OnCollisionEvent(collision);


    protected virtual void OnInitialize() { }

    protected virtual void OnUpdateEvent() { }

    protected virtual void OnCollisionEvent(Collision2D collision) {
        Destroy(gameObject);
    }
}