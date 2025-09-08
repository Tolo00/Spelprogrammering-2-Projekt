using UnityEngine;

public static class Bootloader {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap() {
        InitializeSingletons();
    }

    private static void InitializeSingletons() {
        _ = GameInput.Inst;
    }
}