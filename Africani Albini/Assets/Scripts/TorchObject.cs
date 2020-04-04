using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchObject : InventoryObject
{
    //public bool pickable = true;
    //public float weight = 1;
    //public string name = "Phone";
    //[TextArea]
    //public string description;
    //public GameObject playerObject;
    //public Sprite icon;
    public bool active;// = false;
    
    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        active = false;
        //print("A torch is born");
        //Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void Update()
    {
        ////print("A torch is ALIVE");
        //if (equip)
        //{
        //    transform.position = playerObject.transform.position + playerObject.transform.forward * 2;
        //    transform.rotation = playerObject.transform.rotation;
        //    Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
        //    //transform.GetComponent<Collider>();
        //    //transform.GetComponent<>
        //}   
        ownedCoordinates();
    }

    public override void useObject()
    {
        print("Using Torcia");
        active = !active;
        if (active) print("Turned On");
        else print("Turned Off");
        transform.Find("SpotLight").gameObject.SetActive(active);
        //if ( active )
        //{
        //    //transform.gameObject.SetActive(active);
        //    transform.Find("SpotLight").gameObject.SetActive(active);
        //    //transform.Find("Mesh").gameObject.SetActive(!active);
        //} else
        //{
        //    transform.Find("SpotLight").gameObject.SetActive(active);
        //    //transform.Find("Mesh").gameObject.SetActive(active);
        //    //transform.gameObject.SetActive(active);
        //}
        //equip = active;
    }

    //public override void equipObject()
    //{
    //    //transform.gameObject
    //}
    //
    //public override void unEquipObject()
    //{
    //    //transform.gameObject
    //}


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
        }
    }

}
