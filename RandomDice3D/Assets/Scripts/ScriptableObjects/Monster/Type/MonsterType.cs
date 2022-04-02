using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MonsterType")]
public class MonsterType : ScriptableObject
{
    public Sprite sprite;

    public float HpMult = 1f;

    public float moveSpeedMult = 1f;

    public int attackPowerMult = 1;
}
