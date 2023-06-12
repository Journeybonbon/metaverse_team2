using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Monster : MonoBehaviour
{
    enum MonsterState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }

    CharacterController cc;

    public float findDistance = 20f; // 플레이어 발견 범위
    Transform player; // 플레이어 위치
    NavMeshAgent agent;
    public float attackDistance = 5f; // 플레이어 공격 범위
    public float moveSpeed = 4f; // 몬스터 이동 속도

    MonsterState m_State = MonsterState.Idle;

    float attackDelay = 3f;
    float currentTime = 0;
    bool ishitted = false;

    public Slider hpslider;

    Animator anim;

    [SerializeField]
    public int maxhp = 30;
    int hp;

    public GameObject canv;

    public GameObject audioHitobj;
    AudioSource audioHit;
    public GameObject audioDieobj;
    AudioSource audioDie;
    public GameObject audioAtkobj;
    AudioSource audioAtk;
    public GameObject audioShtobj;
    AudioSource audioSht;

    private ItemMaker factory;

    // Start is called before the first frame update
    void Start()
    {
        hp = 30;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        agent.speed = moveSpeed;
        anim = transform.GetComponentInChildren<Animator>();
        audioHit = audioHitobj.GetComponent<AudioSource>();
        audioDie = audioDieobj.GetComponent<AudioSource>();
        audioAtk = audioAtkobj.GetComponent<AudioSource>();
        audioSht = audioShtobj.GetComponent<AudioSource>();
        factory = GameObject.Find("ItemMaker").GetComponent<ItemMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case MonsterState.Idle:
                Idle();
                break;

            case MonsterState.Move:
                Move();
                break;

            case MonsterState.Attack:
                Attack();
                break;

            case MonsterState.Damaged:
                break;

            case MonsterState.Die:
                break;
        }
        hpslider.value = (float)hp / (float)maxhp;
    }

    void Idle()
    {
        // when the monster finds a player
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = MonsterState.Move;

            //agent.enabled = true;
            print("상태 전환 : Idle -> Move");
            anim.SetTrigger("Run");
        }
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            agent.enabled = true;
            agent.SetDestination(player.position);
            // AI NAV bake 해야함
        }
        else
        {
            m_State = MonsterState.Attack;
            currentTime = attackDelay;
        }

    }

    void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                anim.SetTrigger("Attack");

                currentTime = 0;
                player.GetComponent<PlayerMove>().DamageAction(5);
                audioAtk.Play();
                //agent.enabled = false;
                agent.ResetPath();
            }
        }
        else
        {
            //agent.enabled = true;
            m_State = MonsterState.Move;
            currentTime = 0;
            anim.SetTrigger("AttackToRun");
        }
    }

    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }

    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(0.5f);
        if (ishitted == true)
        {
            m_State = MonsterState.Move;
            anim.SetTrigger("HitToRun");
        }
        else
        {
            m_State = MonsterState.Move;
            audioSht.Play();
            anim.SetTrigger("HitToShout");
            ishitted = true;
        }
    }

    public void HitMonster(int hitPower)
    {
        if (m_State == MonsterState.Damaged || m_State == MonsterState.Die)
        {
            return;
        }

        hp -= hitPower;

        if (transform.GetComponent<NavMeshAgent>().enabled == true)
        {
            agent.ResetPath();
        }

        if (hp > 0)
        {
            m_State = MonsterState.Damaged;
            Damaged();
            print("상태 전환 : Any State -> Damagaed");
            anim.SetTrigger("Hit");
            audioHit.Play();
        }
        else
        {
            //hpslider.SetActive(false);
            agent.enabled = false;
            m_State = MonsterState.Die;
            print("상태 전환 : Any State -> Die");
            canv.SetActive(false);
            anim.SetTrigger("Die");
            audioDie.Play();
            Die();
        }
    }

    /*void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        yield return new WaitForSeconds(2f);
        // print("소멸 !");
        factory.DropItem(transform.position);
        Destroy(gameObject);
    }*/

    void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
        
    }

    IEnumerator DieProcess()
    {
        yield return new WaitForSeconds(3f);
        // print("소멸 !");
        Destroy(gameObject);
        factory.DropItem(transform.position);
    }
}
