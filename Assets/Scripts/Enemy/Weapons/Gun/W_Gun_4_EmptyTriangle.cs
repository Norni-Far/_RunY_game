using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Gun_4_EmptyTriangle : MonoBehaviour
{
    private GameObject Player;
    [SerializeField] private bool StartTargeting;
    [SerializeField] private EnemyMashine EnemyMashine;
    // Bullet
    [SerializeField] private Transform PlaceOFCreateBullet;
    [SerializeField] private GameObject Prefab_Bullet_4_;
    [SerializeField] private AudioSource CreateSound;
    //

    [SerializeField] private float TimeForCreateEnemy;
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
        StartTargeting = true;
        yield return new WaitWhile(() => EnemyMashine.PlayerIsLive);
        StartTargeting = false;
        StartCoroutine(CheckPlayer());
    }

    IEnumerator Targgeting()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (StartTargeting)
                Fire();

            yield return new WaitForSeconds(TimeForCreateEnemy);
        }

    }

    private void Fire()
    {
        CreateSound.Play();
        GameObject Inst = Instantiate(Prefab_Bullet_4_, PlaceOFCreateBullet.position, PlaceOFCreateBullet.rotation);
        Inst.GetComponent<S_EmptyTriangle>().EnemyMashine = EnemyMashine;
    }

}
