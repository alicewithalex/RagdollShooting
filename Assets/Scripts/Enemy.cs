using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RagdollPart
{

    public Rigidbody rigidbody;
    public Collider collider;
    
    public RagdollPart(Rigidbody rigidbody, Collider collider)
    {
        this.rigidbody = rigidbody;
        this.collider = collider;
    }

    public void TogglePhysics(bool value)
    {
        rigidbody.useGravity = value;
        rigidbody.isKinematic = !value;

        rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;

        collider.enabled = true;
    }
}

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField]
    private Animator _animator;

    private List<RagdollPart> _ragdollParts;

    private Vector3 initialPosition = Vector3.zero;
    
    void Start()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        
        initialPosition = transform.position;
        
        Initialize();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleRagdoll(true);
            ApplyForce(transform.position+new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f)),5000f);
            _animator.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    private void Initialize()
    {
        _ragdollParts = new List<RagdollPart>();

        List<Rigidbody> rigidbodies = GetComponentsInChildren<Rigidbody>().ToList();
        List<Collider> colliders = new List<Collider>();

        foreach (var rb in rigidbodies)
        {
            colliders.Add(rb.GetComponent<Collider>());

            if (rb != null && colliders[colliders.Count - 1] != null)
            {
                _ragdollParts.Add(new RagdollPart(rb,colliders[colliders.Count-1]));
            }
        }
        

        foreach (var part in _ragdollParts)
        {
            part.TogglePhysics(false);
        }
    }

    private void ToggleRagdoll(bool value)
    {
        foreach (var part in _ragdollParts)
        {
            part.TogglePhysics(value);
        }
    }

    private void Reset()
    {
        TimeAffector.instance.ToggleSlowMotion(false);
        
        ToggleRagdoll(false);
        _animator.enabled = true;
        _animator.Rebind();

        transform.position = initialPosition;
    }

    public void OnTakeDamage(Vector3 position,float force)
    {
        ToggleRagdoll(true);
        _animator.enabled = false;
        ApplyForce(position, force);
        
        TimeAffector.instance.ToggleSlowMotion(true);
    }

    private void ApplyForce(Vector3 pos, float force)
    {
        foreach (var part in _ragdollParts)
        {
            part.rigidbody.AddExplosionForce(force,pos,1f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb.velocity.sqrMagnitude > 10f)
        {

            if (_animator.enabled)
            {
                _animator.enabled = false;
                ToggleRagdoll(true);
            }
        }
    }
}
