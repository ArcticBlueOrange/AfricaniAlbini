using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Look : MonoBehaviour
{
    CharacterData data;
    private bool mouseLock;/*,
                 mouseCentered;/**/
    public float sensitivity = 45.0f;
    public bool webGLRightClickRotation = true;
    float rotX, rotY;

    void Start()
    {
        data = GetComponent<CharacterData>();
        webGLRightClickRotation = false;
        mouseLock = false;
        //mouseCentered = true;
    }

    void Update()
    {
        if ( !isMouseLock() )//|| data.inventory.InventoryActive))
        {
            rotX = Input.GetAxis("Mouse X") * sensitivity;
            rotY = Input.GetAxis("Mouse Y") * sensitivity;
            //rotX = Input.GetKey (KeyCode.Joystick1Button4) * sensitivity;
            //rotY = Input.GetKey (KeyCode.Joystick1Button5) * sensitivity;
        } else { rotX = rotY = 0; }

        if (mouseLock)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false; //fastidioso al momento
            Cursor.lockState = CursorLockMode.Locked; //fastidioso al momento
        }

        if (webGLRightClickRotation)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                CameraRotation(data.control.cam, rotX, rotY);
            }
        }
        else if (!webGLRightClickRotation)
        {
            CameraRotation(data.control.cam, rotX, rotY);
        }
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(0, rotX * Time.deltaTime, 0);
        cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);
    }

    public void LockMouse() { mouseLock = !mouseLock; }
    //public void CenterMouse()  { mouseCentered = !mouseCentered; } //try to do everything with mouseLocks
    public void LockMouse(bool b) { mouseLock = b; }
    //public void CenterMouse(bool b)  { mouseCentered = b; }

    public bool isMouseLock() { return mouseLock; }
    //public bool isMouseCentered() { return mouseCentered; }
}
