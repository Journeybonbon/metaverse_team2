using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Switch : MonoBehaviour
{
    public string CCScene = "CCScene"; // CCScene ���� �̸��� �Ҵ����ݴϴ�.

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
        if (scene.name == "SampleScene")
        {
            StartCoroutine(LoadNextSceneAfterDelay(6f));
        }
        else if (scene.name == "CCScene")
        {
            Debug.Log("CCScene���� ��ȯ�Ǿ����ϴ�.");
        }
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(CCScene);
    }
}