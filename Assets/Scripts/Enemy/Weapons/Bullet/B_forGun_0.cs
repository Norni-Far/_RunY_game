using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class B_forGun_0 : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private TrailRenderer Line;
    [SerializeField] private Gradient Two;

    [SerializeField] private float PowerSpeed;

    private bool Dead;
    private bool Ground;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float Rand = Random.Range(-5, 5);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + Rand);
        rb.AddForce(transform.right * PowerSpeed);
    }

    void Update()
    {
        if (transform.tag == "Untagged" && !Dead && !Ground)
        {
            Dead = true;
            Line.transform.SetParent(null);
            Line.colorGradient = Two;
            Line.autodestruct = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (!Dead)
            {
                Line.autodestruct = true;
                Line.transform?.SetParent(null);
            }

            StartCoroutine(StartDestroy());
        }

    }

    IEnumerator StartDestroy()
    {
        Ground = true;
        gameObject.tag = "Untagged";
        gameObject.isStatic = true;
        rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
