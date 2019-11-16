using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{

	public float flySpeed = 5f;

	public Vector3 flyOffTo;

	public bool hasLanded = false;
	public bool hasEaten = false;

	GameObject target;

	private float countdown = 4f;

	public void SetTarget (GameObject t)
	{
		target = t;
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Player"))
		{
			target = null;
		}
	}

	private void Update()
	{
		if (target != null)
		{
			if (Vector2.Distance(transform.position, target.transform.position) > .1f)
			{
				Vector2 dir = (target.transform.position - transform.position).normalized;
				transform.Translate(dir * flySpeed * Time.deltaTime);
			} else
			{
				GetComponent<CircleCollider2D>().enabled = true;
				countdown -= Time.deltaTime;
				if (countdown <= 0f)
				{
					target = null;
				}
			}
		} else
		{
			Vector2 dir = (flyOffTo - transform.position).normalized;
			transform.Translate(dir * flySpeed * Time.deltaTime);

			if (Vector2.Distance(transform.position, flyOffTo) <= .1f)
			{
				Destroy(gameObject);
			}
		}
	}

}
