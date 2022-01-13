using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyDead : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioSource ExplosionSound;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ExplosionSound.Play();
            ExplosionSound.gameObject.GetComponent<S_DestroyAfterTime>().StartTimer();
            ExplosionSound.gameObject.transform.SetParent(null);


            rb.bodyType = RigidbodyType2D.Static;
            explosion.SetActive(true);
            explosion.transform.SetParent(null);
            Destroy(gameObject);
        }

    }
}
