using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet",fileName ="Bullet")]
public class BulletType : ScriptableObject
{
   [Range(0,10000)]
   public float bulletSpeed;
   [Range(0,30000)]
   public float bulletExplosionForce;
   [Range(0.1f, 2.5f)]
   public float bulletScale = 1f;

   public GameObject bulletPrefab;
   public GameObject hitEffect;
}
