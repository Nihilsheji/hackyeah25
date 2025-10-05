using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Sirenix.OdinInspector;

public class GameRestart : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "GameScene"; // The scene you want to reload
    private bool isRestarting;

    [Button]
    public void RestartGame()
    {
        if (isRestarting)
            return;

        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        isRestarting = true;

        // Check if the scene is loaded
        Scene targetScene = SceneManager.GetSceneByName(targetSceneName);
        if (targetScene.isLoaded)
        {
            // Unload the target scene first
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(targetScene);
            yield return unloadOp;
        }

        // Load the scene again additively
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
        yield return loadOp;

        // Optionally set it active again
        Scene newScene = SceneManager.GetSceneByName(targetSceneName);
        SceneManager.SetActiveScene(newScene);

        isRestarting = false;
    }
}
