using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MonsterStatus")]
public class MonsterStatus : ScriptableObject
{
    public float attackPower;
    public float hp;
    public float moveSpeed;
}
