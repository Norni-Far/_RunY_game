using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(HingeJoint2D))]
public class B_GravityBall : MonoBehaviour
{
    Rigidbody2D rb;
    HingeJoint2D Hinglejoint;

    [SerializeField] private float Impulse = 2;
    private bool Magnet = false;
    private void Start()
    {
        Hinglejoint = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.mass = Random.Range(0.07f, 0.2f);
        StartCoroutine(Destroy());
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Magnet = true;
            transform.SetParent(collision.transform);
            Hinglejoint.enabled = true;
            Hinglejoint.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
            gameObject.layer = 13; // чтобы не косался стрел 
            rb.gravityScale = rb.gravityScale * (-1);
            StartCoroutine(StartGlue());
        }
    }


    IEnumerator StartGlue()
    {
        yield return new WaitForSeconds(10f);

        Destroy(gameObject);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(10f);

        if (!Magnet)
        {
            Destroy(gameObject);
        }
    }


}
