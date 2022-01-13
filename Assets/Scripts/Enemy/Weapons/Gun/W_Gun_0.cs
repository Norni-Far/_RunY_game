using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class W_Gun_0 : MonoBehaviour
{
    [SerializeField] private EnemyMashine EnemyMashine;
    [SerializeField] private GameObject Player;
    public bool StartTargeting;
    private string Target = "Player";

    [SerializeField] private GameObject Prefab_Bullet;
    [SerializeField] private AudioSource soundFire;
    [SerializeField] private Transform PointOfCreate;

    [SerializeField] private bool ISeePlayer;
    private void Start()
    {
        EnemyMashine = GameObject.FindGameObjectWithTag("EnemyMashine").GetComponent<EnemyMashine>();
        StartCoroutine(CheckPlayer());
        StartCoroutine(FireForTarget());
    }

    IEnumerator CheckPlayer()
    {
        yield return new WaitUntil(() => EnemyMashine.PlayerIsLive);

        Player = MoveEnemyStatic.FindPlayer();
        
        ISeePlayer = true;

        yield return new WaitWhile(() => EnemyMashine.PlayerIsLive);

        ISeePlayer = false;

        StartCoroutine(CheckPlayer());
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Target)
        {
            if (!StartTargeting)
            {
                StartTargeting = true;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Target)
        {
            StartTargeting = false;
        }
    }

    IEnumerator FireForTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.1f, 2f));

            if (StartTargeting)
            {
                //
                soundFire.Play();
                //

                Instantiate(Prefab_Bullet, PointOfCreate.position, transform.rotation);
            }
        }
    }
}
