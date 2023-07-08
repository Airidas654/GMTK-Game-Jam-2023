using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickableDrop : MonoBehaviour
{
    [SerializeField] float randDist = 0.5f;
    [SerializeField] float seekDist = 1f;
    [SerializeField] int waterAmount = 5;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seekDist);
    }

    bool seek;
    bool canSeek;

    public void Reset()
    {
        tweenVal = 0;
        seek = false;
        canSeek = false;
       

        Vector2 randPos = new Vector2(gameObject.transform.position.x,gameObject.transform.position.y) + new Vector2(Random.Range(-randDist,randDist),Random.Range(-randDist,randDist));

        transform.localScale = new Vector3(0,0,0);
        transform.DOScale(1, 0.5f).SetEase(Ease.InOutCubic).Play();
        transform.DOMove(randPos, 1).SetEase(Ease.OutCubic).Play().OnComplete(()=>canSeek = true);
    }

    Tween seekTween;
    float tweenVal;
    void Update()
    {
        if (canSeek && !GameManager.Instance.MaxWaterReached())
        {
            Vector3 playerPos = GameManager.Instance.Player.transform.position;
            if ((playerPos-transform.position).sqrMagnitude <= seekDist * seekDist)
            {
                canSeek = false;
                seek = true;
                seekTween = DOTween.To(()=>0f,x=>tweenVal = x,1f,0.5f).SetEase(Ease.Linear).OnComplete(()=>
                {
                    Pump.Instance.drops.Release(gameObject);
                    GameManager.Instance.AddWater(waterAmount);
                });
                
            }
        }

        if (seek)
        {
            Vector3 playerPos = GameManager.Instance.Player.transform.position;
            transform.position = Vector3.Lerp(transform.position, playerPos, tweenVal);
        }
    }
}
