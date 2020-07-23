using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    
    //References
    [SerializeField] private Player _player;
    [SerializeField] private GameObject shootVisualEffect;
    [SerializeField] private BulletType [] bullets;
    [SerializeField] private Transform bulletSpawnPos;
    
    //Events
    public event Action<int> OnBulletChanged;

    private short currentBulletIndex;
    private Camera _camera;
    
    void Start()
    {
        if (_player == null)
        {
            _player = GetComponentInParent<Player>();
        }

        if(shootVisualEffect==null)
            shootVisualEffect = Resources.Load<GameObject>("muzzleFlash");

        if (bulletSpawnPos == null)
        {
            bulletSpawnPos = transform.GetChild(0).GetChild(0).transform;
        }

        if (bullets == null || bullets.Length<1)
        {
            bullets = new BulletType[3];

            for (int i = 0; i < bullets.Length; i++)
            {
                string name = $"Bullet{i + 1}";
                bullets[i] = Resources.Load<BulletType>(name);
            }
        }

        _camera=Camera.main;
        
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentBulletIndex < bullets.Length - 1)
                currentBulletIndex++;
            else
            {
                currentBulletIndex = 0;
            }

            OnBulletChanged(currentBulletIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CanShoot())
            {
                RaycastHit hit;
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 250f,
                    1 << LayerMask.NameToLayer("Enemy")))
                {
                    Shoot(hit.point);
                    print(hit.collider.transform.root.name);
                }
            }
        }
    }

    public void Reset()
    {
        shootVisualEffect.SetActive(false);
        SortBullets();

        currentBulletIndex = (short)Random.Range(0, bullets.Length);
        OnBulletChanged(currentBulletIndex);
    }

    private void SortBullets()
    {
        Array.Sort(bullets,CompareTo);
    }

    private int CompareTo(BulletType type1,BulletType type2)
    {
        if (type1.bulletExplosionForce > type2.bulletExplosionForce)
        {
            return 1;
        }
        else if (type1.bulletExplosionForce < type2.bulletExplosionForce)
        {
            return -1;
        }
        else return 0;
    }

    private bool CanShoot()
    {
        if (shootVisualEffect)
            return !shootVisualEffect.activeSelf;
        else return false;
    }

    private void Shoot(Vector3 point)
    {
        _player.PlayAnimation(point);
        shootVisualEffect.SetActive(true);

        SpawnBullet(point);
        
        DissableWithDelay(shootVisualEffect, 1f);
    }

    private void SpawnBullet(Vector3 point)
    {
        Bullet bullet = Instantiate(bullets[currentBulletIndex].bulletPrefab, null).GetComponent<Bullet>();
        bullet.transform.position = bulletSpawnPos.position;
        bullet.transform.localScale = Vector3.one * bullets[currentBulletIndex].bulletScale;
        bullet.transform.forward = bulletSpawnPos.forward;
        
        bullet.InitializeBullet(bullets[currentBulletIndex]);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce((point-bullet.transform.position).normalized*bullets[currentBulletIndex].bulletSpeed);
    }

    private void DissableWithDelay(GameObject go, float time)
    {
        StartCoroutine(Delay(go, time));
    }

    private IEnumerator Delay(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
    }
}
