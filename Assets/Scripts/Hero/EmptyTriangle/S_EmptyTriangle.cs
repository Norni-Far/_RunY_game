using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class S_EmptyTriangle : MonoBehaviour
{
    [SerializeField] public EnemyMashine EnemyMashine;
    Rigidbody2D rb;
    [SerializeField] private GameObject Player;
    [SerializeField] private float ForceOfJump = 20;
    [SerializeField] private bool isJump = true;
    [SerializeField] private AudioSource FallSound;

    // Parts
    [SerializeField] private GameObject MaxHealthBody;
    [SerializeField] private GameObject MainPart;
    [SerializeField] private GameObject DeadPart;
    [SerializeField] private GameObject[] ColliderPartsOfTriangle = new GameObject[9];
    [SerializeField] private GameObject[] RigidbodyPartsOfTriangles = new GameObject[9];


    [SerializeField] private int Health = 10;
    private int twoHealth;
    private int ForHp = 10;

    [SerializeField] private bool go = false;

    void Start()
    {
        ForHp = Health / 10;
        twoHealth = Health;

        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(WaitForDead());
        StartCoroutine(goJump());
        StartCoroutine(CheckPlayer());
    }

    void Update()
    {
        CheckHealth();
    }

    private void FixedUpdate()
    {
        if (rb.IsSleeping() && isJump)
            isJump = false;
    }

    IEnumerator WaitForDead()
    {
        yield return new WaitForSeconds(30f);
        Health -= Health + 1;
    }


    IEnumerator CheckPlayer()
    {
        yield return new WaitUntil(() => EnemyMashine.PlayerIsLive);

        Player = MoveEnemyStatic.FindPlayer();

        yield return new WaitWhile(() => EnemyMashine.PlayerIsLive);

        StartCoroutine(CheckPlayer());
    }


    IEnumerator goJump()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (!isJump && go)
                Jump();
        }
    }

    private void Jump()
    {
        if (Player != null)
        {
            Vector3 target = (Player.transform.position - gameObject.transform.position).normalized;
            rb.AddForce((target + new Vector3(0, 1, 0)) * ForceOfJump);
            rb.AddTorque(-65);
        }

    }

    private void CheckHealth()
    {
        if (Health / ForHp < twoHealth)
        {
            twoHealth--;

            switch (twoHealth)
            {
                case 9:
                    MaxHealthBody.SetActive(false);
                    MainPart.SetActive(true);
                    break;

                case 8:
                case 7:
                case 6:
                case 5:
                case 4:
                case 3:
                case 2:
                case 1:
                    DestroyTriangle(twoHealth);
                    ColliderPartsOfTriangle[twoHealth - 1].SetActive(true);
                    break;

                case 0:
                    DestroyTriangle(twoHealth);
                    MainPart.GetComponent<PolygonCollider2D>().enabled = true;
                    break;

                case -1:
                    DestroyObject();
                    break;
            }
        }
    }

    public void DestroyTriangle(int health)
    {
        ColliderPartsOfTriangle[health].SetActive(false);
        ColliderPartsOfTriangle[health].gameObject.tag = "Ground";

        RigidbodyPartsOfTriangles[health].tag = "Ground";
        RigidbodyPartsOfTriangles[health].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        RigidbodyPartsOfTriangles[health].GetComponent<PolygonCollider2D>().enabled = true;
        RigidbodyPartsOfTriangles[health].GetComponent<S_DestroyAfterTime>().StartTimer();
        RigidbodyPartsOfTriangles[health].transform.SetParent(null);

    }

    private void DestroyObject()
    {
        gameObject.tag = "Ground";
        MainPart.SetActive(false);
        DeadPart.transform.SetParent(null);
        DeadPart.SetActive(true);

        Destroy(gameObject);
    }



    // Collision

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJump = false;
            FallSound.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Ground")        
        //    isJump = false;


        if (collision.gameObject.tag == "Arrow")
        {
            GameObject Arrow = collision.gameObject;
            TakeDamage(Arrow);
            Health--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Ground")        
        //    isJump = false;        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isJump = true;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isJump = true;

    }

    public void TakeDamage(GameObject Arrow)
    {
        Arrow.tag = "Untagged";
        Arrow.transform.SetParent(gameObject.transform);
        Arrow.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(Arrow.GetComponent<S_Arrow>());
        Arrow.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        Arrow.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
