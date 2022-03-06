using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerationController : MonoBehaviour
{
    [SerializeField] public Sprite grassSprite;
    [SerializeField] public Sprite swampSprite;
    [SerializeField] public Sprite stoneSprite;
    [SerializeField] public GameObject enemyTemplate;
    [SerializeField] public GameObject bossTemplate;
    [SerializeField] public GameObject levelTemplate;
    [SerializeField] public GameObject enemyContainer;
    [SerializeField] public GameObject[] foliage;
    

    private GameObject _currentLevel;
    private GameObject _currentPortal;
    private int currentLevel = 2;
    private bool portal = false;
    
    private void Awake()
    {
        GenerateLevel(grassSprite, 4);
    }

    private IEnumerator PortalActivation()
    {
        var portalController = _currentPortal.GetComponent<PortalController>();
        yield return new WaitUntil(() => portalController.activated == true);
        Destroy(_currentLevel);
        switch (currentLevel++)
        {
            case 1:
                GenerateLevel(grassSprite, 5);
                break;
            case 2:
                GenerateLevel(stoneSprite, 7);
                break;
            case 3:
                GenerateBossLevel();
                break;
            default:
                GenerateLevel(stoneSprite, currentLevel++);
                break;
        }

        portal = false;
    }

    private void Update()
    {
        if (enemyContainer.transform.childCount == 0 && !portal)
        {
            portal = true;
            _currentPortal.SetActive(true);
            StartCoroutine(PortalActivation());
        }
    }

    

    private void GenerateLevel(Sprite sprite, int enemyCount)
    {
        var ground = transform.GetChild(0).GetChild(1);
        var spriteRenderer = ground.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        _currentLevel = Instantiate(levelTemplate, transform);
        _currentLevel.transform.position = Vector3.zero;
        _currentPortal = _currentLevel.transform.GetChild(0).gameObject;
        _currentPortal.SetActive(false);
        for (var i = 0; i < enemyCount; i++)
        {
            var randX = Random.Range(-20, 20);
            var randZ = Random.Range(-15, 15);
            var newEnemy = Instantiate(enemyTemplate, new Vector3(randX, 20, randZ),
                new Quaternion(), enemyContainer.transform);
            newEnemy.transform.name = "Enemy";
            var enemyController = newEnemy.GetComponent<EnemyController>();
            enemyController.enabled = true;
        }

        for (var i = 0; i < 20; i++)
        {
            var randX = Random.Range(-20, 20);
            var randZ = Random.Range(-15, 15);
            var foliageType = Random.Range(0, foliage.Length);
            var fol = Instantiate(foliage[foliageType], _currentLevel.transform);
            fol.transform.position = new Vector3(randX, 0, randZ);
        }
    }
    
    private void GenerateBossLevel()
    {
        var ground = transform.GetChild(0).GetChild(1);
        var spriteRenderer = ground.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = stoneSprite;
        _currentLevel = Instantiate(levelTemplate, transform);
        _currentLevel.transform.position = Vector3.zero;
        _currentPortal = _currentLevel.transform.GetChild(0).gameObject;
        _currentPortal.SetActive(false);

        var boss = Instantiate(bossTemplate, enemyContainer.transform);
        var bossController = boss.GetComponent<BossController>();
        bossController.enabled = true;
        boss.transform.position = transform.position;

        for (var i = 0; i < 20; i++)
        {
            var randX = Random.Range(-20, 20);
            var randZ = Random.Range(-15, 15);
            var foliageType = Random.Range(0, foliage.Length);
            var fol = Instantiate(foliage[foliageType], _currentLevel.transform);
            fol.transform.position = new Vector3(randX, 0, randZ);
        }
    }
}
