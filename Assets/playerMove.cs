using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    public bool debugs = true;
    float speed = 10;
    Rigidbody myRB;
    // Start is called before the first frame update
    void Start()
    {
        myRB= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        myRB.AddForce(Direction(debugs) * speed);
    }

    Vector3 Direction(bool debugs)
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);

        if (debugs)
        {
            Debug.DrawRay(transform.position, myRB.velocity, Color.yellow);
            Debug.Log("vector: " + dir);
            Debug.DrawRay(transform.position, dir * 2f, Color.white);
        }
        return dir;
    }
}