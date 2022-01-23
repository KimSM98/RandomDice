using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceEye : MonoBehaviour
{
    private Enemy target;

    public void SetTarget(Enemy enemy)
    {
        target = enemy;
    }
}
