using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearSwitcher : MonoBehaviour
{
    public string ClearScene = "ClearScene"; // CCScene ���� �̸��� �Ҵ����ݴϴ�.

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
        if (scene.name == "EndScene(Claer)")
        {
            StartCoroutine(LoadNextSceneAfterDelay(3f));
        }
        else if (scene.name == "ClearScene")
        {
            Debug.Log("ClearScene���� ��ȯ�Ǿ����ϴ�.");
        }
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(ClearScene);
    }
}
