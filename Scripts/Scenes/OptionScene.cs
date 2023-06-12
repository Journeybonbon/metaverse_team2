using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionScene : MonoBehaviour
{
    public GameObject optCanv;
    public GameObject mainCanv;

    public GameObject main;
    public GameObject inGame;
    public GameObject option;

    private void Start()
    {
        optCanv.SetActive(false);
    }
    public void ClickOpt()
    {
        main.SetActive(false);
        option.SetActive(true);
        Debug.Log("set!");
        StartCoroutine(OpotionProcess());
    }

    IEnumerator OpotionProcess()
    {
        yield return new WaitForSeconds(3f);
        option.SetActive(false);
        mainCanv.SetActive(false);
        optCanv.SetActive(true);
    }

    public void CloseOpt()
    {
        optCanv.SetActive(false);
        mainCanv.SetActive(true);
        inGame.SetActive(true);
        StartCoroutine(CloseProcess());

    }

    IEnumerator CloseProcess()
    {
        yield return new WaitForSeconds(3f);
        inGame.SetActive(false);
        main.SetActive(true);
    }
}