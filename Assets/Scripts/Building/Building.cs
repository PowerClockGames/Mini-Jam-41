using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public BuildingAsset asset;
    public GameObject crystalIcon;
    public BuildingPopBox buildingPopBox;
    public BuildingState buildingState;

    private SpriteRenderer _buildingSprite;
    private Level _currentLevelAsset;
    private GameObject _fireParticlePrefab;
    private GameObject _fireParticles;
    private HealthBar _buildingBar;

    private Vector3 _crystalTarget;
    private Vector3 _crystalPosition;

    private int _currentLevel = 0;
    private int _clickCount = 0;
    private float _constructionTimeLeft;
    private float _crystalMoveTime = .8f;
    private bool _isGenerating = false;

    void Awake()
    {
        _buildingSprite = gameObject.GetComponent<SpriteRenderer>();
        _crystalTarget = new Vector3(11, 6.5f);
        _crystalPosition = crystalIcon.transform.position;
        _currentLevelAsset = asset.levels[_currentLevel];
        _fireParticlePrefab = Resources.Load<GameObject>("Fire");
    }

    // Update is called once per frame
    void Update()
    {

        switch(buildingState)
        {
            case BuildingState.Constructing:
                Construct();
                break;
        }
    }

    void OnMouseDown()
    {
        Debug.Log("clicked on building");
        if(buildingState == BuildingState.Built)
        {
            buildingPopBox.Show();
        }

        if(buildingState == BuildingState.Damaged)
        {
            Debug.Log(_clickCount);
            if (_clickCount == 4)
            {
                _clickCount = 0;
                Destroy(gameObject);
                Destroy(_fireParticles);
            } else
            {
                _clickCount++;
            }
        }
    }

    private void OnMouseExit()
    {
        if(buildingPopBox.isOpen)
        {
            buildingPopBox.Hide();
        }
    }

    public void IncreaseBuildingLevel()
    {
        if(_currentLevel <= asset.levels.Length)
        {
            Level upgradeAsset = asset.levels[_currentLevel + 1];
            if (upgradeAsset != null)
            {
                promptAndUpgrade(upgradeAsset);
            }
        }
    }

    private void promptAndUpgrade(Level levelAsset)
    {
        if (GameManager.Instance.crystalAmount >= levelAsset.levelCost)
        {
            UIManager.Instance.ShowPopup(string.Format("Do you really want to upgrade {0} for {1}?", asset.buildingName, levelAsset.levelCost), false, () =>
            {
                buildingPopBox.Hide();
                _currentLevelAsset = levelAsset;
                GameManager.Instance.DecreaseCrystals(_currentLevelAsset.levelCost);
                StartConstruction(_currentLevelAsset);
                _currentLevel++;
            });

        }
        else
        {
            UIManager.Instance.ShowPopup("You dont have enough magic to buy this.", true, () => { });
        }
    }

    public void StartConstruction(Level levelAsset)
    {
        GameManager.Instance.AddBuilding(gameObject);

        if(_isGenerating)
        {
            _isGenerating = false;
        }

        _buildingBar = HealthBar.Create(gameObject.transform.position + Vector3.up, new Vector3(0.2f, 0.02f), Color.blue, Color.gray);
        StartCoroutine(Construction(levelAsset, () =>
        {
            if(asset.constructionCompletedSFX != null)
            {
                // Play SFX
            }
            _isGenerating = true;
            StartCoroutine(GenerateCrystals(levelAsset.crystalsPerMinute));

        }));
    }

    private void Construct()
    {
        _constructionTimeLeft -= Time.deltaTime;
        if (_constructionTimeLeft <= 0f)
        {
            _constructionTimeLeft = 0;
            buildingState = BuildingState.Built;
        }

        _buildingBar.SetSize(1 - (_constructionTimeLeft / _currentLevelAsset.levelConstructionTime));
    }

    IEnumerator Construction(Level levelAsset, System.Action done)
    {
        _constructionTimeLeft = levelAsset.levelConstructionTime;
        _buildingSprite.sprite = asset.constructionImage;
        buildingState = BuildingState.Constructing;

        yield return new WaitForSeconds(levelAsset.levelConstructionTime);

        _buildingSprite.sprite = levelAsset.levelSprite;
        _buildingBar.Destroy();
        done();
    }

    IEnumerator GenerateCrystals(int cpm)
    {
        while(_isGenerating)
        {
            crystalIcon.transform.position = _crystalPosition;
            crystalIcon.SetActive(true);
            LeanTween.move(crystalIcon, _crystalTarget, _crystalMoveTime).setEaseInCubic();
            LeanTween.delayedCall(crystalIcon, _crystalMoveTime + 0.1f, () => {
                GameManager.Instance.IncreaseCrystals(cpm);
                crystalIcon.SetActive(false);
            });
            yield return new WaitForSeconds(5);
        }

    }

    public void SetDamaged()
    {
        _isGenerating = false;
        buildingState = BuildingState.Damaged;

        if(_fireParticlePrefab != null)
        {
            _fireParticles = Instantiate(_fireParticlePrefab, gameObject.transform.position, Quaternion.identity);
        }

    }

    public void SetUnderAttack()
    {
        buildingState = BuildingState.UnderAttack;
    }

    public bool IsUnderAttack()
    {
        return buildingState == BuildingState.UnderAttack;
    }
}

public enum BuildingState
{
    Constructing,
    Built,
    UnderAttack,
    Damaged,
}
