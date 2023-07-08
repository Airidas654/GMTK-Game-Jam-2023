using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] RectTransform anyKeyText;
    [SerializeField] float duration;
    [SerializeField] float maxSizeOffset;

    [SerializeField] Image black;
    [SerializeField] TextMeshProUGUI survived;

    bool once;

    void Start()
    {
        anyKeyText.DOScale(new Vector2(anyKeyText.localScale.x * maxSizeOffset, anyKeyText.localScale.x * maxSizeOffset), duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        DOTween.Kill(black.color);
        DOTween.To(() => black.color, x => black.color = x, new Color(0, 0, 0, 0), 1).OnComplete(() => black.gameObject.SetActive(false)).SetEase(Ease.InOutCubic);
        once = false;
        if (UiManager.survived != default)
        {
            if(UiManager.survived == -1)
            {
                survived.text = "Congratulations, you won the game!";
            }
            else
            {
                survived.text = string.Format("You survived for {0} minutes ant {1} seconds", UiManager.survived/60, UiManager.survived%60);
            }
            survived.gameObject.SetActive(true);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !once)
        {
            black.gameObject.SetActive(true);
            DOTween.Kill(black.color);
            once = true;
            DOTween.To(() => black.color, x => black.color = x, new Color(0, 0, 0, 1), 1).SetEase(Ease.InOutCubic).OnComplete(() => SceneManager.LoadScene(1));
        }
    }
}
