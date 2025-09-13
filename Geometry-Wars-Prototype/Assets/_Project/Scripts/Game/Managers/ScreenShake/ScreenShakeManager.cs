using Arosoul.Essentials;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ScreenShakeManager : Singleton<ScreenShakeManager> {
    [SerializeField] ScreenShakeProfile _defaultProfile;
    [SerializeField] CinemachineImpulseListener _impulseListener;

    private CinemachineImpulseSource _impulseSource;
    private CinemachineImpulseDefinition _impulseDefinition;
    
    private void Awake() {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _impulseDefinition = _impulseSource.ImpulseDefinition;
    }


    public void CameraShake(float impulseForce, float impulseTime) {
        ApplySettingsFromProfile(_defaultProfile, impulseTime);
        _impulseSource.GenerateImpulseWithForce(impulseForce);
    }

    public void CameraShake(float impulseForce, float impulseTime, ScreenShakeProfile profile) {
        ApplySettingsFromProfile(profile, impulseTime);
        _impulseSource.GenerateImpulseWithForce(impulseForce);
    }



    
    private void ApplySettingsFromProfile(ScreenShakeProfile profile, float impulseTime) {
        // Impulse source
        _impulseDefinition.ImpulseDuration = impulseTime;
        _impulseSource.DefaultVelocity = profile.defaultVelocity;
        _impulseDefinition.CustomImpulseShape = profile.impulseCurve;
    
        // Impulse Listener
        _impulseListener.ReactionSettings.AmplitudeGain = profile.listenerAmplitude;
        _impulseListener.ReactionSettings.FrequencyGain = profile.listenerFrequency;
        _impulseListener.ReactionSettings.Duration = profile.listenerDuration;
    }
}
