using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{

    public GameObject canv;
    public GameObject crosshair;

    // Update is called once per frame
    void Update()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            crosshair.SetActive(false);
            canv.SetActive(true);
        }

        if (ARAVRInput.GetUp(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            canv.SetActive(false);
            crosshair.SetActive(true);
        }
    }
}
