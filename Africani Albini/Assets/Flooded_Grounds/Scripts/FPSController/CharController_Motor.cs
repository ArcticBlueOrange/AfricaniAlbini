using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour {

	private float speed = 10.0f;

    public float WalkSpeed = 7.0f;
    public float RunnigSpeed = 12.0f;
    public float JumpHeight = 8.0f;
    public bool flight = true,
                noclip = true;

	public float sensitivity = 30.0f;
	public float WaterHeight = 15.5f;
	CharacterController character;
    CapsuleCollider collider;

	private GameObject cam;
    public GameObject cam1;
    public GameObject cam2;

    private bool NightVision = false;
    float moveFB, moveLR;
	float rotX, rotY;
	public bool webGLRightClickRotation = true;
	float gravity = -9.8f;
    Vector3 movement;


	void Start(){
		//LockCursor ();
		character = GetComponent<CharacterController> ();
        collider = GetComponent<CapsuleCollider>();
		if (Application.isEditor) {
			webGLRightClickRotation = false;
			sensitivity = sensitivity * 1.5f;
            Cursor.visible = false;
            speed = WalkSpeed;
            cam = cam1;
            cam2.gameObject.SetActive(false);
		}
        movement = new Vector3(0, 0, 0);
	}


	void CheckForWaterHeight(){
		if (transform.position.y < WaterHeight) {
			gravity = 0f;			
		} else {
			gravity = -9.8f;
		}
	}



    void Update() {

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

        //if (!GetComponent<InventoryManager>().InventoryActive)
        {
    		rotX = Input.GetAxis ("Mouse X") * sensitivity;
	    	rotY = Input.GetAxis ("Mouse Y") * sensitivity;
        }

		//rotX = Input.GetKey (KeyCode.Joystick1Button4);
		//rotY = Input.GetKey (KeyCode.Joystick1Button5);

		CheckForWaterHeight ();

        movement.x = moveFB;
        movement.z = moveLR;
		//Vector3 movement = new Vector3 (moveFB, verticalMovement, moveLR);



		if (webGLRightClickRotation) {
			if (Input.GetKey (KeyCode.Mouse0)) {
				CameraRotation (cam, rotX, rotY);
			}
		} else if (!webGLRightClickRotation) {
			CameraRotation (cam, rotX, rotY);
		}

		movement = transform.rotation * movement;
		character.Move (movement * Time.deltaTime);
	}


	void CameraRotation(GameObject cam, float rotX, float rotY){		
		transform.Rotate (0, rotX * Time.deltaTime, 0);
		cam.transform.Rotate (-rotY * Time.deltaTime, 0, 0);
	}




}
