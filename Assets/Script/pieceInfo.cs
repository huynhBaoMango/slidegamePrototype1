using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pieceInfo : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    public pieceType type;
    public bool isEmpty;
    int x = 0;
    int y = 0;
    
    private Action<int, int> swapFunc = null;

    public void Init(int row, int col, float width, Action<int, int> swapFunc)
    {
        UpdateType();
        UpdatePos(row, col, width);
        this.swapFunc = swapFunc;
        this.isEmpty = false;
    }

    public void UpdateType()
    {
        switch(type) 
        {
            case pieceType.top:
                GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;
            case pieceType.bottom:
                GetComponent<SpriteRenderer>().sprite = sprites[1];
                break;
            case pieceType.normal:
                GetComponent<SpriteRenderer>().sprite = sprites[2];
                break;

        }
    }
    public void UpdatePos(int row, int col, float width)
    {
        x = row;
        y = col;
        StartCoroutine(Move(new Vector3(-1 + (2 * width * col) + width, +1 - (2 * width * row) - width, 0)));
        
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && swapFunc != null)
        {
            swapFunc(x, y);
        }
    }
    IEnumerator Move(Vector3 end)
    {
        float elapseTime = 0;
        float duration = 0.1f;

        Vector3 start = this.gameObject.transform.localPosition;

        while (elapseTime <= duration)
        {
            this.gameObject.transform.localPosition = Vector3.Lerp(start, end, (elapseTime/duration));
            elapseTime += Time.deltaTime;
            yield return null;
        }

        this.gameObject.transform.localPosition = end;
    }

    public enum pieceType
    {
        normal,
        top,
        bottom
    }
}
