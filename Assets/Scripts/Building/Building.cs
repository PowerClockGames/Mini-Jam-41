using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    [Header("General")]
    public GameObject crystalIcon;
    public BuildingPopBox buildingPopBox;

    [Header("Collider")]
    public BoxCollider2D buildingCollider;
    public BoxCollider2D buildingTileCollider;

    [Header("State")]
    [SerializeField]
    private BuildingState _buildingState;
    [SerializeField]
    private BuildingAsset _asset;

    private SpriteRenderer _buildingSprite;
    private Level _currentLevelAsset;
    private GameObject _ExplosionParticlePrefab;
    private GameObject _ExtinguishParticlePrefab;
    private GameObject _fireParticlePrefab;
    private GameObject _fireParticles;
    private HealthBar _buildingBar;
    private AudioSource _fireSound;
    private ClickToAct _clickActionHandler;

    private Vector3 _crystalTarget;
    private Vector3 _crystalPosition;
    private BoxCollider2D _buildingColliderDefault;

    private int _currentLevel = 0;
    private float _constructionTimeLeft;
    private float _crystalMoveTime = .8f;
    private bool _isGenerating = false;

    void Awake()
    {
        _buildingSprite = GetComponent<SpriteRenderer>();
        _clickActionHandler = GetComponent<ClickToAct>();
        _crystalTarget = new Vector3(11, 6.5f);
        _crystalPosition = crystalIcon.transform.position;
        _fireParticlePrefab = Resources.Load<GameObject>("FireParticle");
        _ExtinguishParticlePrefab = Resources.Load<GameObject>("ExtinguishParticle");
        _ExplosionParticlePrefab = Resources.Load<GameObject>("ExplosionParticle");
    }

    void Update()
    {
        if(IsConstructing())
        {
            Construct();
        }

        if(GameManager.Instance.selectedBuilding)
        {
            ToggleColliders(true);
        }

        if(GameManager.Instance.gameHasEnded)
        {
            _isGenerating = false;
        }
    }

    void OnMouseDown()
    {
        if(GameManager.Instance.selectedBuilding == null && !IsUnderAttack())
        {
            SoundManager.Instance.PlaySound(_asset.buildingSelectedSFX, transform.position);
            switch (_buildingState)
            {
                case BuildingState.Built:
                    string levelString = (_currentLevel + 1).ToString();
                    bool isMaxLevel = _currentLevel == _asset.levels.Length - 1;
                    buildingPopBox.Show(levelString, isMaxLevel);
                    ToggleColliders(false);
                    break;
                case BuildingState.Damaged:
                    ExtinguishFire();
                    break;
            }
        }
    }

    private void OnMouseExit()
    {
        if (buildingPopBox.isOpen)
        {
            buildingPopBox.Hide();
            ToggleColliders(true);
        }
    }

    public void SetAsset(BuildingAsset asset)
    {
        _asset = asset;
        _currentLevelAsset = _asset.levels[_currentLevel];
    }

    public void ToggleColliders(bool isTile)
    {
        buildingCollider.enabled = !isTile;
        buildingTileCollider.enabled = isTile;
    }

    private void ExtinguishFire()
    {
        _clickActionHandler.SetMultiClickAction(4, BlowUpBuilding, ShowClickBurst);
    }

    private void ShowClickBurst()
    {
        if (_ExtinguishParticlePrefab != null)
        {
            Instantiate(_ExtinguishParticlePrefab, gameObject.transform.position, Quaternion.identity);
        }

        SoundManager.Instance.PlaySound(_asset.buildingExtinguishSFX, transform.position);
        CameraShake.Shake(0.25f, 0.3f);
    }

    private void BlowUpBuilding()
    {
        if (_ExplosionParticlePrefab != null)
        {
            Instantiate(_ExplosionParticlePrefab, gameObject.transform.position, Quaternion.identity);
        }

        CameraShake.Shake(0.3f, 0.4f);
        GameManager.Instance.isHouseOnFire = false;
        GameManager.Instance.RemoveBuilding(gameObject);
        SoundManager.Instance.PlaySound(_asset.buildingDestroyedSFX, transform.position);
        SoundManager.Instance.StopLoopingSound(_fireSound);
        Destroy(gameObject);
        Destroy(_fireParticles);
    }

    public void IncreaseBuildingLevel()
    {
        if(_currentLevel < _asset.levels.Length - 1)
        {
            Level upgradeAsset = _asset.levels[_currentLevel + 1];
            if (upgradeAsset != null && IsBuilt())
            {
                PromptAndUpgrade(upgradeAsset);
            }
        }
    }

    private void PromptAndUpgrade(Level levelAsset)
    {
        UIManager.Instance.ShowPopup(string.Format("Do you really want to upgrade {0} for {1} Magic?", _asset.buildingName, levelAsset.levelCost), false, () =>
        {
            if (GameManager.Instance.crystalAmount > levelAsset.levelCost)
            {
                buildingPopBox.Hide();
                _currentLevelAsset = levelAsset;
                GameManager.Instance.DecreaseCrystals(_currentLevelAsset.levelCost);
                SoundManager.Instance.PlaySound(_asset.buildingUpgradedSFX, transform.position);
                StartConstruction(_currentLevelAsset);
                _currentLevel++;
            }
            else
            {
                UIManager.Instance.ShowPopup("You dont have enough Magic to buy this.", true, () => { });
            }
        });

    }

    public void StartConstruction(Level levelAsset)
    {
        GameManager.Instance.AddBuilding(gameObject);

        if(_isGenerating)
        {
            _isGenerating = false;
        }

        _buildingBar = HealthBar.Create(gameObject.transform.position + new Vector3(0,0.5f,0), new Vector3(0.2f, 0.02f), Color.blue, Color.gray);
        StartCoroutine(Construction(levelAsset, () =>
        {
             SoundManager.Instance.PlaySound(_asset.buildingConstructedSFX, transform.position);
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
            SetBuilt();
        }

        _buildingBar.SetSize(1 - (_constructionTimeLeft / _currentLevelAsset.levelConstructionTime));
    }

    IEnumerator Construction(Level levelAsset, System.Action onBuildingConstructed)
    {
        _constructionTimeLeft = levelAsset.levelConstructionTime;
        _buildingSprite.sprite = _asset.constructionImage;
        SetConstructing();

        yield return new WaitForSeconds(levelAsset.levelConstructionTime);

        _buildingBar.Destroy();
        _buildingSprite.sprite = levelAsset.levelSprite;
        onBuildingConstructed();
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
                SoundManager.Instance.PlaySound(_asset.magicGeneratedSFX, transform.position);
                crystalIcon.SetActive(false);
            });
            yield return new WaitForSeconds(5);
        }

    }

    public void DamageBuilding()
    {
        _isGenerating = false;
        SetDamaged();
        GameManager.Instance.isHouseOnFire = true;
        _fireSound = SoundManager.Instance.PlaySound(_asset.buildingDamagedSFX, transform.position, true);

        if (_fireParticlePrefab != null)
        {
            Vector3 firePosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.05f);
            _fireParticles = Instantiate(_fireParticlePrefab, firePosition, Quaternion.identity);
        }

    }

    public void SetDamaged()
    {
        _buildingState = BuildingState.Damaged;
    }

    public void SetConstructing()
    {
        _buildingState = BuildingState.Constructing;
    }

    public void SetBuilt()
    {
        _buildingState = BuildingState.Built;
    }

    public void SetUnderAttack()
    {
        _buildingState = BuildingState.UnderAttack;
    }

    public bool IsUnderAttack()
    {
        return _buildingState == BuildingState.UnderAttack;
    }

    public bool IsDamaged()
    {
        return _buildingState == BuildingState.Damaged;
    }

    public bool IsBuilt()
    {
        return _buildingState == BuildingState.Built;
    }

    public bool IsConstructing()
    {
        return _buildingState == BuildingState.Constructing;
    }
}

public enum BuildingState
{
    Constructing,
    Built,
    UnderAttack,
    Damaged,
}
