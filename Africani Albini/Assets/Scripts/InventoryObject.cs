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

    // Start is called before the first frame update
    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public virtual void useObject() { print("(virtual) Using " + name); }

}
