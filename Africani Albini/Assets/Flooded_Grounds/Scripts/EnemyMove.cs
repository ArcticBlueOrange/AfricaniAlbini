using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Transform Target1;
    [SerializeField] Transform Target2;
    [SerializeField] Transform Target3;

    private Transform TargetPosition;
    [SerializeField] GameObject playerObject;

    [SerializeField] int CurrentTarget = 1;
    [SerializeField] float waitTime = 2;
    private bool Contact = false;
    private int lastTarget = 1;
    [SerializeField] int enemyState = 0;
    [SerializeField] float walkSpeed = 5;
    [SerializeField] float runSpeed = 10;




    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        TargetPosition = Target1;
        //MoveToTarget();
        lastTarget = CurrentTarget;
        enemyState = 0;//1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EnemieTarget")) {
            if(Contact == false)
            {
                Contact = true;
                CurrentTarget = Random.Range(1, 4);

                if(CurrentTarget == lastTarget)
                {
                    TryAgain();
                }
                else
                {
                    StartCoroutine(Wait());
                }
            }
        }
    }

    private void TryAgain()
    {
        if(lastTarget==1)
        {
            CurrentTarget = lastTarget + 1;
        }
        else if(lastTarget>1)
        {
            CurrentTarget = lastTarget -1;
        }
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

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.F2)) { enemyState = 0; print("State " + enemyState);} // idle
        if ( Input.GetKeyDown(KeyCode.F3)) { enemyState = 1; print("State " + enemyState);} // patrol
        if ( Input.GetKeyDown(KeyCode.F4)) { enemyState = 2; print("State " + enemyState);} // inseguimento

        switch( enemyState)
        {
            case 0:
                StartCoroutine(Wait());
                break;
            case 1:
                MoveToTarget();
                break;
            case 2:
                followPlayer();
                break;
        }
    }

    void MoveToTarget()
    {
        GetComponent<NavMeshAgent>().isStopped = false;
        anim.SetInteger("State", 0);
        if (CurrentTarget==1)
        {
            TargetPosition = Target1;
        }
        if (CurrentTarget == 2)
        {
            TargetPosition = Target2;
        }
        if (CurrentTarget == 3)
        {
            TargetPosition = Target3;
        }
        GetComponent<NavMeshAgent>().speed = walkSpeed;
        GetComponent<NavMeshAgent>().destination = TargetPosition.position;
        lastTarget = CurrentTarget;
        Contact = false;
    }

    void followPlayer()
    {
        anim.SetInteger("State", 0);
        GetComponent<NavMeshAgent>().isStopped = false;
        GetComponent<NavMeshAgent>().speed = runSpeed;
        GetComponent<NavMeshAgent>().destination = playerObject.transform.position;
    }


}
