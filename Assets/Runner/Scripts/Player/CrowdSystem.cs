using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSystem : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private Transform runnersParent;
    [SerializeField] private GameObject runnerPrefabs;

    [Header("Settings")]
    [SerializeField] private float angle;
    [SerializeField] private float radius;
   
    void Update()
    {
        if (!GameManager.instance.IsGameState())
            return;

            PlaceRunners();

            if (runnersParent.childCount <= 0)
            GameManager.instance.SetGameState(GameManager.GameState.Gameover);
        
        
    }
    private void PlaceRunners()
    {
        for(int i = 0; i < runnersParent.childCount; i++)
        {
            Vector3 childLocalPosition = GetRunnerLocalPosition(i);
            runnersParent.GetChild(i).localPosition = childLocalPosition;
        }
    }
    private Vector3 GetRunnerLocalPosition(int index)
    {
        float x = radius * Mathf.Sqrt(index) * Mathf.Cos(Mathf.Deg2Rad * index * angle);
        float z = radius * Mathf.Sqrt(index) * Mathf.Sin(Mathf.Deg2Rad * index * angle);

        return new Vector3(x, 0, z);
    }

    public float GetCrowdRadius()
    {
        return radius * Mathf.Sqrt(runnersParent.childCount);
    }

    public void ApplyBonus(BonusType bonusType, int bonusAmount)
    {
        switch (bonusType)
        {
            case BonusType.Addition:
                AddRunners(bonusAmount);
                break;
            case BonusType.Product:
                int runnersToAdd =(runnersParent.childCount * bonusAmount)-runnersParent.childCount;
                AddRunners(runnersToAdd);
                break;
            case BonusType.Difference:
                RemoveRunners(bonusAmount);
                break;
            case BonusType.Division:
                int runnersToRemove = runnersParent.childCount - (runnersParent.childCount / bonusAmount);
                RemoveRunners(runnersToRemove);
                break;
        }
    }
    private void AddRunners(int amount)
    {
        for(int i=0; i < amount; i++)
        {
            Instantiate(runnerPrefabs, runnersParent);
        }
        playerAnimator.Run();
    }
    private void RemoveRunners(int amount)
    {
        if (amount > runnersParent.childCount)
            amount = runnersParent.childCount;

        int runnersAmount = runnersParent.childCount;

        for(int i = runnersAmount- 1; i>= runnersAmount - amount; i--)
        {
            Transform runnerToDestory = runnersParent.GetChild(i);
            runnerToDestory.SetParent(null);
            Destroy(runnerToDestory.gameObject);
        }
    }
}
