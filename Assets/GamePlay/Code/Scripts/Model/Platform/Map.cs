using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadMap()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void LoadNextMap()
    {
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int currScenes = SceneManager.GetActiveScene().buildIndex;
        if (currScenes < totalScenes)
        {
            currScenes++;
        }
        SceneManager.LoadScene(currScenes);
    }
}
