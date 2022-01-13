using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class W_Gun_2_CircleOfMagnit : MonoBehaviour
{
    [SerializeField] private EnemyMashine EnemyMashine;
    private GameObject Player;
    [SerializeField] private bool StartTargeting;


    // Bullet
    [SerializeField] private Transform PlaceOFCreateBullet;
    [SerializeField] private GameObject Prefab_Bullet_2_;
    [SerializeField] private AudioSource soundFire;
    //

    [SerializeField] private float TimeForCreateBall = 10;
    private string Target = "Player";

    private void Start()
    {
        EnemyMashine = GameObject.FindGameObjectWithTag("EnemyMashine").GetComponent<EnemyMashine>();
        StartCoroutine(CheckPlayer());
        StartCoroutine(Targgeting());
    }


    void LateUpdate()
    {
        if (gameObject.tag == "Destroy")
        {
            StartTargeting = false;
            Destroy(this);
        }
    }


    IEnumerator CheckPlayer()
    {
        yield return new WaitUntil(() => EnemyMashine.PlayerIsLive);

        Player = MoveEnemyStatic.FindPlayer();

        yield return new WaitWhile(() => EnemyMashine.PlayerIsLive);

        StartCoroutine(CheckPlayer());
    }

    IEnumerator Targgeting()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeForCreateBall);

            if (EnemyMashine.PlayerIsLive)
            {
                gameObject.transform.eulerAngles = new Vector3(0, 0, Random.Range(15f, 25.1f));

                Fire();

            }            
        }
    }

    private void Fire()
    {
        //
        soundFire.Play();
        //

        GameObject Inst = Instantiate(Prefab_Bullet_2_, PlaceOFCreateBullet.position, PlaceOFCreateBullet.rotation);
        Inst.GetComponent<B_forCircleGun_2_>().Move(gameObject.transform.eulerAngles);
    }


    // Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Target)
        {
            StartTargeting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Target)
        {
            StartTargeting = false;
        }
    }

}
