using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public float offset = 45;
    
    private Animator _animator;
    private Transform currentRot;
    private Vector3 target = Vector3.zero;

    private void Start()
    {
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }

        currentRot = transform.GetChild(0).GetChild(0).GetChild(2);
    }

    public void PlayAnimation(Vector3 target)
    {
        _animator.SetTrigger("Shoot");
        this.target = target;
    }

    public void LateUpdate()
    {
        if (target != Vector3.zero)
        {
            currentRot.LookAt(target, Vector3.up);
            currentRot.rotation = currentRot.rotation * Quaternion.Euler(0, offset, 0);
        }
    }
}
