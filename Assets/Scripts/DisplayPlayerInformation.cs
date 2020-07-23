using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum BulletName
{
    Weak=0,
    Normal=1,
    Strong=2
}

public class DisplayPlayerInformation : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private TextMeshProUGUI bulletText;
    
    void Start()
    {
        bulletText = GetComponent<TextMeshProUGUI>();

        weapon = FindObjectOfType<Weapon>();
        weapon.OnBulletChanged += UpdateBulletInformation;
        weapon.Reset();
    }

    void Update()
    {
        
    }

    private void OnDisable()
    {
        weapon.OnBulletChanged -= UpdateBulletInformation;
    }

    private void UpdateBulletInformation(int bulletSortIndex)
    {
        bulletText.text = ((BulletName) bulletSortIndex).ToString()+" Bullet";
    }
}
