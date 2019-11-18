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
    public BuildingState buildingState;
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

    private Vector3 _crystalTarget;
    private Vector3 _crystalPosition;
    private BoxCollider2D _buildingColliderDefault;

    private int _currentLevel = 0;
    private int _clickCount = 0;
    private float _constructionTimeLeft;
    private float _crystalMoveTime = .8f;
    private bool _isGenerating = false;

    void Awake()
    {
        _buildingSprite = GetComponent<SpriteRenderer>();
        _crystalTarget = new Vector3(11, 6.5f);
        _crystalPosition = crystalIcon.transform.position;
        _fireParticlePrefab = Resources.Load<GameObject>("FireParticle");
        _ExtinguishParticlePrefab = Resources.Load<GameObject>("ExtinguishParticle");
        _ExplosionParticlePrefab = Resources.Load<GameObject>("ExplosionParticle");
    }

    void Update()
    {
        if(buildingState == BuildingState.Constructing)
        {
            Construct();
        }

        if(GameManager.Instance.selectedBuilding)
        {
            ToggleColliders(true);
        } else
        {
            ToggleColliders(false);
        }

        if(GameManager.Instance.gameHasEnded)
        {
            _isGenerating = false;
        }

    }

    void OnMouseDown()
    {
        if(GameManager.Instance.selectedBuilding == null && buildingState != BuildingState.UnderAttack)
        {
            SoundManager.Instance.PlaySound(_asset.buildingSelectedSFX, transform.position);
            switch (buildingState)
            {
                case BuildingState.Built:
                    buildingPopBox.Show((_currentLevel + 1).ToString());

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
        if (_clickCount == 4)
        {
            _clickCount = 0;

            if(_ExplosionParticlePrefab != null)
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
        else
        {
            if (_ExtinguishParticlePrefab != null)
            {
                Instantiate(_ExtinguishParticlePrefab, gameObject.transform.position, Quaternion.identity);
            }

            SoundManager.Instance.PlaySound(_asset.buildingExtinguishSFX, transform.position);     
            CameraShake.Shake(0.25f, 0.3f);
            _clickCount++;
        }
    }

    public void IncreaseBuildingLevel()
    {
        if(_currentLevel + 1 < _asset.levels.Length)
        {
            Level upgradeAsset = _asset.levels[_currentLevel + 1];
            if (upgradeAsset != null && buildingState == BuildingState.Built)
            {
                promptAndUpgrade(upgradeAsset);
            }
        }
    }

    private void promptAndUpgrade(Level levelAsset)
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
            buildingState = BuildingState.Built;
        }

        _buildingBar.SetSize(1 - (_constructionTimeLeft / _currentLevelAsset.levelConstructionTime));
    }

    IEnumerator Construction(Level levelAsset, System.Action done)
    {
        _constructionTimeLeft = levelAsset.levelConstructionTime;
        _buildingSprite.sprite = _asset.constructionImage;
        buildingState = BuildingState.Constructing;

        yield return new WaitForSeconds(levelAsset.levelConstructionTime);

        _buildingBar.Destroy();
        _buildingSprite.sprite = levelAsset.levelSprite;
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
                SoundManager.Instance.PlaySound(_asset.magicGeneratedSFX, transform.position);
                crystalIcon.SetActive(false);
            });
            yield return new WaitForSeconds(5);
        }

    }

    public void SetDamaged()
    {
        _isGenerating = false;
        buildingState = BuildingState.Damaged;
        GameManager.Instance.isHouseOnFire = true;
        _fireSound = SoundManager.Instance.PlaySound(_asset.buildingDamagedSFX, transform.position, true);

        if (_fireParticlePrefab != null)
        {
            Vector3 firePosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.05f);
            _fireParticles = Instantiate(_fireParticlePrefab, firePosition, Quaternion.identity);
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

    public bool IsDamaged()
    {
        return buildingState == BuildingState.Damaged;
    }
}

public enum BuildingState
{
    Constructing,
    Built,
    UnderAttack,
    Damaged,
}
