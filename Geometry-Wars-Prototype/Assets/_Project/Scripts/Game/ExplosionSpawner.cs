using UnityEngine;


public class ExplosionSpawner : MonoBehaviour {
    [System.Serializable]
    public struct ExplosionVariant {
        public ParticleSystem ParticlePrefab;
        public float WarpingForce;
        public float WarpingRadius;
    }


    [SerializeField] float startDelay;

    [Header("Explosions Parameters")]
    [SerializeField] float _minSpawnInterval = 1f;
    [SerializeField] float _maxSpawnInterval = 5f;
    [Space]
    [SerializeField] float _explosionShakeForce = 0.3f;
    [SerializeField] float _explosionShakeTime = 0.5f;

    [Header("References")]
    [SerializeField] BoxCollider2D _bounds;
    [SerializeField] ExplosionVariant[] _explosionVarients;

    private bool _spawningEnabled;

    private float _spawnInterval;
    private float _spawnIntervalTimer;


    void Start() {
        Invoke("EnableSpawning", startDelay);
    }
    private void EnableSpawning() => _spawningEnabled = true;



    void Update() {
        if (!_spawningEnabled) return;

        _spawnIntervalTimer += Time.deltaTime;
        if (_spawnIntervalTimer >= _spawnInterval) {
            _spawnIntervalTimer = 0;
            _spawnInterval = Random.Range(_minSpawnInterval, _maxSpawnInterval);

            SpawnExplosionAtRandomInBounds();
        }
    }

    private void SpawnExplosionAtRandomInBounds() {
        ExplosionVariant explosionVariant = _explosionVarients[Random.Range(0, _explosionVarients.Length)];

        Vector2 pos = GetRandomPointInsideBounds();

        var inst = Instantiate(explosionVariant.ParticlePrefab, transform);
        inst.transform.position = pos;

        WarpingGrid.ApplyExplosiveForce(explosionVariant.WarpingForce, pos, explosionVariant.WarpingRadius);
        ScreenShakeManager.Inst.CameraShake(_explosionShakeForce, _explosionShakeTime);
    }

    public Vector2 GetRandomPointInsideBounds() {
        float x = Random.Range(_bounds.bounds.min.x, _bounds.bounds.max.x);
        float y = Random.Range(_bounds.bounds.min.y, _bounds.bounds.max.y);
        return new Vector2(x, y);
    }
}
