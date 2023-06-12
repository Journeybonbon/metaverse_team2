using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailSwitcher : MonoBehaviour
{
    public string FailScene = "FailScene"; // CCScene 씬의 이름을 할당해줍니다.

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
        if (scene.name == "EndScene(Fail)")
        {
            StartCoroutine(LoadNextSceneAfterDelay(3f));
        }
        else if (scene.name == "FailScene")
        {
            Debug.Log("ClearScene으로 전환되었습니다.");
        }
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(FailScene);
    }
}
