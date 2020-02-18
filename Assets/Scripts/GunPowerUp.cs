using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPowerUp : PowerUp
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterControls>().GunBoost();
            Destroy(this.gameObject);
        }
    }
}
