using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr inst;
    private void Awake()
    {
        inst = this; 
    }

    public int spawnAmount = 0;

    public void ChangeSceneToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }



    public void ChangeSceneToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }

    // ----------------------------------- PART 1 ----------------------------------- //
    public void ChangeSceneToEnvironmentSelectPart1()
    {
        SceneManager.LoadScene("EnvironmentSelectPart1");
    }

    // Circle Environment
    public void ChangeSceneTo20CirclesEnvironmentPart1()
    {
        spawnAmount = 20;
        SceneManager.LoadScene("CircleEnvironmentPart1");
    }
    public void ChangeSceneTo30CirclesEnvironmentPart1()
    {
        spawnAmount = 30;
        SceneManager.LoadScene("CircleEnvironmentPart1");
    }
    public void ChangeSceneTo100CirclesEnvironmentPart1()
    {
        spawnAmount = 100;
        SceneManager.LoadScene("CircleEnvironmentPart1");
    }

    // ----------------------------------- PART 2 ----------------------------------- //
    public void ChangeSceneToEnvironmentSelectPart2()
    {
        SceneManager.LoadScene("EnvironmentSelectPart2");
    }

    // Circle Environment
    public void ChangeSceneTo20CirclesEnvironmentPart2()
    {
        spawnAmount = 20;
        SceneManager.LoadScene("CircleEnvironmentPart2");
    }
    public void ChangeSceneTo30CirclesEnvironmentPart2()
    {
        spawnAmount = 30;
        SceneManager.LoadScene("CircleEnvironmentPart2");
    }
    public void ChangeSceneTo100CirclesEnvironmentPart2()
    {
        spawnAmount = 100;
        SceneManager.LoadScene("CircleEnvironmentPart2");
    }
}
