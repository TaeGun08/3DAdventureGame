using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    [Header("스테이터스 설정")]
    [SerializeField] private float damage;
    [SerializeField] private float hp;
    [SerializeField] private float armor;
    [SerializeField] private float stamina;
    [SerializeField] private float ciritical;
    [SerializeField] private float ciriticalDamage;
    [Space]
    [SerializeField] private GameObject statusObj;
    [SerializeField] private List<Button> buttons; 
}
