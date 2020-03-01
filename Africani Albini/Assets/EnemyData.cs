using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//MAIN "AI" CLASS OF THE ENEMY, and referencer for the other ENEMY scripts
public class EnemyData : MonoBehaviour
{
    private GameObject player;

    private EnemyMove move;
    private EnemySight sight;
    public GameObject patrolWayPoints;
    public enum EnemyStateEnum { idle, patrol, search, chase};
    public EnemyStateEnum enemyState;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        move = GetComponent<EnemyMove>();
        sight = GetComponent<EnemySight>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2)) { enemyState = EnemyStateEnum.idle; print("State idle"); } // idle
        if (Input.GetKeyDown(KeyCode.F3)) { enemyState = EnemyStateEnum.patrol; print("State patrol"); } // patrol
        if (Input.GetKeyDown(KeyCode.F4)) { enemyState = EnemyStateEnum.chase; print("State follow"); } // inseguimento
        if (Input.GetKeyDown(KeyCode.F5)) { enemyState = EnemyStateEnum.search; print("State search"); } // inseguimento
    }

}
