using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour {

	private float speed = 10.0f;
    public float WalkSpeed = 7.0f;
    public float RunnigSpeed = 12.0f;
    public float JumpHeight = 8.0f;
    public bool flight = true,
                noclip = true;/*,
                mouseLock = false;*/
	public float WaterHeight = 15.5f;
	CharacterController character;
    CapsuleCollider collider;
    public GameObject cam;
    public GameObject cam1;
    public GameObject cam2;
    private bool NightVision = false;
    float moveFB, moveLR;
	
	float gravity = -9.8f;
    Vector3 movement;
    CharacterData data;

    void Start(){
		//LockCursor ();
		character = GetComponent<CharacterController> ();
        collider = GetComponent<CapsuleCollider>();
		//if (Application.isEditor) {
		//sensitivity = sensitivity * 1.5f;
        //; //todo need a special script for dealing with cursor stuff?
        speed = WalkSpeed;
        cam = cam1;
        cam2.SetActive(false);
        //}
        movement = new Vector3(0, 0, 0);
        data = GetComponent<CharacterData>();
    }

	void CheckForWaterHeight(){
		if (transform.position.y < WaterHeight) {
			gravity = 0f;			
		} else {
			gravity = -9.8f;
		}
	}

    void Update()
    {
        if (Input.GetButton("Run"))
        {
            speed = RunnigSpeed;
        }
        else
        {
            speed = WalkSpeed;
        }

        if (flight)
        {
            if (Input.GetButton("Jump")) movement.y = JumpHeight;
            else if (Input.GetKey(KeyCode.C)) movement.y = -JumpHeight;
            else movement.y = 0;
        }
        else if (character.isGrounded)
        {
            if (Input.GetButtonDown("Jump")) movement.y = JumpHeight;
            else movement.y = 0;
        }
        else movement.y += gravity * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.G)) flight = !flight;

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
        {
            noclip = !noclip;
            collider.enabled = !noclip;
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            data.look.LockMouse();
        }

        /* rimuovo temp.
        if(Input.GetKeyDown(KeyCode.N))
        {
            if(NightVision==false)
            {
                cam2.gameObject.SetActive(true);
                cam = cam2;
                NightVision = true;
            }
            else
            {
                cam2.gameObject.SetActive(false);
                cam = cam1;
                NightVision = false;
            }
        }/**/

        moveFB = Input.GetAxis ("Horizontal") * speed;
		moveLR = Input.GetAxis ("Vertical") * speed;

		CheckForWaterHeight();

        movement.x = moveFB;
        movement.z = moveLR;

		movement = transform.rotation * movement;
		character.Move (movement * Time.deltaTime);
	}

}
