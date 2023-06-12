using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{
    public GameObject itemFactory;
    public GameObject treasure;
    public GameObject explosionFactory;

    AudioSource _audio;
    Transform player;

    private int cnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        treasure.SetActive(false);
        player = GameObject.Find("Player").transform;
        _audio = explosionFactory.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DropItem(Vector3 position)
    {
        int number = Random.Range(0, 3);

        if (cnt != 12)
        {
            if (number == 1)
            {
                GameObject item = Instantiate(itemFactory);
                item.transform.position = position;
            }
            else if (number == 2)
            {
                GameObject fire = Instantiate(explosionFactory);

                fire.transform.position = position;
                _audio.Play();

                if (Vector3.Distance(fire.transform.position, player.position) < 15)
                {
                    player.GetComponent<PlayerMove>().DamageAction(10);
                }
            }
        }
        else if (cnt == 12)
        {
            treasure.SetActive(true);
            treasure.transform.position = position;
            Debug.Log("TREASURE!");
        }
        Debug.Log(cnt);
        cnt++;
    }

}
