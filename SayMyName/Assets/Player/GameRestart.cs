using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Sirenix.OdinInspector;

public class GameRestart : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "Scene lvl"; // The scene you want to reload
    private bool isRestarting;

    [Button]
    public void RestartGame()
    {
        Debug.Log("Restart");
        if (isRestarting)
            return;

        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        isRestarting = true;

        Debug.Log("AAAABUG1");

        // Check if the scene is loaded
        Scene targetScene = SceneManager.GetSceneByName(targetSceneName);
        if (targetScene.isLoaded)
        {
            Debug.Log("AAAABUG2");

            // Unload the target scene first
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(targetScene);
            yield return unloadOp;
        }

        Debug.Log("AAAABUG3");

        // Load the scene again additively
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
        yield return loadOp;
        Debug.Log("AAAABUG4");

        // Optionally set it active again
        Scene newScene = SceneManager.GetSceneByName(targetSceneName);
        SceneManager.SetActiveScene(newScene);
        Debug.Log("AAAABUG5");

        int sceneCount = SceneManager.sceneCount;
        Debug.Log($"Loaded Scenes ({sceneCount}):");

        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            Debug.Log($"[{i}] {scene.name} (isLoaded: {scene.isLoaded})");
        }

        isRestarting = false;
    }
}
