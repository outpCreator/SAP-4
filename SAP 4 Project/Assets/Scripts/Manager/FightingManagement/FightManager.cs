using UnityEngine;
using System.Collections.Generic;

public class FightManager : MonoBehaviour
{
    public static FightManager Instance;

    public Material outlineMaterial;

    // Player Components
    PlayerMovement playerMovement;
    CameraMovement cameraMovement;

    [Header("Enemies")]
    EnemyCombat[] allEnemies;
    LevelRoom[] rooms;
    public List<EnemyCombat> activeEnemies = new List<EnemyCombat>();

    public enum FightStates
    {
        NoActiveFight,
        InAFight,
    }

    public FightStates state = FightStates.NoActiveFight;

    private void Awake()
    {
        Instance = this;
    }

    public void InitFightManager()
    {
        playerMovement = PlayerManager.Instance.playerMovementScript;
        cameraMovement = PlayerManager.Instance.cameraMovement;
    }

    public void GetEnemies()
    {
        allEnemies = null;
        allEnemies = FindObjectsByType<EnemyCombat>(FindObjectsSortMode.None);

        foreach (EnemyCombat enemy in allEnemies)
        {
            if (enemy != null)
            {
                enemy.SetUpEnemy();
            }
        }

        rooms = FindObjectsByType<LevelRoom>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        if (playerMovement == null) return;

        UpdateFightState();

        switch (state)
        {
            case FightStates.NoActiveFight:

                cameraMovement.SwitchCameraMode(false);

                break;
            case FightStates.InAFight:

                cameraMovement.SwitchCameraMode(true);

                break;
        }

        outlineTargetsB.Clear();
        foreach (OutlineInstance outline in outlineTargets)
        {
            //if (outline != null)
            //{
            //    Graphics.DrawMesh(outline.meshFilter.sharedMesh, outline.meshFilter.transform.localToWorldMatrix, outlineMaterial, 0, null, 0, outline.propertyBlock);

            //    outline.timeLeft -= Time.deltaTime;
            //    if (outline.timeLeft >= 0.0)
            //    {
            //        outlineTargetsB.Add(outline);
            //    }
            //}
        }

        outlineTargets.Clear();
        outlineTargets.AddRange(outlineTargetsB);

            print(allEnemies.Length);
        if (allEnemies.Length <= 0)
        {

            foreach (LevelRoom room in rooms)
            {
                room.enemiesCleared = true;
            }
        }
    }

    void UpdateFightState()
    {

        List<EnemyCombat> cleanedEnemies = new List<EnemyCombat>();

        foreach (var enemy in allEnemies)
        {
            if (enemy == null || enemy.State == EnemyCombat.EnemyState.Died)
                continue;

            if (enemy.State == EnemyCombat.EnemyState.InRange)
            {
                enemy.outOfRangeTimer = 0;

                if (!activeEnemies.Contains(enemy))
                {
                    activeEnemies.Add(enemy);
                }
                cleanedEnemies.Add(enemy);
            }
            else if (activeEnemies.Contains(enemy))
            {
                enemy.outOfRangeTimer += Time.deltaTime;

                if (enemy.outOfRangeTimer < EnemyCombat.maxOutOfRangeTime)
                {
                    cleanedEnemies.Add(enemy);
                }
            }
        }

        activeEnemies = cleanedEnemies;

        if (activeEnemies.Count > 0)
        {
            state = FightStates.InAFight;
        }
        else
        {
            state = FightStates.NoActiveFight;
        }
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
