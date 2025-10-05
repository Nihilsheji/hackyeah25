using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSceneLoader : MonoBehaviour
{
    [SerializeField] private string persistentSceneName = "PersistentScene";

    void Awake()
    {
        // Check if the scene is already loaded
        Scene persistentScene = SceneManager.GetSceneByName(persistentSceneName);

        if (!persistentScene.isLoaded)
        {
            // Load it additively so it doesn't replace the current scene
            SceneManager.LoadScene(persistentSceneName, LoadSceneMode.Additive);
            Debug.Log($"[PersistentSceneLoader] Loaded scene: {persistentSceneName}");
        }
        else
        {
            Debug.Log($"[PersistentSceneLoader] Scene already loaded: {persistentSceneName}");
        }
    }
}
