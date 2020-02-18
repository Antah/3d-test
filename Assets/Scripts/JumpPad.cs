using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour, IPickupable
{
    [SerializeField]
    public int speed;

    private bool on = true;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Drop(float damageModifier)
    {
        on = true;
    }

    public void PickUp()
    {
        on = false;
    }

    public void Throw(float damageModifier)
    {
        on = true;
    }

    void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player") && on)
            {
                var rb = other.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(Vector3.up * speed);
                audioSource.Play();
            }
        }
}
