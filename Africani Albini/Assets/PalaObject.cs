﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalaObject : InventoryObject
{
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (equip)
        //{
        //    transform.position = playerObject.GetComponent<CharacterData>().control.cam1.transform.position + //playerObject.transform.forward * 2;
        //                         playerObject.GetComponent<CharacterData>().control.cam1.transform.forward * 2;
        //    transform.rotation = playerObject.GetComponent<CharacterData>().control.cam1.transform.rotation;//playerObject.transform.rotation;
        //    Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
        //    //transform.GetComponent<Collider>();
        //    //transform.GetComponent<>
        //}
        ownedCoordinates();
    }

    public override void useObject()
    {
        // perform animation
        print("TODO Animazione Badile");

    }
    public override void useObject(InventoryObject oth)
    {
        print("TODO Animazione Badile");
        if (oth.objectName == "Zolla di Terra")
        {
            print("Scavando...");
            //io.useObject(GetComponent<PalaObject>());
            StartCoroutine(DestroyAfterEndZolla(oth.transform.gameObject));
        }
    }


    IEnumerator DestroyAfterEndZolla(GameObject zolla)
    {
        while (zolla)
        {
            yield return null;
        }
        playerObject.GetComponent<CharacterData>().inventory.dropSelectedObject();
        Destroy(gameObject);
    }

}
