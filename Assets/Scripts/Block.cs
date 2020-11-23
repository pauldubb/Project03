using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public ParticleSystem dirtParticle;
    private MeshRenderer _visuals;
    private Collider _collider;

    // Start is called before the first frame update
    void Awake()
    {
        _visuals = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
    }

    public void Die()
    {
        dirtParticle.Play();
        _visuals.enabled = false;
        _collider.enabled = false;
        Destroy(gameObject,2);
    }

}
