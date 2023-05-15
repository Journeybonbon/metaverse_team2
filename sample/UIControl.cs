using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIControl : MonoBehaviour
{
    public int hp = 20;// 플레이어 체력 변수
    int maxHP = 20;
    public Slider hpSlider;
    public GameObject hitEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = (float)hp / (float)maxHP;
    }

    public void DamageAction(int damage) 
    {
        hp -= damage;
        if(hp > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
        if(hp < 0)
        {
            hp = 0;
        }
        print(hp);
    }

    IEnumerator PlayHitEffect() // 맞았을 때
    {
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitEffect.SetActive(false);
    }
}
