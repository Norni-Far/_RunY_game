using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_DestroyAfterTime : MonoBehaviour
{
    public void StartTimer() => StartCoroutine(StartTimeDestroy());

    IEnumerator StartTimeDestroy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
