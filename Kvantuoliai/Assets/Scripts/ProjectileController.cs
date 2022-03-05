using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] public float lifeTime = 2f;
    [SerializeField] public float speedMultiplier = 2f;
    [SerializeField] public int dmg = 20;
    [SerializeField] public bool piercing = false;
    [SerializeField] public float explodeTime = 1f;
    [SerializeField] public GameObject exploder;
    public bool friendly = true;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var hitName = other.transform.name;
        if (hitName == "Enemy" && friendly)
        {
            var enemyComponent = other.GetComponent<EnemyController>();
            enemyComponent.TakeDmg(dmg);
            if (explodeTime > 0)
            {
                Explode();
            }
            if (!piercing)
            {
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator Fire(Vector3 forceVector)
    {
        _rigidbody.AddForce(forceVector * speedMultiplier);
        if (explodeTime > 0)
        {
            yield return new WaitForSeconds(explodeTime);
            if (this != null)
            {
                Explode();
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject, lifeTime);
        }
    }

    private void Explode()
    {
        var newExploder = Instantiate(exploder);
        newExploder.transform.position = transform.position;
        var explosionController = newExploder.GetComponent<ExplosionController>();
        explosionController.Explosion();
    }
}
