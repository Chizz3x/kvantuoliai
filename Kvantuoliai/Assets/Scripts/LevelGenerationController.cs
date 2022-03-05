using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerationController : MonoBehaviour
{
    [SerializeField] public Sprite grassSprite;
    [SerializeField] public Sprite swampSprite;
    [SerializeField] public Sprite stoneSprite;
    [SerializeField] public GameObject enemyTemplate;
    [SerializeField] public GameObject levelTemplate;
    [SerializeField] public GameObject enemyContainer;
    [SerializeField] public GameObject[] foliage;

    private GameObject _currentLevel;
    private int currentLevel = 1;
    
    private void Awake()
    {
        GenerateLevel(grassSprite, 4);
    }

    private void Update()
    {
        if (enemyContainer.transform.childCount == 0)
        {
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
                    GenerateLevel(stoneSprite, 8);
                    break;
                default:
                    GenerateLevel(stoneSprite, currentLevel++);
                    break;
            }
        }
    }

    

    private void GenerateLevel(Sprite sprite, int enemyCount)
    {
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        _currentLevel = Instantiate(levelTemplate, transform);
        _currentLevel.transform.position = Vector3.zero;

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
}
