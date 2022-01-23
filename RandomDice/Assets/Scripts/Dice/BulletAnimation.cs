using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimation : MonoBehaviour
{
    public Sprite defaultSpr;
    public Sprite[] explosionSpr;
 
    private WaitForSeconds explosionAnimSpeed;
    
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        explosionAnimSpeed = new WaitForSeconds(0.05f);
    }

    public IEnumerator PlayExplosionAnim()
    {
        // 색깔 초기화

        int sprNum = 0;
        while(sprNum < explosionSpr.Length)
        {
            yield return explosionAnimSpeed;
            //Debug.Log("SPR " + sprNum);
            spriteRenderer.sprite = explosionSpr[sprNum];

            sprNum++;
        }

        //yield return explosionAnimSpeed;

        spriteRenderer.sprite = defaultSpr;

        //yield return new WaitForSeconds(0.5f);
        GetComponent<Bullet>().ResetBullet();
    }

    //private void InitSprColor(Color color)
    //{
    //    foreach(Sprite spr in explosionSpr)
    //    {
    //        spr.
    //    }
    //}
}
