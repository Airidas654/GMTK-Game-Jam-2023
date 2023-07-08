using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShotTrail : MonoBehaviour
{
    [SerializeField] float disapearTime = 1f;
    SpriteRenderer spriteRenderer;
    Color beginColor;
    public void Reset(Vector3 from, Vector3 to)
    {
        from.z = 0;
        to.z = 0;
        float dist = (from - to).magnitude;

        Vector3 dir = (from - to) / dist;

        transform.rotation = Quaternion.LookRotation(dir, new Vector3(0, 0, -1));
        transform.position = (from + to) / 2;
        transform.localScale = new Vector3(transform.localScale.x, dist, 1);

        spriteRenderer.color = beginColor;

        spriteRenderer.DOColor(new Color(beginColor.r, beginColor.g, beginColor.b, 0f), disapearTime).SetEase(Ease.InCirc).OnComplete(() =>
        {
            PoolManager.Instance.shotTrailPool.Release(gameObject);
        });
    }

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        beginColor = spriteRenderer.color;
    }
}
