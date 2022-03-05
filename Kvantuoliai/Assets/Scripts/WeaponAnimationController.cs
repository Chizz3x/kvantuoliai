using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    [SerializeField] public GameObject parentObject;
    [SerializeField] public float moveSpeed = 2f;
    
    private Animator _animator;
    private AudioSource _audioSource;
    public bool animationPlaying;
    private static readonly int Dog = Animator.StringToHash("Dog");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Move = Animator.StringToHash("Move");

    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        var parent = gameObject.transform.parent;
        parent.transform.position =
            Vector3.Lerp(parent.transform.position, parentObject.transform.position, Time.deltaTime * moveSpeed);
        var diff = parent.transform.position - parentObject.transform.position;
        if (Mathf.Abs(diff.x) >= 0.01f || Mathf.Abs(diff.y) >= 0.01f)
        {
            _animator.SetTrigger(Move);
            _animator.ResetTrigger(Idle);
            var localScale = transform.localScale;
            if (diff.x < 0)
            {
                if (localScale.x < 0)
                {
                    transform.localScale += new Vector3(localScale.x * -2, 0, 0);
                }
            }
            else
            {
                if (localScale.x > 0)
                {
                    transform.localScale += new Vector3(localScale.x * -2, 0,0);
                }
            }
        }
        else
        {
            _animator.ResetTrigger(Move);
            _animator.SetTrigger(Idle);
        }
    }

    public IEnumerator TriggerWeaponAnimation()
    {
        _animator.SetTrigger(Dog);
        _animator.ResetTrigger(Idle);
        _audioSource.Play();
        animationPlaying = true;
        yield return new WaitForSeconds(0.1f);
        animationPlaying = false;
    }
}
