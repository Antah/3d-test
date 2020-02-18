using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    void PickUp();
    void Drop(float damageModifier = 0);
    void Throw(float damageModifier = 0);
}

public class PickupCollider : MonoBehaviour
{
    [SerializeField]
    public string itemName;
    [SerializeField]
    public GameObject item;

    public bool isCarried;
    private Transform carrier;

    public void Pickup(Transform parent)
    {
        isCarried = true;
        carrier = parent;
        item.GetComponent<IPickupable>().PickUp();
        item.GetComponent<Rigidbody>().detectCollisions = false;
        item.GetComponent<Rigidbody>().useGravity = false;
    }

    private void Update()
    {
        if(isCarried)
            item.transform.position = carrier.position;
    }

    public void Drop(Vector3 force, float damageModifier = 0)
    {
        isCarried = false;
        item.GetComponent<IPickupable>().Drop(damageModifier);
        item.transform.parent = null;
        item.GetComponent<Rigidbody>().detectCollisions = true;
        item.GetComponent<Rigidbody>().useGravity = true;
        item.GetComponent<Rigidbody>().AddForce(force);
    }

    public void Throw(Vector3 force, float damageModifier = 0)
    {
        isCarried = false;
        item.GetComponent<IPickupable>().Throw(damageModifier);
        item.transform.parent = null;
        item.GetComponent<Rigidbody>().detectCollisions = true;
        item.GetComponent<Rigidbody>().useGravity = true;
        item.GetComponent<Rigidbody>().AddForce(force);
    }
}
