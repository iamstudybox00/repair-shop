using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    Bullet[] bullets;


    public BulletManager(int bulletCount, GameObject bulletOrigin, Transform parent)
    {
        bullets = new Bullet[bulletCount];
        for (int i = 0; i < bulletCount; i++)
        {
            bullets[i] = Object.Instantiate(bulletOrigin).GetComponent<Bullet>();
            bullets[i].transform.SetParent(parent);
            bullets[i].gameObject.SetActive(false);
        }
    }

    public void Shoot(Vector3 position, Vector3 dir, Color color)
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].gameObject.activeSelf)
            {
                bullets[i].Init(position, dir, color, 100);
                bullets[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public void ResetBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].gameObject.SetActive(false);
        }
    }
}