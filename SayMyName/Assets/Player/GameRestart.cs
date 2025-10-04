using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

public class GameRestart : MonoBehaviour
{
    private bool hasRestarted;

    [Button]
    public async void RestartGame()
    {
        if (hasRestarted)
            return;

        hasRestarted = true;

        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        // Unload the scene first
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
        if (unloadOp != null)
        {
            while (!unloadOp.isDone)
                await Task.Yield(); // Wait for unload to finish
        }

        // Then load it again
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        if (loadOp != null)
        {
            while (!loadOp.isDone)
                await Task.Yield(); // Wait for load to finish
        }

        hasRestarted = false;
    }
}
