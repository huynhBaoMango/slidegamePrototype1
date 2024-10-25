using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    public SlideGameManager gameManager;
    public EnemyManager enemyManager;
    public TextMeshProUGUI text1, text2;
    void OnEnable()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        text1.text = "Delay time to wave: \n" + enemyManager.waveDelay;
        text2.text = "Time unit need to move: \n" + enemyManager.unitTimeToMove;
    }

    public void AddScore()
    {
        gameManager.UpdateScore(1000);
    }

}
