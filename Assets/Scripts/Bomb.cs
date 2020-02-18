using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Bullet, IPickupable
{
    [SerializeField]
    private GameObject explosionObject;
    private bool armed;

    public void Init(float damageModifier)
    {
        armed = true;
        this.damageModifier = damageModifier;
        damage = (int)((float)damage * damageModifier);
        Destroy(3.0f);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (!armed)
            return;
        Destroy();
        if (collisionInfo.gameObject.tag == "Enemy")
            collisionInfo.gameObject.GetComponent<Enemy>().GetHit(damage);
    }

    private void Destroy(float time = 0f)
    {
        StartCoroutine(DestroyWithDelay(time));
    }

    IEnumerator DestroyWithDelay(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject explosion = Instantiate(explosionObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        explosion.transform.parent = transform.parent;
        explosion.GetComponent<Explosion>().SetDamageModifier(damageModifier);
        explosion.SetActive(true);

        mr.enabled = false;
        sc.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        transform.SetParent(null);
    }

    public void PickUp()
    {

    }

    public void Drop(float damageModifier)
    {
        this.damageModifier = damageModifier;
        Destroy(3.0f);
    }

    public void Throw(float damageModifier)
    {       
        Init(damageModifier);
    }
}
