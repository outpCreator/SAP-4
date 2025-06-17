using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    GameObject spawnPoint;

    public Material outlineMaterial;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.InitLevel();
        }

        if (LevelSpawner.Instance != null)
        {
            LevelSpawner.Instance.InitLevel();
        }

        if (LevelManager.Instance == null || LevelSpawner.Instance == null && PlayerManager.Instance != null)
        {
            Debug.Log("Not in a Level! Initialized from Game Manger");

            spawnPoint = GameObject.FindWithTag("SpawnPoint");

            PlayerManager.Instance.InitPlayer();
            PlayerManager.Instance.SetSpawnPoint(spawnPoint.transform);
        }

        if (FightManager.Instance != null)
        {
            Debug.Log("Fight Manager is not null!");
            FightManager.Instance.InitFightManager();
            FightManager.Instance.GetEnemies();
        }
    }

    private void Update()
    {
        outlineTargetsB.Clear();
        foreach(OutlineInstance outline in outlineTargets)
        {
            
            Graphics.DrawMesh(
                outline.meshFilter.sharedMesh, 
                outline.meshFilter.transform.localToWorldMatrix, 
                outlineMaterial, 
                0, 
                null, 
                0, 
                outline.propertyBlock);

            outline.timeLeft -= Time.deltaTime;
            if(outline.timeLeft >= 0.0)
            {
                outlineTargetsB.Add(outline);
            }
        }

        outlineTargets.Clear();
        outlineTargets.AddRange(outlineTargetsB);

        
    }

    class OutlineInstance
    {
        public MeshFilter meshFilter;
        public double timeLeft;
        public MaterialPropertyBlock propertyBlock;
    }
    List<OutlineInstance> outlineTargets = new List<OutlineInstance>();
    List<OutlineInstance> outlineTargetsB = new List<OutlineInstance>();

    public void AddOutlineObject(MeshFilter target, Color color, float duration)
    {
        var outlineInstance = new OutlineInstance { meshFilter = target, timeLeft = duration, propertyBlock = new MaterialPropertyBlock() };
        outlineInstance.propertyBlock.SetColor("_Color", color);
        outlineTargets.Add(outlineInstance);

    }
}
