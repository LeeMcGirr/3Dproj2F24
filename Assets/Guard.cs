using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : NPC
{

    Vector3 rayDir;
    public float castRadius = 2f;
    // Start is called before the first frame update
    void Start()
    {
        targetBall = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BallListener();
        if( targetBall != null)
        {
            Vector3 wishDir = transform.InverseTransformDirection(Vector3.forward * 250f);
            myRB.AddForce(wishDir);
        }
    }

    void BallListener()
    {
        rayDir = Vector3.forward * 10f;
        rayDir = transform.InverseTransformDirection(rayDir);
        RaycastHit hit;


        Debug.DrawRay(transform.position, rayDir, Color.white);
        if (Physics.Raycast(transform.position, rayDir, out hit, Mathf.Infinity))
        {
        //    Debug.Log("ray cast name: " + hit.collider.gameObject.name);
        //    Debug.Log("ray cast position: " + hit.point);
        }


        if(Physics.SphereCast(transform.position, castRadius, rayDir, out hit))
        {
            Debug.Log("sphere cast name: " + hit.collider.gameObject.name);
            Debug.Log(" sphere cast position: " + hit.point);
            if(hit.collider.gameObject.tag == "Ball")
            {
                targetBall = hit.collider.gameObject;
            }
            else { targetBall = null; }
        }
        else { targetBall = null; }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 increment = rayDir.normalized;
        for(int i = 0; i < 10; i++)
        {
            Gizmos.DrawWireSphere((transform.position + (rayDir*i)/10), castRadius);
        }

        Gizmos.color = Color.red;
    }
}
