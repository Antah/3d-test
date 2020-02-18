using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform start, end;
    [SerializeField]
    private GameObject platform;
    [SerializeField]
    private float rateInit = 1f;
    [SerializeField]
    private float waitTime = 3f;

    void Start()
    {
        platform.transform.position = start.position;
        StartCoroutine(MovePlatform(false));
    }

    IEnumerator MovePlatform(bool reverse)
    {
        float ti = 0.0f;
        Vector3 posA, posB;
        if (reverse)
        {
            posB = start.position;
            posA = end.position;
        }
        else
        {
            posA = start.position;
            posB = end.position;
        }

        while (ti < 1f)
        {
            ti += Time.deltaTime * rateInit;
            platform.transform.position = Vector3.Lerp(posA, posB, ti);
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);
        StartCoroutine(MovePlatform(!reverse));
    }
}
