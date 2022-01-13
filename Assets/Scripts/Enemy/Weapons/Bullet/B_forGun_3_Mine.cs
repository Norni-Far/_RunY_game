using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PolygonCollider2D), typeof(Rigidbody2D))]
public class B_forGun_3_Mine : MonoBehaviour
{
    [SerializeField] private GameObject Explosion_particle;
    [SerializeField] private AudioSource ExplosionSound;

    private void Start()
    {
        StartCoroutine(Destroy());
    }

    public void Explosion()        
    {
        ExplosionSound.Play();
        Explosion_particle.transform.SetParent(null);
        Explosion_particle.SetActive(true);

        ExplosionSound.gameObject.GetComponent<S_DestroyAfterTime>().StartTimer();
        ExplosionSound.gameObject.transform.SetParent(null);

        Destroy(gameObject);
    }   

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(40f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
            Explosion();
    }



}
