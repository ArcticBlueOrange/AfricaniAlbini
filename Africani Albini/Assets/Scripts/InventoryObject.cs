using System.Collections;
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
    // Start is called before the first frame update
    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>());
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
        pickable = !equip;
    }

    public virtual void unEquipObject()
    {
        print("(virtual) Unequipping " + name);
        equip = false;
        //pickable = false;
        transform.Find("Mesh").gameObject.SetActive(equip);
        transform.gameObject.SetActive(equip);
        pickable = !equip;
    }
}
