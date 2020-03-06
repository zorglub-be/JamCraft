using System;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private int _damage;

    public void DealDamage(GameObject target)
    {
        if (target == null)
            return;
        target.GetComponentInChildren<IDamageable>()?.TakeDamage(_damage);
    }
}