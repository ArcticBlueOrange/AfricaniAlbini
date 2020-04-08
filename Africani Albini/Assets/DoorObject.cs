using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : InventoryObject
{
    public bool locked,
                open; 
    GameObject pivot,
               mesh;
    [SerializeField] float rotationSpeed;
    void Start()
    {
        pickable = false;
        pivot = transform.Find("Pivot").gameObject;
        mesh = transform.Find("Mesh").gameObject;
        if (locked) objectName = "Door - locked";
        else objectName = "Door - unlocked";
    }

    public void changeState()
    {
        locked = !locked;
        if (locked) objectName = "Door - locked";
        else objectName = "Door - unlocked";
    }
    public void changeState(bool l)
    {
        locked = l;
        if (locked) objectName = "Door - locked";
        else objectName = "Door - unlocked";
    }

    public override void useObject()
    {
        if (!locked)
        {
            print("Door is unlocked");
            if (!open)
            {
                print("Opening door");
                StartCoroutine(OpenDoor());
            }
            else
            {
                print("Closing door");
                StartCoroutine(CloseDoor());
            }
        }
    }

    IEnumerator OpenDoor()
    {
       float rot = 0;
        while (rot < 90)
        {
            mesh.transform.RotateAround(pivot.transform.position, -pivot.transform.up, rotationSpeed * Time.deltaTime);
            rot += rotationSpeed * Time.deltaTime;
            yield return null;
        }
        open = true; 
    }
    
    IEnumerator CloseDoor()
    {
        float rot = 0;
        while (rot < 90)
        {
            mesh.transform.RotateAround(pivot.transform.position, pivot.transform.up, rotationSpeed * Time.deltaTime);
            rot += rotationSpeed * Time.deltaTime;
            yield return null;
        }
        open = false;
    }

}
