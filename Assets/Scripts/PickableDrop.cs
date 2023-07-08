using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickableDrop : MonoBehaviour
{
    [SerializeField] float randDist = 0.5f;
    [SerializeField] float seekDist = 1f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seekDist);
    }

    bool seek;
    bool canSeek;

    public void Reset()
    {
        seek = false;
        canSeek = false;
       

        Vector2 randPos = new Vector2(gameObject.transform.position.x,gameObject.transform.position.y) + new Vector2(Random.Range(-randDist,randDist),Random.Range(-randDist,randDist));

        transform.localScale = new Vector3(0,0,0);
        transform.DOScale(1, 1).SetEase(Ease.InOutCubic).Play();
        transform.DOMove(randPos, 1).SetEase(Ease.InCubic).Play().OnComplete(()=>canSeek = true);
    }

    Tween seekTween;
    void Update()
    {
        if (canSeek)
        {
            Vector3 playerPos = GameManager.Player.transform.position;
            if ((playerPos-transform.position).sqrMagnitude <= seekDist * seekDist)
            {
                canSeek = false;
                seek = true;
                seekTween = DOTween.To(()=>transform.position,x=>transform.position = x,playerPos,1).SetEase(Ease.Linear);
                
            }
        }
    }
}
