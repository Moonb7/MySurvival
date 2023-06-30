using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private CharacterStats stats;

    private Damageable damageable;

    private void Start()
    {
        stats = GetComponent<CharacterStats>();
        if (stats == null)
            stats = GetComponentInParent<CharacterStats>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == this || stats.isDeath)
            return;

        damageable = other.GetComponent<Damageable>();
        if (damageable == null)
        {
            damageable = other.GetComponentInParent<Damageable>();
            return;
        }
        damageable.InflictDamage(stats.attack.GetValue(), false, this.gameObject);

    }
}
