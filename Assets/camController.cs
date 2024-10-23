using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camController : MonoBehaviour
{

    Vector3 myLook;
    public Camera myCam;
    public float lookSpeed = 1f;
    public float camLock = 90f;
    public bool debugs;
    float onStartTimer;
    // Start is called before the first frame update

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        myLook = transform.localEulerAngles;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        onStartTimer += Time.deltaTime;
        //camera forward direction
        myLook += DeltaLook() * Time.deltaTime;


        //clamp the magnitude to keep the player from looking fully upside down
        myLook.y = Mathf.Clamp(myLook.y, -camLock, camLock);

        //Debug.Log("myLook: " + myLook);
        transform.rotation = Quaternion.Euler(0f, myLook.x, 0f);
        myCam.transform.rotation = Quaternion.Euler(-myLook.y, myLook.x, 0f);

        if(debugs)
        {
            Debug.DrawRay(myCam.transform.position, myCam.transform.forward*10f, Color.black);
        }


    }

    Vector3 DeltaLook()
    {
        Vector3 dLook;
        float rotY = Input.GetAxisRaw("Mouse Y") * lookSpeed;
        float rotX = Input.GetAxisRaw("Mouse X") * lookSpeed;
        dLook = new Vector3(rotX, rotY, 0);

        if (dLook != Vector3.zero)
        {
            //Debug.Log("delta look: " + dLook);
        }

        if (onStartTimer < 1f)
        {
            dLook = Vector3.ClampMagnitude(dLook, onStartTimer * 10f);
        }

        return dLook;
    }
}
