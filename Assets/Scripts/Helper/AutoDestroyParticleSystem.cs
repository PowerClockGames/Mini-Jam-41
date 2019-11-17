using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticleSystem : MonoBehaviour
{
    private ParticleSystem ps;
 
 
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
