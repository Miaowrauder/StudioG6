using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScoreButtons : MonoBehaviour
{
    public GameObject UI;
    private ComboUI ComboUI;

    private void Start()
    {
        ComboUI = UI.GetComponent<ComboUI>();
    }

   public void IncreaseCombo()
    {
       ComboUI.Score += 1000 * (1 + (ComboUI.ScoreMult * 2));
    }

    public void DecreaseCombo()
    {
       ComboUI.currentTimer = 0;
    }
}
