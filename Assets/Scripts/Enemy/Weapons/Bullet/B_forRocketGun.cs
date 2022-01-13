using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class B_forRocketGun : MonoBehaviour
{
    [SerializeField] private AudioSource ExplosionSound;

    public GameObject particlesExplosion;
    [SerializeField] private GameObject myFireForRocket;
    Rigidbody2D rb;

    [SerializeField] private float SpeedMove;
    private bool onFire = false;
    void Start()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (onFire)
            rb.AddForce(transform.right * Time.deltaTime * SpeedMove, ForceMode2D.Impulse);
    }

    public void Fire()
    {
        rb.simulated = true;
        transform.parent = null;
        myFireForRocket.SetActive(true);
        onFire = true;
        StartCoroutine(Fallowing());
    }

    IEnumerator Fallowing()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            MoveEnemyStatic.DirectionTowards(gameObject, MoveEnemyStatic.FindPlayer()); ;
        }
    }

    public void Explosion()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ExplosionSound.Play();

            ExplosionSound.gameObject.GetComponent<S_DestroyAfterTime>().StartTimer();
            ExplosionSound.gameObject.transform.SetParent(null);


            particlesExplosion.transform.SetParent(null);
            particlesExplosion.SetActive(true);
            Destroy(gameObject);
        }
    }

}
