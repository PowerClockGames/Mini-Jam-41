using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public BuildingAsset asset;
    public Slider buildingProgressBar;

    [SerializeField]
    private static BuildingState buildingState;
    private SpriteRenderer _buildingSprite;
    private float _constructionTimeLeft;

    // Start is called before the first frame update
    void Awake()
    {
        _buildingSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (buildingState == BuildingState.Constructing)
        {
            _constructionTimeLeft -= Time.deltaTime;
            if (_constructionTimeLeft <= 0f)
            {
                _constructionTimeLeft = 0f;
                buildingState = BuildingState.Built;
            }

            buildingProgressBar.value = 1 - (_constructionTimeLeft / asset.constructionTime);
        }
    }

    public void StartConstruction(float time)
    {
        StartCoroutine(Construction(time));
    }

    IEnumerator Construction(float constructionTime)
    {
        _constructionTimeLeft = constructionTime;
        buildingState = BuildingState.Constructing;
        _buildingSprite.sprite = asset.constructionImage;
        yield return new WaitForSeconds(constructionTime);
        _buildingSprite.sprite = asset.buildingImage;
        buildingProgressBar.value = 0;
    }
}

public enum BuildingState
{
    Constructing,
    Built,
    Destroyed
}
