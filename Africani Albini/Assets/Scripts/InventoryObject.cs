﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObject : MonoBehaviour
{
    public bool pickable = true;
    public float weight = 1;
    public string name = "Phone";
    [TextArea]
    public string description;
    public GameObject playerObject;
    public Sprite icon;
    public bool equip = false;
    protected Rigidbody rb;
    // Start is called before the first frame update
    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>());
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (equip)
        {
            transform.position = playerObject.transform.position + playerObject.transform.forward * 2;
            Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
            
            //transform.GetComponent<Collider>();
            //transform.GetComponent<>
        }
    }

    //these methods need override partially or totally by child classes
    public virtual void useObject()
    {
        print("(virtual) Using " + name);
    }

    public virtual void equipObject()
    {
        print("(virtual) Equipping " + name);
        equip = true;
        pickable = false;
        transform.Find("Mesh").gameObject.SetActive(equip);
        transform.gameObject.SetActive(equip); 
        setColliders(false);
    }

    public virtual void unEquipObject()
    {
        print("(virtual) Unequipping " + name);
        equip = false;
        //pickable = false;
        transform.Find("Mesh").gameObject.SetActive(equip);
        transform.gameObject.SetActive(equip);
        setColliders(true);

        pickable = true;// !equip;
    }

    public virtual void dropObject()
    {
        Debug.Log("Dropping " + name);
        //GetComponent<Collider>().enabled = true;
        equip = false;
        pickable = true;
        transform.Find("Mesh").gameObject.SetActive(true);
        transform.gameObject.SetActive(true);
        rb.velocity = new Vector3(0,0,0);
        setColliders(true);
    }

    void setColliders(bool val)
    {
        Collider[] colls = GetComponents<Collider>();
        foreach (Collider coll in colls) { coll.enabled = val; print("Collider " + coll + " set to " + val); }
    }
}
