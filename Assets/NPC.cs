using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Rigidbody myRB;
    public float mySpeed = 1f;
    public GameObject targetBall;
    public GameObject targetPlayer;
    public NPC myScript;

    // Start is called before the first frame update
    void Awake()
    {
        myRB = GetComponent<Rigidbody>();
        targetPlayer = GameObject.FindWithTag("Player");
        targetBall = GameObject.FindWithTag("Ball");
        myScript = GetComponent<NPC>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("update not defined yet");
    }

    protected virtual void Move()
    {
        Debug.Log("Move not defined for this object: " + name);
    }

    protected virtual void Kick()
    {
        Debug.Log("Kick not defined for this object: " + name);
    }

    protected virtual void Jump()
    {
        Debug.Log("Jump not defined for this object: " + name);
    }
}
