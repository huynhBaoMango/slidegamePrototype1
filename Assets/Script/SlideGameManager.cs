using JetBrains.Annotations;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class SlideGameManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public EnemyManager enemyManager;

    [SerializeField] private Transform unitParent, tileParent;

    public pieceInfo piecePrefab;
    public pieceInfo[,] pieces = new pieceInfo[5, 5];

    [SerializeField] private Image[] heartList;
    public Sprite fullHeartImg, emptyHeartImg;

    public GameObject PlayUI, EndGameUI, SettingUI;
    public TextMeshProUGUI scorePlayUIText, scoreEndGameUIText, currentScoreText;

    public int size;
    public float width;
    public int maxUnitCount;


    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        enemyManager = GetComponent<EnemyManager>();
        if (PlayerPrefs.GetFloat("SlideGameHighScore") != null)
        {
            scorePlayUIText.text ="High score: " + PlayerPrefs.GetFloat("SlideGameHighScore");
        }
        else
        {
            scorePlayUIText.text = 0 + "";
        }

        enemyManager.defaultWaveDelay = PlayerPrefs.GetFloat("dfDelay", 5f);
        enemyManager.defaultUnitTimeToMove = PlayerPrefs.GetFloat("dfSpeed", 10f);
        enemyManager.percentDelay = PlayerPrefs.GetFloat("dfPercentDelay", 0.1f);
        enemyManager.percentTime = PlayerPrefs.GetFloat("dfPercentTime", 0.1f);

    }


    void Init(float gapThickness)
    {
        width = 2 / (float)size;
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                pieceInfo piece = Instantiate(piecePrefab, new Vector3(row, col, 0), Quaternion.identity, tileParent);
                piece.Init(row, col, width, ClickToSwap);
                piece.transform.localScale = ((2 * width) - gapThickness) * Vector3.one;
                pieces[row, col] = piece;


                if (row == size - 1 && col == size - 1)
                {
                    piece.isEmpty = true;
                    piece.GetComponentInChildren<Renderer>().enabled = false;
                    piece.GetComponent<Renderer>().enabled = false;
                }
            }
        }
    }
    [Button]
    IEnumerator CreateUnitPieces()
    {
        foreach (pieceInfo piece in pieces)
        {
            piece.type = pieceInfo.pieceType.normal;
            piece.UpdateType();
        }
        HashSet<Vector2Int> selectedPositions = new HashSet<Vector2Int>();
        int toppartCount = 0;
        int bottompartCount = 0;
        while (toppartCount < maxUnitCount)
        {
            Vector2Int newTopPart = GetRandomPos();
            if (selectedPositions.Add(newTopPart) && !pieces[newTopPart.x, newTopPart.y].isEmpty)
            {
                selectedPositions.Add(newTopPart);
                pieces[newTopPart.x, newTopPart.y].type = pieceInfo.pieceType.top;
                pieces[newTopPart.x, newTopPart.y].UpdateType();
                selectedPositions.Add(new Vector2Int(newTopPart.x + 1, newTopPart.y));
                Debug.Log(newTopPart.ToString() + selectedPositions.Count);
                toppartCount++;
            }
        }
        yield return new WaitForSeconds(0.01f);
        while (bottompartCount < maxUnitCount)
        {
            Vector2Int newTopPart = GetRandomPos();
            if (selectedPositions.Add(newTopPart) && !pieces[newTopPart.x, newTopPart.y].isEmpty)
            {
                selectedPositions.Add(newTopPart);
                pieces[newTopPart.x, newTopPart.y].type = pieceInfo.pieceType.bottom;
                pieces[newTopPart.x, newTopPart.y].UpdateType();
                Debug.Log(newTopPart.ToString() + selectedPositions.Count);
                bottompartCount++;
            }
        }
    }


    void ClickToSwap(int x, int y)
    {
        int dx = GetDx(x, y);
        int dy = GetDy(x, y);
        Swap(x, y, dx, dy);

    }

    void TryToMerge(int x, int y)
    {
        if (pieces[x, y].type == pieceInfo.pieceType.top && x != size - 1)
        {
            if (pieces[x + 1, y].type == pieceInfo.pieceType.bottom)
            {
                playerManager.SpawnUnit(y);
                pieces[x, y].type = pieceInfo.pieceType.normal;
                pieces[x + 1, y].type = pieceInfo.pieceType.normal;
                pieces[x, y].UpdateType();
                pieces[x + 1, y].UpdateType();
                CheckToSpawnNewUnitPieces();
            }
        }
        if (pieces[x, y].type == pieceInfo.pieceType.bottom && x != 0)
        {
            if (pieces[x - 1, y].type == pieceInfo.pieceType.top)
            {
                playerManager.SpawnUnit(y);
                pieces[x, y].type = pieceInfo.pieceType.normal;
                pieces[x - 1, y].type = pieceInfo.pieceType.normal;
                pieces[x, y].UpdateType();
                pieces[x - 1, y].UpdateType();
                CheckToSpawnNewUnitPieces();
            }
        }
    }

    void CheckToSpawnNewUnitPieces()
    {
        bool needToSpawn = true;
        foreach(pieceInfo piece in pieces)
        {
            if (piece.type != pieceInfo.pieceType.normal) needToSpawn = false;
        }
        Debug.Log(needToSpawn);
        if (needToSpawn) StartCoroutine(CreateUnitPieces());
    }

    void Swap(int x, int y, int dx, int dy)
    {
        var from = pieces[x, y];
        var to = pieces[x + dx, y + dy];

        pieces[x, y] = to;
        pieces[x + dx, y + dy] = from;

        from.UpdatePos(x + dx, y + dy, width);
        to.UpdatePos(x, y, width);


        TryToMerge(x+dx, y+dy);
    }
    
    int GetDx(int x, int y)
    {
        if (x < size-1 && pieces[x + 1, y].isEmpty)
        {
            return 1;
        }
        if(x > 0 && pieces[x - 1, y].isEmpty)
        {
           return -1;
        }
        return 0;
    }
    int GetDy(int x, int y)
    {
        if (y < size-1 && pieces[x, y + 1].isEmpty)
        {
            return 1;
        }
        if (y > 0 && pieces[x, y - 1].isEmpty)
        {
            return -1;
        }
        return 0;
    }

    Vector2Int GetRandomPos()
    {
        int x = Random.Range(0, size);
        int y = Random.Range(0, size);
        return new Vector2Int(x, y);
    }

    public void UpdateScore(float addedScore)
    {
        playerManager.score += addedScore;
        currentScoreText.text = playerManager.score+"";
        if(playerManager.score >= 1000 && playerManager.score % 1000 == 0)
        {
            enemyManager.waveDelay -= enemyManager.waveDelay * enemyManager.percentDelay;
            enemyManager.UpdateWaveDelay();
        }
        
        if(playerManager.score >= 1000 && playerManager.score % 5000 == 0)
        {
            enemyManager.unitTimeToMove -= enemyManager.unitTimeToMove * enemyManager.percentTime;
        }

        if(FindObjectOfType<DebugPanel>() != null)
        {
            FindObjectOfType<DebugPanel>().UpdateText();
        }
    }

    public void TakeDamageManager(float damage)
    {
        if(playerManager.heart > 0)
        {
            playerManager.TakeDamage(damage);
            UpdateUIHeart();
        }
        if(playerManager.heart == 0)
        {
            EndGame();
        }

    }

    public void UpdateUIHeart()
    {
        for (int i = 0; i < heartList.Length; i++)
        {
            heartList[i].sprite = (i < playerManager.heart) ? fullHeartImg : emptyHeartImg;
        }
    }


    public void EndGame()
    {
        enemyManager.StopGame();
        EndGameUI.SetActive(true);
        if(PlayerPrefs.GetFloat("highScore") != null)
        {
            if(playerManager.score > PlayerPrefs.GetFloat("SlideGameHighScore"))
            {
                PlayerPrefs.SetFloat("SlideGameHighScore", playerManager.score);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("SlideGameHighScore", playerManager.score);
        }
        scoreEndGameUIText.text = "Your score: \n" + playerManager.score;
    }

    public void OnClickPlay()
    {
        Init(0.05f);
        CreateUnitPieces();
        EndGameUI.SetActive(false);
        PlayUI.SetActive(false);
        StartCoroutine(CreateUnitPieces());
        enemyManager.StartGame();
        playerManager.StartGame();
        UpdateUIHeart();
        UpdateScore(0);
    }

    public void OnClickBack()
    {
        EndGameUI.SetActive(false);
        PlayUI.SetActive(true);
        if (PlayerPrefs.GetFloat("SlideGameHighScore") != null)
        {
            scorePlayUIText.text = PlayerPrefs.GetFloat("SlideGameHighScore") +"";
        }
        else
        {
            scorePlayUIText.text = 0 + "";
        }

        foreach(Transform t in unitParent)
        {
            Destroy(t.gameObject);
        }
        foreach (Transform t in tileParent)
        {
            Destroy(t.gameObject);
        }
        enemyManager.StopGame();
    }

    public void OnClickSetting()
    {
        EndGameUI.SetActive(false);
        PlayUI.SetActive(false);
        SettingUI.SetActive(true);
    }

    public void ChangeSettingGame(float f1, float f2, float f3, float f4)
    {
        enemyManager.defaultWaveDelay = f1;
        enemyManager.defaultUnitTimeToMove = f2;
        playerManager.defaultUnitTimeToMove = f2;
        enemyManager.percentDelay = f3;
        enemyManager.percentTime = f4;
    }

}
