using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ForMainMenuDeadLineWorld : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(TimeToDead());
    }

    IEnumerator TimeToDead()
    {
        yield return new WaitForSeconds(40f);
        Destroy(gameObject);
    }
}
