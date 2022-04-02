using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DiceType")]
public class DiceType : ScriptableObject
{
    public string typeName = "Default";
    public float attackPower;
    public float attackSpeed;
    public enum TargetType
    {
        InFront,
        HPDescendingOrder,
        Random,
        None
    }
    public TargetType targetType;

    public Sprite sprite;
    public Color diceEyeColor;    
}
