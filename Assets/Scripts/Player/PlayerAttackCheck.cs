using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCheck : MonoBehaviour
{
    private float dmage;
    private Color color;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster") ||
            other.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            Monster monsterSc = other.GetComponent<Monster>();
            monsterSc.monsterHit(dmage, color);
        }
    }

    public void SetAttackDamage(float _damage, Color _color)
    {
        dmage = _damage;
        color = _color;
    }
}
