using UnityEngine;

public enum WarpingType { Explosive, Impulse }

public class EffectOnDestroy : MonoBehaviour {
    [SerializeField] Vector2 _offset;

    [Header("Particle & Sound")]
    [SerializeField] GameObject _particle;
    [Space]
    [SerializeField] AudioClip _sound;
    [SerializeField, Range(0, 1.5f)] float _volume = 1f;

    [Header("Warping Grid")]
    [SerializeField] bool warpingEnabled = false;
    [SerializeField] WarpingType warpingType ;
    [SerializeField] float warpingForce = 5f;
    [SerializeField] float warpingRadius = 2f;

    bool isQuitting = false;
    void OnApplicationQuit() {
        isQuitting = true;
    }

    void OnDestroy() {
        if (isQuitting) return;

        SpawnParticle();
        PlaySound();
        WarpingEffect();
    }


    private void SpawnParticle() {
        if (_particle == null) return;

        var inst = Instantiate(_particle, null);
        inst.transform.position = (Vector2)transform.position + _offset;
    }

    private void PlaySound() {
        if (_sound == null) return;
        AudioSource.PlayClipAtPoint(_sound, (Vector2)transform.position + _offset, _volume);
    }

    private void WarpingEffect() {
        if (!warpingEnabled) return;
        if (warpingType == WarpingType.Explosive) WarpingGrid.ApplyExplosiveForce(warpingForce, transform.position, warpingRadius);
        if (warpingType == WarpingType.Impulse) WarpingGrid.ApplyImpulseForce(warpingForce, transform.position, warpingRadius);
    }
}
