using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossController : MonoBehaviour
{
    private Animator _animator;
    private bool _inAnimation = false;
    private GameObject player;
    private ParticleSystem _particleSystem;

    [SerializeField] public GameObject projectile;
    
    private static readonly int Attack4 = Animator.StringToHash("Attack1");
    private static readonly int Attack5 = Animator.StringToHash("Attack2");
    private static readonly int Attack6 = Animator.StringToHash("Attack3");

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (!_inAnimation)
        {
            StartCoroutine(Attack());
        }
    }

    // Start is called before the first frame update
    public IEnumerator Attack()
    {
        _inAnimation = true;
        var random = Random.Range(1, 4);
        switch (random)
        {
            case 1:
                yield return Attack1();
                break;
            case 2:
                yield return Attack2();
                break;
            case 3:
                yield return Attack3();
                break;
        }
        yield return new WaitForSeconds(1f);
        _inAnimation = false;
    }
    
    public IEnumerator Attack1()
    {
        _animator.SetTrigger(Attack4);
        _particleSystem.Play();
        yield return new WaitForSeconds(2f);
    }
    
    public IEnumerator Attack2()
    {
        _animator.SetTrigger(Attack5);
        yield return new WaitForSeconds(2f);
    }
    
    public IEnumerator Attack3()
    {
        _animator.SetTrigger(Attack6);
        
        var proj = Instantiate(projectile);
        var position = transform.position;
        proj.transform.position = position;
        var projController = proj.GetComponent<ProjectileController>();
        
        var dir = (player.transform.position - position).normalized;
        
        StartCoroutine(projController.Fire(dir));
        
        yield return new WaitForSeconds(1f);
    }
}
