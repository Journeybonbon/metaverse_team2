using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Switch1 : MonoBehaviour
{
    public string playscene = "playscene";
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
        if (scene.name == "SpaceshipUp")
        {
            StartCoroutine(LoadNextSceneAfterDelay(3f));
        }
        else if (scene.name == "playscene")
        {
            Debug.Log("playscene으로 전환되었습니다.");
        }
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(playscene);
    }
}