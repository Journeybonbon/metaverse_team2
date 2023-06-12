using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject monsterFactory;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject monster = Instantiate(monsterFactory);
            monster.transform.position = spawnPoints[i].position;
        }
    }
}
