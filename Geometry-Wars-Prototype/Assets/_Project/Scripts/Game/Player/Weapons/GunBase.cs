using UnityEngine;

public abstract class GunBase : MonoBehaviour, IGun {
    
    [Header("Gun Parameters")]
    [SerializeField] float _distanceFromCenter = 0.15f;
    [SerializeField] float _projectilesPerSecond = 1f;

    public bool Enabled { get; private set; }
    public Vector2 Direction { get; private set; } 
    public Vector2 Origin { get; private set; }

    public Vector2 FireOrigin { get; protected set; }

    
    private float _fireTimer = 0f;


    void Update() {
        FireOrigin = Origin + (Direction.normalized * _distanceFromCenter);

        // Call fire x times per second
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= 1/_projectilesPerSecond && Enabled) {
            _fireTimer = 0;
            Fire();
        }
    }


    // Interface (Defined by PlayerGun script)
    public void SetEnabled(bool enabled) => Enabled = enabled;
    public void SetDirection(Vector2 direction) => Direction = direction;
    public void SetOrigin(Vector2 origin) => Origin = origin;

    // Implement in subclass
    protected abstract void Fire();

    
}