using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class S_Triangle : MonoBehaviour
{
    public delegate void Delegats(GameObject Player);
    public event Delegats event_PlayerisLive;
    public event Delegats event_PlayerisDead;

    //
    private Save_Json Save;
    //private GameManager GameManager;
    private S_Canvas S_Canvas;  // TEST
    //

    [Header("Prefabs")]
    [SerializeField] private AudioSource TakeDamage;
    [SerializeField] private AudioSource DeadSound;
    [SerializeField] private AudioSource FallSound;
    [SerializeField] private AudioSource JumpSound;
    [SerializeField] private AudioSource FireArraySound;
    [SerializeField] private AudioSource ExploseDamageSound;



    [SerializeField] private GameObject _prefab_Tuch_;
    [SerializeField] private GameObject Prifab_Arrow;
    private GameObject ParentForArrow;
    private GameObject Hungle;

    // Companents
    private Rigidbody2D rb;

    // Parts
    [SerializeField] private GameObject MaxHealthBody;
    [SerializeField] private GameObject MainPart;
    [SerializeField] private GameObject DeadPart;
    [SerializeField] private GameObject[] ColliderPartsOfTriangle = new GameObject[9];
    [SerializeField] private GameObject[] RigidbodyPartsOfTriangles = new GameObject[9];

    [SerializeField] private float ForceOfJump;
    public float ForceOfJojstik;
    [SerializeField] private bool isJump = false;

    private GameObject ArrowCreate;
    private Rigidbody2D ArrowRbCreate;
    private float AngleForArrow;
    private bool Targeting;

    [SerializeField] public int Health;
    [SerializeField] private int twoHealth;
    private int ForHp = 10;
    private ContactPoint2D[] Contacts = new ContactPoint2D[1];

    [SerializeField] private int SpeedOfFall;
    [SerializeField] private int lvl;
    public void FindWithTeg()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        S_Canvas = GameObject.FindGameObjectWithTag("S_Canvas").GetComponent<S_Canvas>();
        // Вкл джойстиков
        //S_Canvas.JoystickOn();
        //
        Hungle = GameObject.FindGameObjectWithTag("HungleForJump").gameObject;
        //GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        ParentForArrow = GameObject.FindGameObjectWithTag("parentForArroy").gameObject;
        Save = GameObject.FindGameObjectWithTag("S_Save").GetComponent<Save_Json>();
    }

    private void Start()
    {
        FindWithTeg();
        andNowStart();
    }

    private void andNowStart()
    {
        Chooselvl();

        // Оповещение о новом игроке
        event_PlayerisLive?.Invoke(gameObject);
        //GameManager.PlayerInTheScene();
        //

        rb = gameObject.GetComponent<Rigidbody2D>();
        //
        ForHp = Health / 10;
        twoHealth = Health / ForHp;
    }

    private void Chooselvl()
    {
        int lvl = Save.LoadPropertiesForDificultGame.lvl;

        switch (lvl)
        {
            case 0:
                Health = 50;
                break;

            case 1:
                Health = 20;
                break;

            case 2:
                Health = 10;
                break;

            case 3:
                Health = 10;
                break;
        }
    }


    public void Jump()
    {
        if (!isJump)
        {
            JumpSound.Play();

            rb.AddForce(Hungle.transform.localPosition * ForceOfJump * ForceOfJojstik);

            if (Hungle.transform.localPosition.x < 0)
                rb.AddTorque(45);
            else
                rb.AddTorque(-45);

            isJump = false;
        }
    }

    public void Attack_Direction(Vector3 TargetPos, bool Fire)
    {
        if (!Targeting)
            ArrowCreate = Instantiate(Prifab_Arrow, gameObject.transform.position, new Quaternion(0, 0, 0, 0));

        ArrowRbCreate = ArrowCreate.GetComponent<Rigidbody2D>();
        ArrowCreate.transform.SetParent(gameObject.transform);
        Targeting = true;

        Vector3 aa = new Vector3(0, 0, 0);
        Vector3 diff = aa - TargetPos;
        diff.Normalize();
        AngleForArrow = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;

        ArrowCreate.transform.rotation = Quaternion.Euler(0f, 0f, AngleForArrow);
    }
    public void Attack_Fire()
    {
        FireArraySound.Play();
        ArrowCreate.transform.SetParent(ParentForArrow.transform);
        Targeting = false;
        ArrowRbCreate.simulated = true;
        ArrowRbCreate.AddForce(ArrowCreate.transform.up * 200f);
        ArrowCreate.GetComponent<S_Arrow>().StartRotation = true;
    }

    private void Update()
    {
        CheckHealth();
    }

    private void FixedUpdate()
    {
        if (rb.IsSleeping() && isJump)
            isJump = false;

    }

    #region Collicion

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJump = false;           
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.GetContacts(Contacts);

        if (collision.gameObject.tag == "Ground")
        {
            FallSound.Play();
            Instantiate(_prefab_Tuch_, Contacts[0].point, Quaternion.Euler(Vector3.zero), null);
        }

        //if(collision.gameObject.tag == "EmptyTriangle")
            //TakeDamage.Play();


        if (collision.gameObject.tag == "0_Bullet")
        {
            //TakeDamage.Play();
            collision.transform.tag = "Untagged";
            Health -= 1;
        }

        if (collision.gameObject.tag == "1_Rocket")
        {
            ExploseDamageSound.Play();
           
            B_forRocketGun Rocket = collision.gameObject.GetComponent<B_forRocketGun>();
            Health -= 2;
            Rocket.particlesExplosion.transform.SetParent(null);
            Rocket.particlesExplosion.SetActive(true);
            WaveOfExplosion(collision.gameObject);
            Destroy(collision.gameObject);
        }


        if (collision.gameObject.tag == "DeadWheel" && !isJump)
            Health -= Health + 1;


        if (collision.gameObject.tag == "Mine")
        {
            //ExploseDamageSound.Play();

            //ExploseDamageSound.gameObject.GetComponent<S_DestroyAfterTime>().StartTimer();
            //ExploseDamageSound.gameObject.transform.SetParent(null);

            collision.gameObject.GetComponent<B_forGun_3_Mine>().Explosion();
            Health -= 3;
            WaveOfExplosion(collision.gameObject);
            //rb.AddForce((transform.position - collision.transform.position) * 25, ForceMode2D.Impulse);
        }

        if (collision.gameObject.tag == "EmptyTriangle")
            Health -= 1;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isJump = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            //isJump = false;
            if (rb.velocity.y < 4)
                S_Canvas.UpdateMaxFall(Mathf.Abs(rb.velocity.y));
        }

        if (collision.gameObject.tag == "Ground" && rb.velocity.y < -SpeedOfFall) // приземление         
            Health -= Convert.ToInt32(Mathf.Abs(rb.velocity.y)) / SpeedOfFall;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isJump = true;

    }

    #endregion


    // Взрывная волна
    private void WaveOfExplosion(GameObject Explosion) => rb.AddForce((transform.position - Explosion.transform.position) * 25, ForceMode2D.Impulse);

    private void CheckHealth()
    {
        if (Health / ForHp < twoHealth)
        {
            print("dc");

            S_Canvas.UpdateMaxFall(Health);
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

            TakeDamage.Play();

        }
    }

    public void DestroyTriangle(int health)
    {
        ColliderPartsOfTriangle[health].SetActive(false);
        //ColliderPartsOfTriangle[health].gameObject.tag = "Ground";

        // RigidbodyPartsOfTriangles[health].tag = "Ground";
        RigidbodyPartsOfTriangles[health].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        RigidbodyPartsOfTriangles[health].GetComponent<PolygonCollider2D>().enabled = true;
        RigidbodyPartsOfTriangles[health].GetComponent<S_DestroyAfterTime>().StartTimer();
        RigidbodyPartsOfTriangles[health].transform.SetParent(null);
    }

    private void DestroyObject()
    {

        gameObject.tag = "Untagged";
        MainPart.SetActive(false);
        DeadPart.transform.SetParent(null);
        DeadPart.SetActive(true);

        DeadSound.Play();
        //S_Canvas.JoystickOff();

        // Оповещение о разрушении игрока
        event_PlayerisDead?.Invoke(gameObject);
        //GameManager.PlayerIsDestroy();
        //

        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
