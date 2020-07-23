using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private BulletType type;
    private bool IsInteractable = true;

    private Vector3 lastPos = Vector3.zero;


    public void Update()
    {
        if (IsInteractable)
        {
            RaycastHit hit;
            if (Physics.Raycast(lastPos, (transform.position - lastPos).normalized, out hit, (transform.position-lastPos).magnitude,
                1 << LayerMask.NameToLayer("Enemy")))
            {
                Collision(hit);
            }

        }
    }

    public void FixedUpdate()
    {
        lastPos = transform.position;
    }


    private void Collision(RaycastHit hit)
    {
        if (IsInteractable)
        {
            IDamagable damagable = hit.collider.gameObject.GetComponentInParent<IDamagable>();
            if (damagable != null)
            {
                IsInteractable = false;
                Vector3 explosionPos = hit.point + hit.normal * 0.25f;
                damagable.OnTakeDamage(explosionPos, type.bulletExplosionForce);
                ParticleSystem vfx = Instantiate(type.hitEffect, null).GetComponentInChildren<ParticleSystem>();
                vfx.transform.position = hit.point;
                Destroy(vfx.transform.root.gameObject, vfx.main.duration);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if (IsInteractable)
        // {
        //     IDamagable damagable = other.gameObject.GetComponentInParent<IDamagable>();
        //     if (damagable != null)
        //     {
        //         print("Collision "+other.collider.name);
        //         IsInteractable = false;
        //         damagable.OnTakeDamage(transform.position, type.bulletExplosionForce);
        //         ParticleSystem vfx = Instantiate(type.hitEffect, null).GetComponentInChildren<ParticleSystem>();
        //         vfx.transform.position = other.contacts[0].point;
        //         Destroy(vfx.transform.root.gameObject, vfx.main.duration);
        //         Destroy(gameObject);
        //     }
        // }
    }

    public void InitializeBullet(BulletType type)
    {
        this.type = type;
    }
}
