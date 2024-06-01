using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackCheck : MonoBehaviour
{
    private float dmage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            InputController playerSc = other.GetComponent<InputController>();
            playerSc.PlayerHitCheck(dmage);

            gameObject.SetActive(false);
        }
    }

    public void SetAttackDamage(float _damage)
    {
        dmage = _damage;
    }
}
