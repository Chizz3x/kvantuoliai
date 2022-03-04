using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    public bool animationPlaying;
    private static readonly int Dog = Animator.StringToHash("Dog");
    private static readonly int Idle = Animator.StringToHash("Idle");

    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TriggerWeaponAnimation()
    {
        Debug.Log("Animation starting");
        _animator.SetTrigger(Dog);
        _audioSource.Play();
        animationPlaying = true;
        Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        _animator.SetTrigger(Idle);
        animationPlaying = false;
        Debug.Log("Animation over");
    }
}
