using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_DestroyAfterStart_Timer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(StartTimeDestroy());
    }
    IEnumerator StartTimeDestroy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
