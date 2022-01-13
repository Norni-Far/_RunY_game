using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class W_Gun_1_RocketGun : MonoBehaviour
{
    [SerializeField] private EnemyMashine EnemyMashine;
    private GameObject Player;
    public bool StartTargeting;

    private string Target = "Player";

    public GameObject Prefab_Bullet;
    [SerializeField] private AudioSource StartRocket;

    [SerializeField] private Transform PointOfCreateUp;
    [SerializeField] private Transform PointOfCreateDown;

    private void Start()
    {
        EnemyMashine = GameObject.FindGameObjectWithTag("EnemyMashine").GetComponent<EnemyMashine>();
        StartCoroutine(CheckPlayer());
        StartCoroutine(Targetting());
    }

    IEnumerator CheckPlayer()
    {
        yield return new WaitUntil(() => EnemyMashine.PlayerIsLive);

        Player = MoveEnemyStatic.FindPlayer();

        yield return new WaitWhile(() => EnemyMashine.PlayerIsLive);

        StartCoroutine(CheckPlayer());
    }

    IEnumerator Targetting()
    {
        while (true)
        {
            GameObject BulletUp = Instantiate(Prefab_Bullet, PointOfCreateUp.position, PointOfCreateUp.rotation);
            BulletUp.transform.SetParent(PointOfCreateUp.transform);

            GameObject BulletDown = Instantiate(Prefab_Bullet, PointOfCreateDown.position, PointOfCreateDown.rotation);
            BulletDown.transform.SetParent(PointOfCreateDown.transform);


            while (PointOfCreateUp.transform.childCount != 0 || PointOfCreateDown.childCount != 0)
            {
                yield return new WaitForSeconds(5f);

                if (gameObject.tag != "Destroy")
                    if (StartTargeting && Player != null)
                        if (BulletUp != null)
                        {
                            StartRocket.Play();

                            print("Fire_1");
                            BulletUp.GetComponent<B_forRocketGun>().Fire();
                            BulletUp = null;
                            yield return new WaitForSeconds(10f);
                        }
                        else if (BulletDown != null)
                        {
                            StartRocket.Play();

                            print("Fire_2");
                            BulletDown.GetComponent<B_forRocketGun>().Fire();
                            BulletDown = null;
                            yield return new WaitForSeconds(10f);
                        }
            }

            yield return new WaitForSeconds(10f);
        }
    }

    void LateUpdate()
    {
        if (gameObject.tag != "Destroy")
            if (StartTargeting && Player != null)
            {
                MoveEnemyStatic.DirectionTowards(gameObject, Player);
            }
            else
            {
                StartTargeting = false;
                gameObject.transform.eulerAngles = Vector3.zero;
            }
        else
            Destroy(this);
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
