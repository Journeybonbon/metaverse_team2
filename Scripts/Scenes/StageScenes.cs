using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageScenes : MonoBehaviour
{
    public string StageScene = "StageScene";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "playscene")
        {
            StartCoroutine(LoadNextSceneAfterDelay(3f));
        }
        else if (scene.name == "StageScene")
        {
            Debug.Log("StageScene으로 전환되었습니다.");
        }
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(StageScene);
    }
}