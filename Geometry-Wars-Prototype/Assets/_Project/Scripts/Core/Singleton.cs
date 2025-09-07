using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arosoul.Essentials {

    /// <summary>
    /// Generic abstract Singleton class for managing a single instance of a MonoBehaviour-derived object within a scene.
    /// This class ensures only one instance of the object exists in the scene, and destroys any additional instances.
    /// The instance is reset when the scene is unloaded, making it suitable for scene-specific singletons.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
    {
        public static T Inst { get; protected set; }
        protected void OnEnable() {
            if (Inst != null) { 
                Destroy(gameObject);
                return;
            }
            Inst = this as T;

            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += RemoveReferenceOnSceneSwitch;
            OnSingletonEnable();
        }

        protected void OnDisable() {
            if (Inst == this) {
                UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= RemoveReferenceOnSceneSwitch;
                OnSingletonDisable();

                Inst = null;
            }
        }

        void RemoveReferenceOnSceneSwitch(Scene current) {
            if (Inst == null) return;
            if (current.buildIndex == gameObject.scene.buildIndex)
                Inst = null;
        }

        // Override these to ensure the singleton is initialized correctly
        protected virtual void OnSingletonEnable() { }
        protected virtual void OnSingletonDisable() { }
    }

    /// <summary>
    /// Generic abstract SingletonPersistent class for managing a single persistent instance of a MonoBehaviour-derived object across scenes.
    /// This class ensures only one instance of the object exists, and the object is not destroyed when changing scenes.
    /// Useful for global singletons that should persist throughout the application's lifetime.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class SingletonPersistent<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static bool IsInitialized { get; private set; }
        
        private static T inst;
        public static T Inst { 
            get {
                if (inst != null) return inst;

                // Try to load a prefab with the matching component from Resources
                T foundPrefabInstance = LoadPrefabFromResources();
                if (foundPrefabInstance != null) {
                    return inst;
                }
                
                // Lazy loading - Create instance if none exists
                GameObject lazyInst = new GameObject($"[{typeof(T).Name}]");
                lazyInst.AddComponent<T>();
                return inst;
            } 
            protected set => inst = value; 
        }

        private static T LoadPrefabFromResources() {
            // Load all prefabs from Resources that might contain our singleton component
            T[] prefabs = Resources.LoadAll<T>("Singleton"); // Path: Resources/Singleton/

            foreach (var prefab in prefabs) {
                if (prefab != null) {
                    T instance = GameObject.Instantiate(prefab);
                    instance.gameObject.name = $"[{prefab.name}]";
                    return instance;
                }
            }

            return null; // No matching prefab found
        }

        protected void OnEnable() {
            if (inst != null && inst != this) { 
                if (gameObject != null)
                    Destroy(gameObject);
                return;
            }
            inst = this as T;

            transform.parent = null; // Unparent
            DontDestroyOnLoad(gameObject);
            IsInitialized = true;
            
            OnSingletonEnable();
        }

        protected void OnDisable() {
            if (IsInitialized) {
                OnSingletonDisable();
                inst = null;
            }
        }

        // Force intitialize singleton
        public static void Initialize() {
            _ = Inst;
        }

        // Override these to ensure the singleton is initialized correctly
        protected virtual void OnSingletonEnable() { }
        protected virtual void OnSingletonDisable() { }
    }

}