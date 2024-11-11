using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class playerMove : MonoBehaviour
{
    public bool debugs = true;
    public float speed = 10f;
    public float wallSpeed = 20f;
    public float jumpForce = 500f;
    public float walkAngleLimit = 45f;
    public float wallAngleLimit = 90f;
    Rigidbody myRB;
    public Camera myCam;
    List<ContactPoint> allHits = new List<ContactPoint>();
    ContactPoint currentGround;
    Vector3 inputDir;
    Vector3 playerLook;

    public enum playerMode
    {
        GROUNDED,
        FLYING,
        WALLRUN
    }

    public playerMode myMode;

    bool canJump;
    bool jump;


    // Start is called before the first frame update
    void Start()
    {
        myRB= GetComponent<Rigidbody>();
        canJump = false;
        jump = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)) { jump = true; }
    }

    void FixedUpdate()
    {
        playerLook = myCam.transform.TransformDirection(Vector3.forward) * 100;
        inputDir = Direction(true);
        //just a lil little visual and console debug for collisions
        Debug.Log("hits count: " + allHits.Count);
        foreach (ContactPoint hit in allHits)
        {
            if (hit.point != Vector3.zero) Debug.Log("hit at: " + hit.point);
            Debug.DrawRay(hit.point, Vector3.down * 3, Color.magenta, 1f);
            Debug.DrawRay(hit.point, Vector3.left, Color.magenta, 1f);
            Debug.DrawRay(hit.point, Vector3.forward, Color.magenta, 1f);
            currentGround = hit;
        }

        canJump = true;
        float groundAngle = Vector3.Angle(currentGround.normal, Vector3.up);
        if (groundAngle > walkAngleLimit && groundAngle <= wallAngleLimit) { if (myMode != playerMode.WALLRUN) EnterWallRun(); }
        else if (groundAngle < walkAngleLimit) { if (myMode != playerMode.GROUNDED) EnterWalking(); }
        else { if(myMode != playerMode.FLYING) EnterFlying(); canJump = false; }

        switch (myMode)
        {
            case playerMode.GROUNDED:
                Walk();
                break;

            case playerMode.FLYING:
                break;

            case playerMode.WALLRUN:
                WallRun();
                break;
        }

        if(jump && canJump)
        {
            Jump();
            jump = false;
        }


        //at the end of your fixedUpdate(), remember to clear the ArrayList
        //OnCollisionXXX() methods run after fixedUpdate() in the physics cycle
        //so this ensures we only have the most recent collision data
        //each time we run fixedUpdate()
        allHits.Clear();
    }

    void Walk()
    {
        myRB.AddForce(transform.TransformDirection(Direction(debugs)) * speed);
    }

    void WallRun()
    {
        //multiply playerLook Y value by the LOCAL forward of wishDir to give magnitude of force AGAINST the wall
        //essentially, the more of the player's vector is applied directly onto the wall, the greater the stored Y value becomes
        //this way if the player looks to the left or right they will rapidly return to wallrunning. it creates a slippery feel when going directly vertical
        float yStor = playerLook.normalized.y * transform.InverseTransformDirection(inputDir).z;
        Vector3 dir = Vector3.Cross(Vector3.Cross(currentGround.normal, inputDir), currentGround.normal); //cross multiply our strafe to be parallel with plane
        dir.y = (yStor + dir.y) / 2f;

        myRB.AddForce(dir * wallSpeed);
    }

    void Jump()
    {
        myRB.AddForce(Vector3.up * jumpForce);
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
            Debug.DrawRay(transform.position, transform.TransformDirection(dir * 2f), Color.white);
            Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.green);
            Debug.DrawRay(transform.position + Vector3.up, transform.right, Color.green);
        }
        return dir;
    }

    void OnCollisionStay(Collision collision)
    {
        //Unity best practice advises not to directly call collision.contacts
        //so instead we declare an array and use GetContacts(index)
        ContactPoint[] myContacts = new ContactPoint[collision.contactCount];
        for (int i = 0; i < myContacts.Length; i++)
        {
            //this only returns contacts for a single collision, use inside OnCollisionStay for logic that
            //should happen during all single collisions
            myContacts[i] = collision.GetContact(i);

            //this is our class-wide allHits var, reference in fixedUpdate or other methods to get all collisions
            //we use ArrayList.Add() because checking an array for empty values and replacing them
            //always returns exactly 1 valid contactPoint result, probably a race condition problem
            allHits.Add(myContacts[i]);
        }
        //code to run on collision goes here. things like enterMode() or
        //exitMode() functions, bounces or reactionary code, etc

    }

    void EnterFlying()
    {
        myRB.useGravity = true;
        canJump = false;
        myMode = playerMode.FLYING;
    }

    void EnterWalking()
    {
        myRB.useGravity = true;
        canJump = true;
        myMode = playerMode.GROUNDED;
    }

    void EnterWallRun()
    {
        myRB.useGravity = false;
        canJump = true;
        myMode = playerMode.WALLRUN;
    }
}
