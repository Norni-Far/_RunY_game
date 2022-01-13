using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class B_forCircleGun_2_ : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] private float PowerSpeed;
    [SerializeField] private GameObject PrefabGravityBall;
    [SerializeField] private AudioSource TuchSound;

    private bool TuchGround;
    public void Move(Vector3 Rotation)
    {
        rb = GetComponent<Rigidbody2D>();

        transform.eulerAngles = Rotation;
        rb.AddForce(transform.right * PowerSpeed);
    }

    private void MagnetOn()
    {
        TuchSound.Play();

        TuchSound.gameObject.GetComponent<S_DestroyAfterTime>().StartTimer();
        TuchSound.gameObject.transform.SetParent(null);

        for (int i = 0; i < 5; i++)
        {
            GameObject Inst = Instantiate(PrefabGravityBall, transform.position, transform.rotation);
            Inst.transform.SetParent(null);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            if (!TuchGround)
                StartAnimDestroy();
        }
    }

    private void StartAnimDestroy()
    {
        TuchGround = true;
        MagnetOn();
        Destroy(gameObject);
    }



}
