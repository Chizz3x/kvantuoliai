using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public int maxHp = 100;
    [SerializeField] public int damage = 10;
    private int currentHp;
    private int dmg;
    private Animator _animator;
    private GameObject _player;
    private PlayerController _playerController;


    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Move = Animator.StringToHash("Move");
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.transform.name == "Player Collider")
        {
            _animator.SetTrigger(Attack);
            _animator.ResetTrigger(Move);
            _animator.ResetTrigger(Idle);
            _playerController.TakeDmg(damage);
        }
    }

    // Start is called before the first frame update
    private void Awake()    
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        currentHp = maxHp;
        StartCoroutine(SpawnAnimation());
    }
    
    private IEnumerator SpawnAnimation()
    {
        var position = transform.position;
        position = new Vector3(position.x, 20, position.z);
        transform.position = position;
        var oldMS = moveSpeed;
        moveSpeed = 0;
        var startTime = Time.time;
        while (transform.position.y > -1)
        {
            transform.position = Vector3.Lerp(new Vector3(position.x, 20, position.z), new Vector3(position.x, -1, position.z),
                Time.time - startTime);
           yield return null;
        }

        var particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
        yield return new WaitForSeconds(1f);
        moveSpeed = oldMS;
    }
    
    public void TakeDmg(int damageTaken)
    {
        currentHp -= damageTaken;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =
            Vector3.Lerp(transform.position, _player.transform.position, Time.deltaTime * moveSpeed);
        var diff = _player.transform.position - transform.position;
        if (Mathf.Abs(diff.x) >= 0.01f || Mathf.Abs(diff.y) >= 0.01f)
        {
            _animator.SetTrigger(Move);
            _animator.ResetTrigger(Idle);
            var localScale =  transform.GetChild(0).transform.localScale;
            if (diff.x > 0)
            {
                if (localScale.x < 0)
                {
                    transform.GetChild(0).transform.localScale += new Vector3(localScale.x * -2, 0, 0);
                }
            }
            else
            {
                if (localScale.x > 0)
                {
                    transform.GetChild(0).transform.localScale += new Vector3(localScale.x * -2, 0,0);
                }
            }
        }
        else
        {
            _animator.ResetTrigger(Move);
            _animator.SetTrigger(Idle);
        }
    }
}
