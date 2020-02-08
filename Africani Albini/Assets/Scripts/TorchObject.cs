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
    public bool active = false;
    public bool equip = false;
    // Start is called before the first frame update
    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        //print("A torch is born");
        //Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    private void Update()
    {
        ////print("A torch is ALIVE");
        //if (equip)
        //    {
        //        transform.position = playerObject.transform.position;
        //        Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
        //    //transform.GetComponent<Collider>();
        //    //transform.GetComponent<>
        //}   
    }

    public override void useObject()
    {
        print("Using Torcia");
        active = !active;
        if (active) print("Turned On");
        else print("Turned Off");
        //if ( active )
        //{
        //    transform.gameObject.SetActive(active);
        //    transform.Find("SpotLight").gameObject.SetActive(active);
        //    transform.Find("Mesh").gameObject.SetActive(!active);
        //} else
        //{
        //    transform.Find("SpotLight").gameObject.SetActive(!active);
        //    transform.Find("Mesh").gameObject.SetActive(active);
        //    transform.gameObject.SetActive(!active);
        //}

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
        }
    }

}
