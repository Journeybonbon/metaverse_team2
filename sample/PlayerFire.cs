using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    enum WeaponMode //무기 종류
    {
        Normal,
        Sniper
    }
    WeaponMode wMode; //현재 무기 상태
    bool ZoomMode = false; //줌인 상태
    public GameObject eff_Flash; // 사격 시  나타나는 효과
    public Text wModeText; // 무기 상태 텍스트
    public Text bulletCntText; // 현재 총알 수 텍스트
    public GameObject[] weapons; // 무기 관련 에셋/crosshair 저장한 배열
    public GameObject crosshair02_zoom; //줌인 시 UI
    public GameObject[] bulletEffects;
    public SceneLoad script;
    ParticleSystem ps; // 일반 오브젝트 사격 효과
    ParticleSystem es; // 몬스터 사격 효과
    public int weaponPower = 5;

    public GameObject[] audioObj;
    AudioSource shot; // 총 발사 시 오디오
    AudioSource load; // 재장전 시 오디오
    public int currentBulletCnt; // 현재 총알 개수

    private bool isReload = false; // 재장전 상태 표시

    Animator anim;

    void Start()
    {
        // 오브젝트와 컴포넌트 설정
        ps = bulletEffects[0].GetComponent<ParticleSystem>();
        es = bulletEffects[1].GetComponent<ParticleSystem>();
        anim = transform.GetComponentInChildren<Animator>();
        wMode = WeaponMode.Normal; // 초기 모드는 일반 모드
        shot = audioObj[0].GetComponent<AudioSource>();
        load = audioObj[1].GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(script.Canv.activeSelf == false)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1)) //'1'을 누르면 
            {

                wMode = WeaponMode.Normal; //일반 모드로 변경
                if(wModeText.text != "Normal Mode")
                {
                    load.Play();
                    Camera.main.fieldOfView = 60f; // 줌 아웃
                    wModeText.text = "Normal Mode";
                    weapons[0].SetActive(true); // 일반 모드 에셋
                    weapons[1].SetActive(false); // 스나이퍼 에셋
                    weapons[2].SetActive(true); // 일반 모드 crosshair
                    weapons[3].SetActive(false); // 스나이퍼 줌 아웃 crosshair
                    crosshair02_zoom.SetActive(false); // 스나이퍼 줌인 crosshair
                    Camera.main.fieldOfView = 60f;
                    ZoomMode = false;
                }

            }
            else if(Input.GetKeyDown(KeyCode.Alpha2)) // '2'를 누르면
            {

                wMode = WeaponMode.Sniper; //스나이퍼 모드로 변경
                if(wModeText.text != "Sniper Mode")
                {   
                    load.Play(); // 오디오 재생
                    wModeText.text = "Sniper Mode"; // UI의 텍스트 변경

                    // 무기 관련 에셋,ui 변경
                    weapons[0].SetActive(false);
                    weapons[1].SetActive(true);
                    weapons[2].SetActive(false);
                    weapons[3].SetActive(true);
                }

            }

            if(Input.GetMouseButtonDown(1)) //마우스 우클릭 - 줌인/줌아웃
            {
                switch(wMode)
                {
                    // 일반 모드에서는 줌인/줌아웃 적용 x
                    case WeaponMode.Normal:
                        break;
                    
                    //스나이퍼 모드일 경우
                    case WeaponMode.Sniper: 
                        if(!ZoomMode)
                        {
                            // 줌아웃 상태면 줌인으로 변경
                            Camera.main.fieldOfView = 15f; // 화면 확대
                            ZoomMode = true;
                            crosshair02_zoom.SetActive(true);
                            weapons[3].SetActive(false);
                        }
                        else
                        {
                            // 줌인 상태면 줌 아웃으로 변경
                            Camera.main.fieldOfView = 60f;
                            ZoomMode = false;
                            crosshair02_zoom.SetActive(false);
                            weapons[3].SetActive(true);
                        }
                        break;
                }
                
            }
            else if(Input.GetMouseButtonDown(0) && currentBulletCnt > 0){
                //총알이 있고, 사격할 때
                currentBulletCnt -= 1; // 총알 감소
                bulletCntText.text = "10 / " + currentBulletCnt.ToString(); // 남은 총알 수를 나타내는 텍스트
                anim.SetTrigger("GuardToShoot"); // 플레이어 애니메이션 설정
                shot.Play(); // 오디오 재생

                StartCoroutine(ShootEffectOn(0.05f));
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                RaycastHit hitInfo = new RaycastHit();
                //Vector2 reboundRay = Random.insideUnitCircle * SingletoneValue.Instance.rebound * 0.033f;    //반동이 있다면 반동만큼의 크기를 가진 원의 범위 내에서 랜덤값을 가진다.
                anim.SetTrigger("ShootToGuard");
                
                if(Physics.Raycast(ray, out hitInfo))
                {
                    if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        //몬스터가 총을 맞은 경우
                        EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();

                        if(wMode == WeaponMode.Sniper)
                        {
                            eFSM.HitEnemy(5); // 스나이퍼 모드에서 공격하면 데미지 5
                        }
                        else
                        {
                            eFSM.HitEnemy(weaponPower); // 일반 모드에서 공격하면 데미지 3
                        }

                        bulletEffects[1].transform.position = hitInfo.point; // 총알 효과
                        es.Play(); // 몬스터가 맞았을 때 피가 보이는 효과
                    }
                    else
                    {
                    bulletEffects[0].transform.position = hitInfo.point;
                    ps.Play(); // 일반 오브젝트에 조준하면 먼지만 나타남
                    }
                }
            }
            TryReload();
        }
    }

    IEnumerator ShootEffectOn(float duration)
    {
        eff_Flash.SetActive(true); // 몬스터가 맞으면 피격 효과 재생
        yield return new WaitForSeconds(duration);
        eff_Flash.SetActive(false);
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentBulletCnt < 10) // 재장전 버튼을 누르면
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine() // 재장전
    {
            isReload = true;
            load.Play(); // 오디오 재생
            bulletCntText.text = "10 / 10";

            // 연속으로 재장전 시도해도 특정 시간 동안은 안 되도록 제한
            yield return new WaitForSeconds(0.01f);
            currentBulletCnt = 10;

            isReload = false;
    }
}
