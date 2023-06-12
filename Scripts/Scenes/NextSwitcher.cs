using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSwitcher : MonoBehaviour
{
    public void Change()
    {
        SceneManager.LoadScene("SpaceshipUp");
    }
}