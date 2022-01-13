using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider2D), typeof(Rigidbody2D))]
public class B_forGun_3_Capsule : MonoBehaviour
{
    [SerializeField] private float PowerSpeed;
    [SerializeField] private GameObject PrefabOfMine;
    [SerializeField] private AudioSource OtdelenieSound;

    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * PowerSpeed);
        StartCoroutine(StartWayOfMine());
    }

    IEnumerator StartWayOfMine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
            Instantiate(PrefabOfMine, transform.position, transform.rotation);
            OtdelenieSound.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }

    }
}
