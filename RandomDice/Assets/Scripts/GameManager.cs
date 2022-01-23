using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private BulletManager bulletManager;

    private void Awake()
    {
        instance = this;

        bulletManager = GetComponent<BulletManager>();
    }

    public BulletManager GetBulletManager()
    {
        return bulletManager;
    }
}
