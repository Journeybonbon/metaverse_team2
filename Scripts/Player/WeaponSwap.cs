using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public GameObject[] weapon;

    private int index = 0;

    public GameObject crosshair;

    void Start()
    {
        InitWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.One))
        {
            index++;
            if(index >= weapon.Length)
            {
                index = 0;
            }
            SwitchWeapons(index);

        }
    }
    private void InitWeapon()
    {
        for(int i =0; i< weapon.Length;i++)
        {
            weapon[i].SetActive(false);
        }
        weapon[0].SetActive(true);
        index = 0;
    }
    private void SwitchWeapons(int newIndex)
    {
        for(int i=0; i< weapon.Length; i++)
        {
            weapon[i].SetActive(false);
        }
        weapon[newIndex].SetActive(true);
        if (newIndex == 0)
        {
            crosshair.SetActive(true);
        }
        else if (newIndex == 1)
        {
            crosshair.SetActive(false);
        }
    }
}
