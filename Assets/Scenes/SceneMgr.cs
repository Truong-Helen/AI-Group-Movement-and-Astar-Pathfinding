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
        Debug.Log(spawnAmount);
        SceneManager.LoadScene("Circle20EnvironmentPart1");
    }
    public void ChangeSceneTo30CirclesEnvironmentPart1()
    {
        spawnAmount = 30;
        SceneManager.LoadScene("Circle30EnvironmentPart1");
    }
    public void ChangeSceneTo100CirclesEnvironmentPart1()
    {
        spawnAmount = 100;
        SceneManager.LoadScene("Circle100EnvironmentPart1");
    }

    // Rectangle Environment
    public void ChangeSceneTo20RectanglesEnvironmentPart1()
    {
        spawnAmount = 20;
        SceneManager.LoadScene("Rectangle20EnvironmentPart1");
    }
    public void ChangeSceneTo30RectanglesEnvironmentPart1()
    {
        spawnAmount = 30;
        SceneManager.LoadScene("Rectangle30EnvironmentPart1");
    }
    public void ChangeSceneTo100RectanglesEnvironmentPart1()
    {
        spawnAmount = 100;
        SceneManager.LoadScene("Rectangle100EnvironmentPart1");
    }

    // H Environment
    public void ChangeSceneToHEnvironmentPart1()
    {
        SceneManager.LoadScene("HEnvironmentPart1");
    }

    // Office Environment
    public void ChangeSceneToOfficeEnvironmentPart1()
    {
        SceneManager.LoadScene("OfficeEnvironmentPart1");
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
        SceneManager.LoadScene("Circle20EnvironmentPart2");
    }
    public void ChangeSceneTo30CirclesEnvironmentPart2()
    {
        spawnAmount = 30;
        SceneManager.LoadScene("Circle30EnvironmentPart2");
    }
    public void ChangeSceneTo100CirclesEnvironmentPart2()
    {
        spawnAmount = 100;
        SceneManager.LoadScene("Circle100EnvironmentPart2");
    }

    // Rectangle Environment
    public void ChangeSceneTo20RectanglesEnvironmentPart2()
    {
        spawnAmount = 20;
        SceneManager.LoadScene("Rectangle20EnvironmentPart2");
    }
    public void ChangeSceneTo30RectanglesEnvironmentPart2()
    {
        spawnAmount = 30;
        SceneManager.LoadScene("Rectangle30EnvironmentPart2");
    }
    public void ChangeSceneTo100RectanglesEnvironmentPart2()
    {
        spawnAmount = 100;
        SceneManager.LoadScene("Rectangle100EnvironmentPart2");
    }

    // H Environment
    public void ChangeSceneToHEnvironmentPart2()
    {
        SceneManager.LoadScene("HEnvironmentPart2");
    }

    // Office Environment
    public void ChangeSceneToOfficeEnvironmentPart2()
    {
        SceneManager.LoadScene("OfficeEnvironmentPart2");
    }

    // ----------------------------------- PART 3 ----------------------------------- //
    public void ChangeSceneToEnvironmentSelectPart3()
    {
        SceneManager.LoadScene("EnvironmentSelectPart3");
    }

    // Circle Environment
    public void ChangeSceneTo20CirclesEnvironmentPart3()
    {
        spawnAmount = 20;
        SceneManager.LoadScene("CircleEnvironmentPart3");
    }
    public void ChangeSceneTo30CirclesEnvironmentPart3()
    {
        spawnAmount = 30;
        SceneManager.LoadScene("CircleEnvironmentPart3");
    }
    public void ChangeSceneTo100CirclesEnvironmentPart3()
    {
        spawnAmount = 100;
        SceneManager.LoadScene("CircleEnvironmentPart3");
    }

    // Rectangle Environment
    public void ChangeSceneTo20RectanglesEnvironmentPart3()
    {
        spawnAmount = 20;
        SceneManager.LoadScene("RectangleEnvironmentPart3");
    }
    public void ChangeSceneTo30RectanglesEnvironmentPart3()
    {
        spawnAmount = 30;
        SceneManager.LoadScene("RectangleEnvironmentPart3");
    }
    public void ChangeSceneTo100RectanglesEnvironmentPart3()
    {
        spawnAmount = 100;
        SceneManager.LoadScene("RectangleEnvironmentPart3");
    }

    // H Environment
    public void ChangeSceneToHEnvironmentPart3()
    {
        SceneManager.LoadScene("HEnvironmentPart3");
    }

    // Office Environment
    public void ChangeSceneToOfficeEnvironmentPart3()
    {
        SceneManager.LoadScene("OfficeEnvironmentPart3");
    }

    // ----------------------------------- PART 4 ----------------------------------- //
    public void ChangeSceneToEnvironmentSelectPart4()
    {
        SceneManager.LoadScene("EnvironmentSelectPart4");
    }

    // Circle Environment
    public void ChangeSceneTo20CirclesEnvironmentPart4()
    {
        spawnAmount = 20;
        SceneManager.LoadScene("CircleEnvironmentPart4");
    }
    public void ChangeSceneTo30CirclesEnvironmentPart4()
    {
        spawnAmount = 30;
        SceneManager.LoadScene("CircleEnvironmentPart4");
    }
    public void ChangeSceneTo100CirclesEnvironmentPart4()
    {
        spawnAmount = 100;
        SceneManager.LoadScene("CircleEnvironmentPart4");
    }

    // Rectangle Environment
    public void ChangeSceneTo20RectanglesEnvironmentPart4()
    {
        spawnAmount = 20;
        SceneManager.LoadScene("RectangleEnvironmentPart4");
    }
    public void ChangeSceneTo30RectanglesEnvironmentPart4()
    {
        spawnAmount = 30;
        SceneManager.LoadScene("RectangleEnvironmentPart4");
    }
    public void ChangeSceneTo100RectanglesEnvironmentPart4()
    {
        spawnAmount = 100;
        SceneManager.LoadScene("RectangleEnvironmentPart4");
    }

    // H Environment
    public void ChangeSceneToHEnvironmentPart4()
    {
        SceneManager.LoadScene("HEnvironmentPart4");
    }

    // Office Environment
    public void ChangeSceneToOfficeEnvironmentPart4()
    {
        SceneManager.LoadScene("OfficeEnvironmentPart4");
    }
}
