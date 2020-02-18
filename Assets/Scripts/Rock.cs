using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    protected int currentHp;
    protected MeshRenderer mr;
    protected Rigidbody rb;

    [SerializeField]
    protected int fullHp;
    [SerializeField]
    protected Material fullMaterial, halfMaterial, deadMaterial;

    private void Start()
    {
        currentHp = fullHp;
        mr = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        mr.material = fullMaterial;
        rb.freezeRotation = true;
    }

    internal void GetHit(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            rb.freezeRotation = false;
            rb.AddForce(transform.forward * 80);
            mr.material = deadMaterial;
        }
        else if (currentHp <= fullHp / 2)
            mr.material = halfMaterial;
    }
}
