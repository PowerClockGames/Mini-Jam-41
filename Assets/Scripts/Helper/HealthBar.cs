using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public static HealthBar Create(Vector3 position, Vector3 size, Color barColor, Color backgroundColor) {
        Sprite barBase = Resources.Load<Sprite>("white_pixel");
        GameObject healthBarGameObject = new GameObject("HealthBar");
        healthBarGameObject.transform.position = position;

        // Background
        GameObject backgroundGameObject = new GameObject("Background", typeof(SpriteRenderer));
        backgroundGameObject.transform.SetParent(healthBarGameObject.transform);
        backgroundGameObject.transform.localPosition = Vector3.zero;
        backgroundGameObject.transform.localScale = size;
        backgroundGameObject.GetComponent<SpriteRenderer>().color = backgroundColor;
        backgroundGameObject.GetComponent<SpriteRenderer>().sprite = barBase;
        backgroundGameObject.GetComponent<SpriteRenderer>().sortingOrder = 100;

        // Bar
        GameObject barGameObject = new GameObject("Bar");
        barGameObject.transform.SetParent(healthBarGameObject.transform);
        barGameObject.transform.localPosition = new Vector3(- .5f, 0f);

        // Bar Sprite
        GameObject barSpriteGameObject = new GameObject("BarSprite", typeof(SpriteRenderer));
        barSpriteGameObject.transform.SetParent(barGameObject.transform);
        barSpriteGameObject.transform.localPosition = new Vector3(.5f, 0f);
        barSpriteGameObject.transform.localScale = size;
        barSpriteGameObject.GetComponent<SpriteRenderer>().color = barColor;
        barSpriteGameObject.GetComponent<SpriteRenderer>().sprite = barBase;
        barSpriteGameObject.GetComponent<SpriteRenderer>().sortingOrder = 110;

        HealthBar healthBar = healthBarGameObject.AddComponent<HealthBar>();

        return healthBar;
    }

    private Transform bar;

	private void Awake () {
        bar = transform.Find("Bar");
	}

    public void SetSize(float sizeNormalized) {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void SetColor(Color color) {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = color;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
