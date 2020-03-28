using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//SCRIPT TO CALCULATE FIELD OF VISION OF THE ENEMY. Should only communicate with EnemyData
public class EnemySight : MonoBehaviour
{
    [SerializeField] float fieldOfViewAngle = 110f;
    [SerializeField] bool playerInSight { get; set; } = false;
    [SerializeField] Vector3 personalLastSighting;
    private NavMeshAgent nav;
    private SphereCollider col;
    private Animator anim;
    //private Animator playerAnim;
    private InventoryManager playerInventory;
    private EnemyData data;
    private GameObject player;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        col = GetComponentInChildren<SphereCollider>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<InventoryManager>();
        //playerAnim = player.GetComponent<Animator>();
        //print("Sight active");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            //print("Player in area");
            playerInSight = false;

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            //print(angle);
            if (angle <= fieldOfViewAngle * .5f)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                {
                    if (hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        personalLastSighting = player.transform.position;
                        //print("Player In sight. Angle is " + angle.ToString() + ".");
                    }
                }
            }
        }
    }



}
