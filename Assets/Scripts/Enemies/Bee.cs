using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base code by Brackeys additional by me
[RequireComponent(typeof(CircleCollider2D))]
public class Bee : MonoBehaviour
{
    [Header("General")]
    public float flySpeed = 5f;
    public Vector3[] flyOffToPositions;
    public Animator beeAnimator;

    [Header("Audio")]
    public AudioClip beeHurtSFX;

    private Building _target;
    private AudioSource _flyingAudioSource;
    private ClickToAct _clickActionHandler;
    private GameObject _HurtParticlePrefab;
    private Vector3 _flyOffTo;
    private float _cooldown = 4f;
    private int _maxClicksToDestroy = 0;
    private bool hasLanded = false;

    private void Awake()
    {
        _clickActionHandler = GetComponent<ClickToAct>();
        _HurtParticlePrefab = Resources.Load<GameObject>("ExtinguishParticle");
        _maxClicksToDestroy = Random.Range(0, 4);
        _flyOffTo = flyOffToPositions[Random.Range(0, flyOffToPositions.Length - 1)];
    }

    public void SetTarget (Building t, AudioSource sfx)
	{
		_target = t;
        _flyingAudioSource = sfx;

    }

    private void OnMouseDown()
    {
        if(!hasLanded)
        {
            _clickActionHandler.SetMultiClickAction(_maxClicksToDestroy, ChaseBeeAway, HurtBee);
        }
    }

    private void ChaseBeeAway()
    {
        if(_target != null)
        {
            _target.SetBuilt();
            _target = null;
        }
    }

    private void HurtBee()
    {
        if (_HurtParticlePrefab != null)
        {
            Instantiate(_HurtParticlePrefab, transform.position, Quaternion.identity);
        }

        SoundManager.Instance.PlaySound(beeHurtSFX, transform.position);
        CameraShake.Shake(0.25f, 0.3f);
    }

    private void Update()
	{
		if (_target != null)
		{
            if(_target.IsConstructing())
            {
                _target = null;
            }

			if (Vector2.Distance(transform.position, _target.transform.position) > .1f)
			{
				Vector2 dir = (_target.transform.position - transform.position).normalized;
				transform.Translate(dir * flySpeed * Time.deltaTime);
			} else
			{
                hasLanded = true;
				GetComponent<CircleCollider2D>().enabled = true;
                _cooldown -= Time.deltaTime;
                beeAnimator.SetBool("IsAttacking", true);
				if (_cooldown <= 0f)
                {
                    hasLanded = false;
                    _target.DamageBuilding();
                    beeAnimator.SetBool("IsAttacking", false);
                    _target = null;
                }
			}
		} else
		{
			Vector2 dir = (_flyOffTo - transform.position).normalized;
			transform.Translate(dir * flySpeed * Time.deltaTime);

			if (Vector2.Distance(transform.position, _flyOffTo) <= .1f)
			{
                SoundManager.Instance.StopLoopingSound(_flyingAudioSource);
                Destroy(gameObject);
			}
		}
	}

}
