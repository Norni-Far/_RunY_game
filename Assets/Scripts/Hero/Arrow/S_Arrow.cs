using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class S_Arrow : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D BoxCol;

    [SerializeField] private AudioSource TuchGroundSound;
    public bool StartRotation;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        BoxCol = gameObject.GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        if (StartRotation)
        {
            OtherStaticForAll.RotationForDirection(rb, gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartRotation = false;

        if (collision.gameObject.tag == "Ground")
        {
            TuchGroundSound.Play();
            //gameObject.transform.SetParent(collision.gameObject.transform);
            BoxCol.enabled = false;
            rb.simulated = false;
            rb.bodyType = RigidbodyType2D.Static;
            gameObject.isStatic = true;
            Destroy(this);
        }
    }
}
