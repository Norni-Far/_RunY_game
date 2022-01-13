using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyMashineForMainMenu : MonoBehaviour
{
    [SerializeField] private float dumping;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform EnemyPointPosition;

    private void Start()
    {
        StartCoroutine(StartFly());
    }

    private void FixedUpdate()
    {
        Vector3 target = new Vector3(EnemyPointPosition.position.x + offset.x, EnemyPointPosition.position.y + offset.y, transform.position.z);
        Vector3 currentPosition = Vector3.Lerp(transform.position, target, dumping * Time.deltaTime);
        transform.position = currentPosition;
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
}
