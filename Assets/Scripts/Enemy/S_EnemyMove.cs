using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class S_EnemyMove : MonoBehaviour
{
    //
    private EnemyMashine EnemyMashine;
    //

    [SerializeField] private AudioSource ArrayTouchSound;
    [SerializeField] private AudioSource DeadSound;

    [SerializeField] private float dumping;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Transform PointOfCreateWeapon;

    [SerializeField] private GameObject Fire;
    private GameObject InstWeapon;
    private Transform EnemyPointPosition;
    private Rigidbody2D rb;

    [SerializeField] private int Health = 5;
    [SerializeField] private bool StartMove;

    public int NumberOfPosition = 0;
    private void Start()
    {
        EnemyMashine = GameObject.FindGameObjectWithTag("EnemyMashine")?.GetComponent<EnemyMashine>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        //// добавление машины в лист со всеми машинами, для выбора оружия и ИИ 

        StartCoroutine(StartFly());
    }

    public void PlacementTarget(Transform Target)
    {
        EnemyPointPosition = Target;
        StartMove = true;
    }

    public void PlacementWeapons(GameObject Weapon)
    {
        InstWeapon = Instantiate(Weapon, PointOfCreateWeapon.position, PointOfCreateWeapon.rotation);
        InstWeapon.transform.SetParent(PointOfCreateWeapon);
        InstWeapon.transform.localPosition = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (StartMove)
        {
            Vector3 target = new Vector3(EnemyPointPosition.position.x + offset.x, EnemyPointPosition.position.y + offset.y, transform.position.z);
            Vector3 currentPosition = Vector3.Lerp(transform.position, target, dumping * Time.deltaTime);
            transform.position = currentPosition;
        }

        if (Health < 0)
            DeadMashine();
    }

    IEnumerator StartFly()
    {
        while (true)
        {

            yield return new WaitForSeconds(0.5f);
            float RandX = Random.Range(-0.2f, 0.2f);
            float RandY = Random.Range(-0.2f, 0.2f);
            offset = new Vector2(RandX, RandY);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            ArrayTouchSound.Play();
            GameObject Arrow = collision.gameObject;
            TakeDamage(Arrow);
            Health--;
        }
    }

    public void TakeDamage(GameObject Arrow)
    {
        Rigidbody2D AroowRb = Arrow.GetComponent<Rigidbody2D>();
        Arrow.tag = "Untagged";
        Arrow.transform.SetParent(gameObject.transform);
        Arrow.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(Arrow.GetComponent<S_Arrow>());
        AroowRb.bodyType = RigidbodyType2D.Kinematic;
        AroowRb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void DeadMashine()
    {
        DeadSound.Play();

        Fire.SetActive(true);
        InstWeapon.GetComponent<CircleCollider2D>().enabled = false;
        InstWeapon.gameObject.tag = "Destroy";

        rb.gravityScale = 1;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddTorque(Random.Range(-120, 120));
        EnemyMashine.EnemyList.Remove(gameObject);
        EnemyMashine.RandPosition[NumberOfPosition] = null;

        Destroy(this);
    }

}

