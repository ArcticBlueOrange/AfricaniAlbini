using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using Random = UnityEngine.Random;


//THIS SCRIPT SHOULD ONLY CONTAIN  DATA REFERRING ON HOW TO MOVE THE ENEMY
public class EnemyMove : MonoBehaviour
{
    //[SerializeField] Transform Target1;
    //[SerializeField] Transform Target2;
    //[SerializeField] Transform Target3;
    [SerializeField] Transform[] wayPoints;

    private Transform TargetPosition;
    [SerializeField] GameObject playerObject;

    private int lastTarget = 1;
    [SerializeField] int currentTarget = 0;
    [SerializeField] float waitTime = 2;
    private bool Contact = false;
    [SerializeField] float walkSpeed = 5;
    [SerializeField] float runSpeed = 10;
    private EnemyData data;
    private Animator anim;
    void Awake()
    {
        data = GetComponent<EnemyData>();
        updateWayPoints();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        TargetPosition = wayPoints[currentTarget];
        //MoveToTarget();
        lastTarget = wayPoints.Length - 1;
        print(wayPoints.Length);
        //foreach(Transform t in wayPoints)
        //{ print(t.name); }
    }

    public void updateWayPoints()
    {
        wayPoints = data.patrolWayPoints.GetComponentsInChildren<Transform>();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Vector3 horizontalOthr = new Vector3(other.transform.position.x, 0, other.transform.position.z);
    //    Vector3 horizontalSelf = new Vector3(transform.position.x, 0, transform.position.z);
    //    float distance = Vector3.Distance(horizontalOthr, horizontalSelf);
    //    //print(distance);
    //    if(other.gameObject.CompareTag("EnemieTarget") && distance <= 4f ) {
    //        //print(other.name);
    //        //print(other.isTrigger);
    //        if(Contact == false)
    //        {
    //            Contact = true;
    //            currentTarget = Random.Range(0, wayPoints.Length);
    //
    //            if(currentTarget == lastTarget )//|| wayPoints[currentTarget].tag != "EnemieTarget")
    //            {
    //                TryAgain();
    //            }
    //            else
    //            {
    //                StartCoroutine(Wait());
    //            }
    //        }
    //    }
    //}
    private void TryAgain()
    {
        currentTarget = (currentTarget + 1) % wayPoints.Length;
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
        anim.SetInteger("State", 1);
        yield return new WaitForSeconds(waitTime);
        //anim.SetInteger("State", 0);
        Contact = false;
        //MoveToTarget();
        
    }
    void Update()
    {
        switch( data.enemyState)
        {
            case EnemyData.EnemyStateEnum.idle:
                StartCoroutine(Wait());
                break;
            case EnemyData.EnemyStateEnum.patrol:
                MoveToTarget();
                break;
            case EnemyData.EnemyStateEnum.chase:
                followPlayer();
                break;
            case EnemyData.EnemyStateEnum.search:
                break;
        }
    }
    void MoveToTarget()
    {// this might be slow because recalculates the target waypoint at every "Update"
        GetComponent<NavMeshAgent>().isStopped = false;
        anim.SetInteger("State", 0);

        TargetPosition = wayPoints[currentTarget];

        GetComponent<NavMeshAgent>().speed = walkSpeed;
        GetComponent<NavMeshAgent>().destination = TargetPosition.position;
        lastTarget = currentTarget;
        //currentTarget = (currentTarget + 1) % wayPoints.Length;
        Contact = false;
        Vector3 horizontalOthr = new Vector3(wayPoints[currentTarget].transform.position.x, 0, wayPoints[currentTarget].transform.position.z);
        Vector3 horizontalSelf = new Vector3(transform.position.x, 0, transform.position.z);
        if (Vector3.Distance(horizontalOthr, horizontalSelf) < 4)
        {
            //currentTarget = Random.Range(0, wayPoints.Length);
            lastTarget = currentTarget;
            currentTarget = (currentTarget + 1) % wayPoints.Length;
            // qusto serve perché lo 0 appartiene al parent del percorso. Troveremo un modo per non metterlo
            if (currentTarget == 0) currentTarget++;
            //TODO maybe it's better not random???
            //if (currentTarget == lastTarget)//|| wayPoints[currentTarget].tag != "EnemieTarget")
            //{
            //    TryAgain();
            //}
            //else
            //{
            //    StartCoroutine(Wait());
            //}
        }
    }
    void followPlayer()
    {
        anim.SetInteger("State", 0);
        GetComponent<NavMeshAgent>().isStopped = false;
        GetComponent<NavMeshAgent>().speed = runSpeed;
        GetComponent<NavMeshAgent>().destination = playerObject.transform.position;
    }


}
