using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] public int dmg = 10;
    [SerializeField] public bool friendly = true;
    
    private ParticleSystem _particleSystem;
    private ExplosionController _explosionController;
    private AudioSource _audioSource;

    public void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _explosionController = GetComponent<ExplosionController>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Enemy" && friendly)
        {
            var enemyController = other.GetComponent<EnemyController>();
            enemyController.TakeDmg(dmg);
        }
        if (other.transform.name == "Player Collider" && !friendly)
        {
            var playerController = other.GetComponentInParent<PlayerController>();
            playerController.TakeDmg(dmg);
        }
    }

    public void Explosion()
    {
        StartCoroutine(_explosionController.Explode());
    }
    
    private IEnumerator Explode()
    {
        _audioSource.Play();
        _particleSystem.Play();
        yield return new WaitForSeconds(0.1f);
        Destroy(GetComponent<SphereCollider>());
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
