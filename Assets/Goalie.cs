using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goalie : NPC
{
    // Update is called once per frame
    void Update()
    {
        Move(); 
    }

    protected override void Move()
    {
        //code that tracks the soccer ball and attempts to 
        //block shots in here
        float xDelta = targetBall.transform.position.x - transform.position.x;
        Vector3 dir = new Vector3(xDelta, 0f, 0f).normalized;
        dir *= mySpeed;
        myRB.AddForce(dir);
    }
}
