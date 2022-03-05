using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] public int maxHp;
    [SerializeField] public float moveSpeed;
    [SerializeField] public GameObject weaponObject;
    [SerializeField] public float speedThreshold = 0.01f;
    [SerializeField] public GameObject projectileObject;
    [SerializeField] public Camera mainCamera;

    private WeaponAnimationController _weaponAnimationController;
    private Animator _animator;
    CharacterController _controller;
    private int currentHp;
    private AudioSource hurtSource;
    private AudioSource deathSource;

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int WalkRight = Animator.StringToHash("WalkRight");
    private static readonly int WalkLeft = Animator.StringToHash("WalkLeft");
    private static readonly int WalkUp = Animator.StringToHash("WalkUp");

    // Start is called before the first frame update
    void Start()
    {
        _controller = gameObject.AddComponent<CharacterController>();
        _weaponAnimationController = weaponObject.GetComponent<WeaponAnimationController>();
        _animator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
        currentHp = maxHp;
        var audioSources = GetComponents<AudioSource>();
        hurtSource = audioSources[0];
        deathSource = audioSources[1];
    }

    public void TakeDmg(int dmg)
    {
        currentHp -= dmg;
        hurtSource.Play();
        if (currentHp <= 0)
        {
            deathSource.Play();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Mathf.Clamp(transform.position.x, -10, 10); 
        var hSpeed = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var vSpeed = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        _controller.Move(new Vector3(hSpeed, 0 , vSpeed));
        
        
        if (MathF.Abs(hSpeed) <= speedThreshold && vSpeed <= speedThreshold)
        {
            _animator.SetTrigger(Idle);
            _animator.ResetTrigger(WalkRight);
            _animator.ResetTrigger(WalkLeft);
            _animator.ResetTrigger(WalkUp);
        }
        
        if (vSpeed >= speedThreshold)
        {
            _animator.SetTrigger(WalkUp);
            _animator.ResetTrigger(Idle);
            _animator.ResetTrigger(WalkRight);
            _animator.ResetTrigger(WalkLeft);
            
        }
        
        if (hSpeed > speedThreshold)
        {
            _animator.SetTrigger(WalkRight);
            _animator.ResetTrigger(Idle);
            _animator.ResetTrigger(WalkLeft);
            _animator.ResetTrigger(WalkUp);
        }
        
        if (hSpeed < -speedThreshold)
        {
            _animator.SetTrigger(WalkLeft);
            _animator.ResetTrigger(WalkRight);
            _animator.ResetTrigger(Idle);
            _animator.ResetTrigger(WalkUp);
        }

        if(Input.GetMouseButtonDown(0) && !_weaponAnimationController.animationPlaying)
        {
            int layerMask = LayerMask.GetMask("Colliders");
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, layerMask)) {
                var position = weaponObject.transform.position;
                var dir = (hit.point - position).normalized;
                StartCoroutine(_weaponAnimationController.TriggerWeaponAnimation());
                var newProjectile = Instantiate(projectileObject);
                newProjectile.transform.position = position;
                var projectileController = newProjectile.GetComponent<ProjectileController>();
                StartCoroutine(projectileController.Fire(dir));
            }

        }
    }
}   
