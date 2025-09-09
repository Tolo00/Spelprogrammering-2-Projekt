using UnityEngine;

public class DefaultGun : GunBase {
    [Space]
    [SerializeField, Min(0)] float _spreadRandomness = 4f;
    [SerializeField, Min(1)] int _projectileCount = 2;
    [Space]
    [SerializeField, Min(0)] float _speed = 5f; 
    [SerializeField, Min(0)] float _speedRandomness = 0.5f;
    [Space]
    [SerializeField] GameObject _simpleProjectilePrefab;

    protected override void Fire() {
        float originalAngle = Direction.ToDegrees();

        for (int i = 0; i < _projectileCount; i++) {
            float projAngle = originalAngle + Random.Range(-_spreadRandomness, _spreadRandomness);
            float projSpeed = _speed + Random.Range(-_speedRandomness, _speedRandomness);
            
            GameObject inst = Instantiate(_simpleProjectilePrefab, null);
            ProjectileBase projectile = inst.GetComponent<ProjectileBase>();
        
            if (projectile == null) {
                Debug.LogError("Projectile prefab does not implement ProjectileBase in any root components.");
                return;
            }

            projectile.Initialize(FireOrigin, projSpeed, projAngle.DegreesToVector2());
        }
    }
}