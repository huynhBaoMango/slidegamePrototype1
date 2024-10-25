using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public TMP_InputField ip1, ip2, ip3, ip4;
    public SlideGameManager gameManager;

    private void Start()
    {

    }

    public void OnEnable()
    {
        ip1.text = gameManager.enemyManager.defaultWaveDelay+"";
        ip2.text = gameManager.enemyManager.defaultUnitTimeToMove+"";
        ip3.text = gameManager.enemyManager.percentDelay + "";
        ip4.text = gameManager.enemyManager.percentTime + "";
    }

    public void OnClickSaveAndBack()
    {
        float f1 = float.Parse(ip1.text);
        float f2 = float.Parse(ip2.text);
        float f3 = float.Parse(ip3.text);
        float f4 = float.Parse(ip4.text);

        PlayerPrefs.SetFloat("dfDelay", f1);
        PlayerPrefs.SetFloat("dfSpeed", f2);
        PlayerPrefs.SetFloat("dfPercentDelay", f3);
        PlayerPrefs.SetFloat("dfPercentTime", f4);

        gameManager.ChangeSettingGame(f1, f2, f3, f4);
        gameObject.SetActive(false);
        gameManager.PlayUI.SetActive(true);
    }
}
