using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public GameObject weaponObject;

    private WeaponAnimationController weaponAnimationController;
    CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        weaponAnimationController = weaponObject.GetComponent<WeaponAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        float hSpeed = Input.GetAxis("Horizontal") * moveSpeed;
        float vSpeed = Input.GetAxis("Vertical") * moveSpeed;
        controller.Move(new Vector3(hSpeed, 0 , vSpeed));

        if(Input.GetMouseButtonDown(0) && !weaponAnimationController.animationPlaying)
        {
            var angle = Vector2.Angle(new Vector2(transform.position.x, transform.position.y), Input.mousePosition);
            Debug.Log($"Text: {angle.ToString()}");
            StartCoroutine(weaponAnimationController.TriggerWeaponAnimation());
        }
    }
}
