using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public Bullet bulletPrefab;
    [SerializeField]
    private int initNumOfObj = 20;

    private List<Bullet> pendingBullets;
    private List<Bullet> activeBullets;

    private void Start()
    {
        pendingBullets = new List<Bullet>();
        activeBullets = new List<Bullet>();

        for(int i = 0; i < initNumOfObj; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false);

            pendingBullets.Add(bullet);
        }
    }

    public Bullet CreateBullet(Vector2 pos, Enemy target, DiceType type)
    {
        Bullet bullet = null;
        if(pendingBullets.Count > 0)
        {
            int LastIdx = pendingBullets.Count - 1;
            bullet = pendingBullets[LastIdx];

            activeBullets.Add(bullet);
            pendingBullets.RemoveAt(LastIdx);
        }
        else 
        {
            bullet = Instantiate(bulletPrefab);
            activeBullets.Add(bullet);
        }

        bullet.Init(pos, target, type);
        bullet.gameObject.SetActive(true);

        return bullet;
    }

    public void MoveToPendingList(Bullet bullet)
    {
        pendingBullets.Add(bullet);
        activeBullets.Remove(bullet);
    }
}
