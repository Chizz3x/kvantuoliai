using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public GameObject weaponObject;
    [SerializeField] public float speedThreshold = 0.01f;

    private WeaponAnimationController _weaponAnimationController;
    private Animator _animator;
    CharacterController _controller;
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
    }

    // Update is called once per frame
    void Update()
    {
        float hSpeed = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float vSpeed = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        _controller.Move(new Vector3(hSpeed, 0 , vSpeed));
    
        Debug.Log(hSpeed);
        
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
            var transform1 = transform;
            var angle = Vector2.Angle(new Vector2(transform1.position.x, transform1.position.y), Input.mousePosition);
            Debug.Log($"Text: {angle.ToString()}");
            StartCoroutine(_weaponAnimationController.TriggerWeaponAnimation());
        }
    }
}
