using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OtherStaticForAll 
{
   
    public static void RotationForDirection(Rigidbody2D rb, GameObject gameObject)
    {
        var Dir = rb.velocity;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(-Dir.x, Dir.y) * Mathf.Rad2Deg);
    }

}
