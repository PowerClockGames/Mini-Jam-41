using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base code by Brackeys
public class Bee : MonoBehaviour
{
	public float flySpeed = 5f;
	public Vector3 flyOffTo;
    public Animator beeAnimator;

    public bool hasLanded = false;
	public bool hasEaten = false;

	private Building _target;
	private float _countdown = 4f;
    private AudioSource _flyingAudioSource;

    public void SetTarget (Building t, AudioSource sfx)
	{
		_target = t;
        _flyingAudioSource = sfx;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    private void Update()
	{
		if (_target != null)
		{
            if(_target.buildingState == BuildingState.Constructing)
            {
                _target = null;
            }

			if (Vector2.Distance(transform.position, _target.transform.position) > .1f)
			{
				Vector2 dir = (_target.transform.position - transform.position).normalized;
				transform.Translate(dir * flySpeed * Time.deltaTime);
			} else
			{
				GetComponent<CircleCollider2D>().enabled = true;
                _countdown -= Time.deltaTime;
                beeAnimator.SetBool("IsAttacking", true);
				if (_countdown <= 0f)
                {
                    _target.SetDamaged();
                    beeAnimator.SetBool("IsAttacking", false);
                    _target = null;
				}
			}
		} else
		{
			Vector2 dir = (flyOffTo - transform.position).normalized;
			transform.Translate(dir * flySpeed * Time.deltaTime);

			if (Vector2.Distance(transform.position, flyOffTo) <= .1f)
			{
                SoundManager.Instance.StopLoopingSound(_flyingAudioSource);
                Destroy(gameObject);
			}
		}
	}

}
