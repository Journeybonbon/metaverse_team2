using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // public float moveSpeed = 7f;
    // private Rigidbody rb;
    // CharacterController cc;
    // float gravity = -20f; // 중력변수
    // float yVelocity = 0; // 수직 속력 변수
    // public float jumpPower = 7f; // 점프력 변수
    // public bool isJumping = false;
    public int hp = 100;// 플레이어 체력 변수
    int maxHP = 100; // 플레이어의 최대 체력
    public Slider hpSlider; // Hp를 표시하는 UI Slider
    public GameObject hitEffect; // 공격 받을 때의 효과
    public Text hpText; // HP 표시 텍스트

    public GameObject[] Canvs; // 다른 화면을 표시할 UI 저장 배열
    public Image fadeImage;

    public Image hitImg;
    public float duration;
    public float fadeSpeed;
    private float durationTimer;

    // public int cnt = 10;
    // AudioSource audioSoure;
    // public GameObject[] audioObj;
    // private bool isPlay = false;
    // AudioSource audioEnd;
    // AudioSource audioItem;
    // Animator anim;
    void Start()
    {
        // cc = GetComponent<CharacterController>();
        anim = transform.GetComponentInChildren<Animator>();
        hitEffect.SetActive(true);
        hitImg.color = new Color(hitImg.color.r, hitImg.color.g, hitImg.color.b, 0);
        Canvs[2].SetActive(true);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        // rb = GetComponent<Rigidbody>();
        // audioSoure = GetComponent<AudioSource>();
        // audioEnd = audioObj[0].GetComponent<AudioSource>();
        // audioItem = audioObj[1].GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        // float h = Input.GetAxis("Horizontal");
        // float v = Input.GetAxis("Vertical");
        // if(h != 0 || v != 0)
        // {
        //     anim.SetBool("Run", true);
        //     audioSoure.Play();
        // }
        // else
        // {
        //     anim.SetBool("Run", false);
        // }
        // h = h * moveSpeed * Time.deltaTime;
        // v = v * moveSpeed * Time.deltaTime;

        // Vector3 dir = new Vector3(h, 0, v);
        // dir = dir.normalized;

        // // 메인 카메라를 기준으로 방향 변경
        // dir = Camera.main.transform.TransformDirection(dir);

        // transform.position += dir * moveSpeed * Time.deltaTime;

        // if(isJumping && cc.collisionFlags == CollisionFlags.Below)
        // {
        //     isJumping = false;
        //     yVelocity = 0;
        // }

        // if(Input.GetButtonDown("Jump") && !isJumping)
        // {
        //     yVelocity = jumpPower;
        //     isJumping = true;
        // }
        // yVelocity += gravity * Time.deltaTime;
        // dir.y = yVelocity;
        // cc.Move(dir * moveSpeed * Time.deltaTime);

        //HP 표시
        // UIControl.cs에는 canvas관련된 코드만 있습니다!
        hpSlider.value = (float)hp / (float)maxHP;
        hpText.text = hp.ToString();

        // 공격 받았을 때 이미지를 Fade out으로 표현하기
        if (hitImg.color.a > 0)
        {
            if(hp >= 30)
            {
                durationTimer += Time.deltaTime;
                if(durationTimer > duration)
                {
                    float tempAlpha = hitImg.color.a;
                    tempAlpha -= Time.deltaTime * fadeSpeed;
                    hitImg.color = new Color(hitImg.color.r, hitImg.color.g, hitImg.color.b, tempAlpha);
                }
            }

        }
        // if (cnt <= 0) // 게임 종료 조건
        // {
        //     Canvs[3].SetActive(false);
        //     if(isPlay == false)
        //     {
        //         audioEnd.Play();
        //         isPlay = true;
        //     }
        //     if (fadeImage.color.a < 1)
        //     {
        //         durationTimer += Time.deltaTime;
        //         if(durationTimer > duration)
        //         {
        //             float tempAlpha = fadeImage.color.a;
        //             tempAlpha += Time.deltaTime * fadeSpeed;
        //             fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, tempAlpha);
        //         }
        //     }
        //     else
        //     {
        //         Canvs[1].SetActive(true);
        //     }
        // }
    }

    public void DamageAction(int damage) // 플레이어가 공격 받았을 때
    {
        hp -= damage; // hp 감소
        if(hp > 0)
        {
            durationTimer = 0;
            // 공격 받았을 때 UI 효과
            hitImg.color = new Color(hitImg.color.r, hitImg.color.g, hitImg.color.b, 1);
        }
        if(hp <= 0)
        {
            // 플레이어 체력에 0 이하이므로 게임 종료
            hp = 0;
            anim.SetTrigger("Die");
            Canvs[0].SetActive(true);
        }
        print(hp);
    }

    void OnTriggerEnter(Collider other) // 아이템을 섭취하면 (아이템에 다가가면)
    {
		if (other.gameObject.CompareTag("Item")) //닿은 오브젝트가 "Item"이라는 오브젝트라면
        {
            // audioItem.Play(); 오디오 재생
			other.gameObject.SetActive(false); // 아이템 사라지게
            
            // 체력 회복
            if(hp + 15 > 100)
            {
                hp = 100;
            }
            else
            {
                hp += 15;
            }
        }
		
    }
}

