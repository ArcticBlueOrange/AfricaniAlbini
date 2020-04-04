using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidScript : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            gameObject.transform.Rotate(0, 90, 0);
            anim.Play("HajMacarena");
        }
    }
}
