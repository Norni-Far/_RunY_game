using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MainMenuTriangle : MonoBehaviour
{
    Rigidbody2D rb;
    public S_MainMenu_Manager S_Manager;
    [SerializeField] private float ForceOfJump = 20;
    [SerializeField] private float ForceTorgue = 20;

    [SerializeField] private bool isJump = true;

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

    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private Vector3 Direct;
    void Start()
    {
        float a = Random.Range(0, 1.1f);
        float b = Random.Range(0, 1.1f);
        float c = Random.Range(0, 1.1f);

        MaxHealthBody.GetComponent<SpriteRenderer>().color = new Color(a, b, c);
        x = Random.Range(3.1f, 8f);
        y = Random.Range(3.1f, 8f);
        ForceOfJump = Random.Range(50f, 70.1f);

        Direct = new Vector3(x, y, 0);

        ForHp = Health / 10;
        twoHealth = Health;

        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(goJump());
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
        rb.AddForce(Direct * ForceOfJump);
        rb.AddTorque(-ForceTorgue);
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
            isJump = false;


        if (collision.gameObject.tag == "DeadWheel" && !isJump)
        {
            S_Manager.ListOfTriangles.Remove(gameObject);
            Health -= Health + 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Ground")
        //    isJump = false;


        if (collision.gameObject.tag == "DeadWheel" && !isJump)
        {
            S_Manager.ListOfTriangles.Remove(gameObject);
            Health -= Health + 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Ground")
        //    isJump = false;

        if (collision.gameObject.tag == "Finish")
        {
            S_Manager.ListOfTriangles?.Remove(gameObject);
            Destroy(gameObject);
        }
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

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
