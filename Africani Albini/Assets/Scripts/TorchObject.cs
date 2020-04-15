using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchObject : InventoryObject
{
    public bool active;
    
    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        active = false;
    }

    private void Update()
    {
        ownedCoordinates();
    }

    public override void useObject()
    {
        print("Using Torcia");
        active = !active;
        if (active) print("Turned On");
        else print("Turned Off");
        transform.Find("SpotLight").gameObject.SetActive(active);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
        }
    }

}
