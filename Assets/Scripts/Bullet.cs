using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected MeshRenderer mr;
    [SerializeField]
    protected SphereCollider sc;
    [SerializeField]
    protected int damage;

    protected float damageModifier;

    public void Init(float damageModifier)
    {
        this.damageModifier = damageModifier;
        damage = (int)((float)damage * damageModifier);
        Destroy(3.0f);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
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

        mr.enabled = false;
        sc.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        transform.SetParent(null);
    }
}
