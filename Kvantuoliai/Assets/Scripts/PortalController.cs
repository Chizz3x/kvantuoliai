using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] public bool activated;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player Collider" && !activated)
        {
            StartCoroutine(PortalActivation());
        }
    }

    private IEnumerator PortalActivation()
    {
        var particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
        yield return new WaitForSeconds(1f);
        activated = true;
    }
}
