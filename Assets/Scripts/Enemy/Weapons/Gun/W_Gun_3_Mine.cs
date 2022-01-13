using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class W_Gun_3_Mine : MonoBehaviour
{
    [SerializeField] private EnemyMashine EnemyMashine;
    private GameObject Player;
    [SerializeField] private bool StartTargeting;

    // Bullet
    [SerializeField] private Transform PlaceOFCreateBulletOFMine_0;
    [SerializeField] private Transform PlaceOFCreateBulletOFMine_1;
    [SerializeField] private GameObject Prefab_Bullet_3_;
    [SerializeField] private AudioSource FireSound;
    //

    [SerializeField] private float TimeForCreateMine;
    private string Target = "Player";


    private void Start()
    {
        EnemyMashine = GameObject.FindGameObjectWithTag("EnemyMashine").GetComponent<EnemyMashine>();
        StartCoroutine(CheckPlayer());
        StartCoroutine(Targgeting());
    }

    IEnumerator CheckPlayer()
    {
        yield return new WaitUntil(() => EnemyMashine.PlayerIsLive);

        Player = MoveEnemyStatic.FindPlayer();

        yield return new WaitWhile(() => EnemyMashine.PlayerIsLive);

        StartCoroutine(CheckPlayer());
    }

    void LateUpdate()
    {
        if (gameObject.tag == "Destroy")
        {
            StartTargeting = false;
            Destroy(this);
        }
    }

    IEnumerator Targgeting()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);

            if (EnemyMashine.PlayerIsLive && StartTargeting)
            {
                gameObject.transform.eulerAngles = new Vector3(0, 0, Random.Range(35f, 40.1f));
                Fire();
            }

            yield return new WaitForSeconds(TimeForCreateMine);
        }
    }

    private void Fire()
    {
        FireSound.Play();
        GameObject Inst = Instantiate(Prefab_Bullet_3_, PlaceOFCreateBulletOFMine_0.position, PlaceOFCreateBulletOFMine_0.rotation);
        Inst = Instantiate(Prefab_Bullet_3_, PlaceOFCreateBulletOFMine_1.position, PlaceOFCreateBulletOFMine_1.rotation);
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

