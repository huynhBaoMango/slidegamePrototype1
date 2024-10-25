using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitManager : MonoBehaviour
{
    public UnitType unitType;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<unitManager>() != null)
        {
            if (unitType != collision.gameObject.GetComponent<unitManager>().unitType)
            {
                if(unitType == UnitType.player)
                {
                    FindObjectOfType<SlideGameManager>().UpdateScore(100);
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                }
            }
        }
        if(unitType == UnitType.enemy)
        {
            if (collision.gameObject.GetComponent<unitManager>() == null)
            {
                FindObjectOfType<SlideGameManager>().TakeDamageManager(1);
                Destroy(gameObject);
            }
        }

        
    }

    public void MoveTo(float posToMove, float timeToMove)
    {
        LeanTween.moveY(gameObject, posToMove, timeToMove).setOnComplete(() => { Destroy(gameObject); });
    }

    public enum UnitType
    {
        player,
        enemy
    }
}
