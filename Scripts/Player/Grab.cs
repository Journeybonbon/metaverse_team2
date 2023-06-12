using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class Grab : MonoBehaviour
{

    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private LayerMask TreasureLayer;

    private PlayerMove playerMove;
    public Transform crosshair;

    public GameObject canv;
    public GameObject audioObj;
    AudioSource _audio;
    //Transform player;

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection * maxDistance);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        _audio = audioObj.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ARAVRInput.DrawCrosshair(crosshair, true, ARAVRInput.Controller.LTouch);

        RaycastHit hitInfo;
        Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
        int playerlayer = 1 << LayerMask.NameToLayer("player");

        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.LTouch))
        {
            //if (Physics.Raycast(ray, out hitInfo, maxDistance, itemLayer))
            if (Physics.Raycast(ray, out hitInfo, 200, ~playerlayer))
            {
                if (hitInfo.transform.name.Contains("Item"))
                {
                    Debug.Log("È¸º¹µÊ");
                    if(playerMove.HP + 20 <= 100)
                    {
                        playerMove.HP += 20;
                    }
                    else
                    {
                        playerMove.HP = 100;
                    }
                    _audio.Play();
                    Destroy(hitInfo.collider.gameObject);

                }
                //else if (Physics.Raycast(ray, out hitInfo, maxDistance, TreasureLayer))
                else if (hitInfo.transform.name.Contains("Treasure"))
                {
                    playerMove.clearFlag = true;
                    canv.SetActive(true);
                    Debug.Log("È¹µæ");
                    _audio.Play();
                    Destroy(hitInfo.collider.gameObject);
                }
            }
        }
    }
}
