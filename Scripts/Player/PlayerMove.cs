using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using static GroundCheck;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerMove : MonoBehaviour
{
    [Header("Transform References")]
    [SerializeField] private Transform movementOrientation;
    [SerializeField] private Transform characterMesh;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float gravitationalAcceleration;
    [SerializeField] private float jumpForce;


    private float horizontalInput;
    private float verticalInput;
    private bool jumpFlag;
    private bool canJump;
    public bool clearFlag;
    [Space(10.0f)]
    [SerializeField, Range(0.0f, 1.0f)] private float lookForwardThreshold;
    [SerializeField] private float lookForwardSpeed;

    private CharacterController m_characterController;
    private GroundCheck m_groundChecker;
    private Vector3 velocity;

    Transform spaceship;

    [Header("HitEffect")]
    public GameObject hitEffect;
    public Image hitImg;
    public GameObject fadeEffect;
    public Image fadeImage;
    public float duration = 1f;
    public float duration_hp = 0.5f;
    public float fadeSpeed = 0.5f;
    private float durationTimer_hp;
    private float durationTimer;

    [Header("hp")]
    public Slider hpSlider;
    public TextMeshProUGUI hpText;


    [Header("UI")]
    public GameObject canv;
    public TextMeshProUGUI timerText;
    public int timer = 300; //5Ка

    public float initialHP = 100;
    float _hp;

    public float fade_hp = 0.05f;

    public float HP
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }


    void Start()
    {
        _hp = initialHP;
        m_characterController = GetComponent<CharacterController>();
        m_groundChecker = GetComponent<GroundCheck>();
        velocity = new Vector3(0, 0, 0);

        horizontalInput = 0.0f;
        verticalInput = 0.0f;
        jumpFlag = false;
        canJump = true;
        clearFlag = false;

        hitEffect.SetActive(true);
        hitImg.color = new Color(hitImg.color.r, hitImg.color.g, hitImg.color.b, 0);
        fadeEffect.SetActive(true);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);

        spaceship = GameObject.Find("SpaceshipDiffuse").transform;

        StartCoroutine(TimerCoroutine());
    }

    void Update()
    {
        //if (!IsOwner) return;

        horizontalInput = ARAVRInput.GetAxis("Horizontal");
        verticalInput = ARAVRInput.GetAxis("Vertical");

        if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch) && canJump)
        {
            jumpFlag = true;
            canJump = false;
        }

        if (hitImg.color.a > 0)
        {
            if (_hp >= 30)
            {
                durationTimer_hp += Time.deltaTime;
                if (durationTimer_hp > duration_hp)
                {
                    float tempAlpha = hitImg.color.a;
                    tempAlpha -= Time.deltaTime * fadeSpeed;
                    hitImg.color = new Color(hitImg.color.r, hitImg.color.g, hitImg.color.b, tempAlpha);
                }
            }
        }

        if (clearFlag == true && Vector3.Distance(transform.position, spaceship.position) < 90)
        {
            Debug.Log("CLEAR");
            canv.SetActive(false);
            if (fadeImage.color.a < 1)
            {
                durationTimer += Time.deltaTime;
                if (durationTimer > duration)
                {
                    float tempAlpha = fadeImage.color.a;
                    tempAlpha += Time.deltaTime * fadeSpeed;
                    fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, tempAlpha);
                }
            }
            else
            {
                durationTimer += Time.deltaTime;
                if (durationTimer > 5)
                {
                    SceneManager.LoadScene("EndScene(Claer)");
                }
            }
        }

        _hp -= Time.deltaTime * fade_hp;

        hpSlider.value = _hp / initialHP;
        hpText.text = Mathf.CeilToInt(_hp).ToString();

        if (_hp <= 0 || timer <= 0)
        {
            canv.SetActive(false);
            if (fadeImage.color.a < 1)
            {
                durationTimer += Time.deltaTime;
                if (durationTimer > duration)
                {
                    float tempAlpha = fadeImage.color.a;
                    tempAlpha += Time.deltaTime * fadeSpeed;
                    fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, tempAlpha);
                }
            }
            else
            {
                durationTimer += Time.deltaTime;
                if(durationTimer > 5)
                {
                    SceneManager.LoadScene("FailScene");
                }
            }
            
        }
    }

    private void FixedUpdate()
    {
        //if (!IsOwner) return;

        Vector3 planeVelocity = GetXZVelocity(horizontalInput, verticalInput);
        float yVelocity = GetYVelocity(jumpFlag);
        velocity = new Vector3(planeVelocity.x, yVelocity, planeVelocity.z);
        jumpFlag = false;

        m_characterController.Move(velocity * speed * Time.fixedDeltaTime);
    }

    private Vector3 GetXZVelocity(float horizontalInput, float verticalInput)
    {
        Vector3 moveVelocity = movementOrientation.forward * verticalInput + movementOrientation.right * horizontalInput;
        Vector3 moveDirection = moveVelocity.normalized;
        float moveSpeed = Mathf.Min(moveVelocity.magnitude, 1.0f) * speed;

        return moveDirection * moveSpeed;
    }

    private float GetYVelocity(bool jumpFlag)
    {
        float Yvelocity = velocity.y - gravitationalAcceleration * Time.fixedDeltaTime;
        if (!m_groundChecker.IsGrounded() && !jumpFlag)
        {

            if (Yvelocity < 0.0f)
            {
                canJump = true;
                return Mathf.Max(0.0f, Yvelocity);
            }
            else
            {
                return Yvelocity;
            }
            //return velocity.y - gravitationalAcceleration * Time.fixedDeltaTime;
        }
        else if (jumpFlag)
        {
            return velocity.y + jumpForce;
        }
        else
        {
            canJump = true;
            return Mathf.Max(0.0f, Yvelocity);
        }

    }

    /*private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody)
        {
            hit.rigidbody.AddForce(velocity / hit.rigidbody.mass);
        }
        else if (hit.collider)
        {

        }
    }*/

    public void DamageAction(int damage)
    {
        _hp -= damage;
        if (_hp > 0)
        {
            durationTimer_hp = 0;
            hitImg.color = new Color(hitImg.color.r, hitImg.color.g, hitImg.color.b, 1);
        }
        else if (_hp <= 0)
        {
            _hp = 0;
        }
        Debug.Log(_hp);
    }

    IEnumerator TimerCoroutine()
    {
        timer -= 1;
        if(timer <= 10)
        {
            timerText.color = new Color(255, 0, 0);
        }

        if(timer <= 0)
        {
            timer = 0;
        }
        timerText.text = (timer / 60 % 60).ToString("D2") + ":" + (timer % 60).ToString("D2");
        yield return new WaitForSeconds(1f);
        StartCoroutine(TimerCoroutine());
    }
}
