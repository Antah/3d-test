using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private int damage;
    /*
    [SerializeField]
    private List<ParticleSystem> particles;
    */
    [SerializeField]
    private int duration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
            other.gameObject.GetComponent<Enemy>().GetHit(damage);
    }

    private void Start()
    {
        /*
        float duration = 0f;
        foreach(var particle in particles)
        {
            if(duration < particle.duration)
                duration = particle.duration;
        }
        */
        Destroy(duration);
    }

    private void Destroy(float time = 0f)
    {
        StartCoroutine(DestroyWithDelay(time));
    }

    IEnumerator DestroyWithDelay(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        transform.SetParent(null);
    }

    public void SetDamageModifier(float damageModifier)
    {
        damage = (int)((float)damage * damageModifier);
    }
}
