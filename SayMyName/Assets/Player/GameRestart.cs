using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    private bool hasRestarted;

    [Button]
    public void RestartGame()
    {
        if (hasRestarted == true)
            return;

        hasRestarted = true;

        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Load the scene additively
        SceneManager.LoadSceneAsync(currentScene.name, LoadSceneMode.Additive).completed += (AsyncOperation op) =>
        {
            // Once the new instance is loaded, set it as active
            Scene newScene = SceneManager.GetSceneByName(currentScene.name);
            SceneManager.SetActiveScene(newScene);

            // Unload the old scene
            SceneManager.UnloadSceneAsync(currentScene);
        };
    }
}
