using System.Collections;
using UnityEngine;

public class ScoreMultiplier : MonoBehaviour
{
    private GameManager gameManager;
    public float activeMultiplier;
    public float degradationDelay;
    public float degradationRate;
    public float multiplierReduction;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    public void StartMultiplier(int value, float addedMultiplier)
    {
        StartCoroutine(MultiplierDegradation(value, addedMultiplier));
    }

    IEnumerator MultiplierDegradation(int value, float addedMultiplier)
    {
        float remainingReduction = addedMultiplier;

        gameManager.currentEnemies--;
        gameManager.points += (int)(value*activeMultiplier);
        activeMultiplier += addedMultiplier;

        yield return new WaitForSeconds(degradationDelay);

        while (remainingReduction > multiplierReduction)
        {
            activeMultiplier -= multiplierReduction;
            remainingReduction -= multiplierReduction;

            yield return new WaitForSeconds(degradationRate);
        }

        activeMultiplier -= remainingReduction;
        activeMultiplier = Mathf.Round(activeMultiplier * 100) /100;
    }
}
