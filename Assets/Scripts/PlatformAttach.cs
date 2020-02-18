using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
            other.gameObject.transform.parent = this.transform;      
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
            other.gameObject.transform.parent = null;
    }
}
