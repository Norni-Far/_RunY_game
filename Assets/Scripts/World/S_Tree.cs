using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Tree : MonoBehaviour
{
    [SerializeField] private CircleCollider2D CircleCol;
    [SerializeField] private Rigidbody2D rb;

    bool ok;
    void Start()
    {
        float y = Random.Range(1.0f, 1.6f);
        float x = Random.Range(y - 0.2f, y + 0.2f);

        // change sclae
        transform.localScale = new Vector3(x, y, 0);

        StartCoroutine(StartDestroy());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" && !ok)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
        else
        {
            ok = true;
            //CircleCol.enabled = false;
            //rb.simulated = false;
            Destroy(this);
            StopAllCoroutines();
        }

    }

    IEnumerator StartDestroy()
    {
        yield return new WaitForSeconds(2f);
        //CircleCol.enabled = false;
        //rb.simulated = false;
        Destroy(this);
    }
}
