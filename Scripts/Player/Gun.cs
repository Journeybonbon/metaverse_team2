using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform crosshair;

    [Header("BulletEffects")]
    public GameObject[] bulletEffects;
    ParticleSystem bulletEffect;
    ParticleSystem MonsterEffect; //monster bulletEffect
    AudioSource bulletAudio;

    // Update is called once per frame
    void Start()
    {
        bulletEffect = bulletEffects[0].GetComponent<ParticleSystem>();
        MonsterEffect = bulletEffects[1].GetComponent<ParticleSystem>();
        bulletAudio = bulletEffects[0].GetComponent<AudioSource>();
    }

    void Update()
    {
        ARAVRInput.DrawCrosshair(crosshair);
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
        {
            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);

            ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);

            bulletAudio.Stop();
            bulletAudio.Play();

            RaycastHit hitInfo;
            int playerlayer = 1 << LayerMask.NameToLayer("player");
            int layerMask = playerlayer;

            if (Physics.Raycast(ray, out hitInfo, 200, ~layerMask))
            {
                if (hitInfo.transform.name.Contains("Monster"))
                {
                    Monster monster = hitInfo.transform.GetComponent<Monster>();
                    monster.HitMonster(5);
                    //bulletEffects[1].transform.forward = hitInfo.normal;
                    bulletEffects[1].transform.position = hitInfo.point;
                    MonsterEffect.Play();
                }
                else
                {
                    bulletEffects[0].transform.position = hitInfo.point;
                    //bulletEffect.Stop();
                    bulletEffect.Play();

                }
            }
        }
    }
}
