using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] float startDelay;

    [Header("Spawn Parameters")]
    [SerializeField] float _minSpawnInterval = 1f;
    [SerializeField] float _maxSpawnInterval = 5f;
    [Space]
    [SerializeField] float _minSpawnDistanceFromPlayer = 2f;
    [Space]
    [SerializeField] int _minEnemiesPerInterval = 2;
    [SerializeField] int _maxEnemiesPerInterval = 6;

    [Header("References")]
    [SerializeField] BoxCollider2D _bounds;
    [SerializeField] Player _player;
    [Space]
    [SerializeField] GameObject[] _enemyPrefabs;


    private List<EnemyBase> _enemiesAlive = new();
    private bool spawningEnabled;

    private float spawnInterval;
    private float spawnIntervalTimer;


    void Start() {
        Invoke("EnableSpawning", startDelay);
    }
    private void EnableSpawning() => spawningEnabled = true;

    void Update() {
        if (!spawningEnabled) return;

        spawnIntervalTimer += Time.deltaTime;
        if (spawnIntervalTimer >= spawnInterval) {
            spawnIntervalTimer = 0;
            spawnInterval = Random.Range(_minSpawnInterval, _maxSpawnInterval);

            TriggerSpawn();
        }
    }

    private void TriggerSpawn() {
        
        // Spawn one wave of enemies around the player
        int spawnAmount = (int)Random.Range((float)_minEnemiesPerInterval, (float)_maxEnemiesPerInterval+1);
        for (int i = 0; i < spawnAmount; i++) {
            int enemyIndex = (int)Random.Range(0f, _enemyPrefabs.Length);
            GameObject enemyPrefab = _enemyPrefabs[enemyIndex];

            SpawnEnemyAtRandomInBounds(enemyPrefab);
        }
    }


    private void SpawnEnemyAtRandomInBounds(GameObject enemyPrefab) {
        Vector2 pos;
        do {
            pos = GetRandomPointInsideBounds();
        } while (Vector2.Distance(pos, (Vector2)_player.transform.position) < _minSpawnDistanceFromPlayer);

        SpawnEnemy(enemyPrefab, pos);
    }





    private EnemyBase SpawnEnemy(GameObject enemyPrefab, Vector2 position) {
        var inst = Instantiate(enemyPrefab);
        EnemyBase enemy = inst.GetComponent<EnemyBase>();
        if (enemy == null) {
            Debug.LogError("Enemy does not have a root component that inherits from EnemyBase.");
            Destroy(inst);
            return null;
        }

        // Set enemy state
        float rotation = Random.Range(0, 360);
        enemy.transform.SetPositionAndRotation(position, Quaternion.AngleAxis(rotation, Vector3.forward));

        // Subscribe to destroy event
        enemy.OnDestroyEvent += OnEnemyDestroyEvent;
        _enemiesAlive.Add(enemy);
        return enemy;
    }

    private void OnEnemyDestroyEvent(object sender, System.EventArgs e) {
        if (sender is EnemyBase enemy) {
            if (!_enemiesAlive.Contains(enemy)) 
                Debug.LogError("Enemy should not exist!");
            
            enemy.OnDestroyEvent -= OnEnemyDestroyEvent;
            _enemiesAlive.Remove(enemy);
        }
    }

    public Vector2 GetRandomPointInsideBounds() {
        float x = Random.Range(_bounds.bounds.min.x, _bounds.bounds.max.x);
        float y = Random.Range(_bounds.bounds.min.y, _bounds.bounds.max.y);
        return new Vector2(x, y);
    }
}
