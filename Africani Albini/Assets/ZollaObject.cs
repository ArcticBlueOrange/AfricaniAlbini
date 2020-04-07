using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZollaObject : InventoryObject
{
    Animator anim;
    void Start()
    {
        pickable = false;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public override void useObject()
    {
        print("Nothing happens");
        //anim.SetTrigger("Scava");
        //StartCoroutine(destroyAfterAnim());
    }
    public override void useObject(InventoryObject obj)
    {
        if (obj.objectName == "Badile")
        {
            anim.SetTrigger("Scava");
            StartCoroutine(destroyAfterAnim());
        }
    }

    IEnumerator destroyAfterAnim()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("End")) //Non si pulò fare di meglio?
        {
            yield return null;
        }
        Destroy(gameObject);
    }


}
