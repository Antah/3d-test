using System;
using UnityEngine;

class Enemy : MonoBehaviour
{
    protected int currentHp;
    protected MeshRenderer mr;
    protected Rigidbody rb;

    [SerializeField]
    protected int fullHp;
    [SerializeField]
    protected Material fullMaterial, halfMaterial, deadMaterial;
    [SerializeField]
    protected bool destroyOnDeath;

    private void Start()
    {
        currentHp = fullHp;
        mr = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        mr.material = fullMaterial;
        if (rb != null)
            rb.freezeRotation = true;
    }

    internal void GetHit(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            if (destroyOnDeath)
                Destroy(this.gameObject);
            else
            {
                if (rb != null)
                {
                    rb.freezeRotation = false;
                    rb.AddForce(transform.forward * 80);
                }
                mr.material = deadMaterial;
            }
        }
        else if (currentHp <= fullHp / 2)
            mr.material = halfMaterial;
    }
}
