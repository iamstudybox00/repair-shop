using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public void StartParticle(Vector3 position)
    {
        transform.position = position;
        gameObject.GetComponent<ParticleSystem>().Play();
    }
}
