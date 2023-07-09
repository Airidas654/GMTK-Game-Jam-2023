using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static bool alreadySeenTutorial;

    [SerializeField] RectTransform anyKeyText;
    [SerializeField] float duration;
    [SerializeField] float maxSizeOffset;

    [SerializeField] Image black;
    [SerializeField] TextMeshProUGUI survived;

    bool once;
    bool InTutorial;
    bool starting;

    private void Awake()
    {
        starting = true;
    }

    void Start()
    {
        anyKeyText.DOScale(new Vector2(anyKeyText.localScale.x * maxSizeOffset, anyKeyText.localScale.x * maxSizeOffset), duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        DOTween.Kill(black.color);
        DOTween.To(() => black.color, x => black.color = x, new Color(0, 0, 0, 0), 1).OnComplete(() => { black.gameObject.SetActive(false); starting = false; }).SetEase(Ease.InOutCubic);
        once = false;
        InTutorial = false;
        if (UiManager.survived != null)
        {
            if (UiManager.survived == -1)
            {
                survived.text = "Congratulations, you won the game!";
            }
            else
            {
                survived.text = string.Format("You survived for {0} minutes ant {1} seconds", UiManager.survived / 60, UiManager.survived % 60);
            }
            survived.gameObject.SetActive(true);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!starting)
        {
            if (Input.anyKeyDown && !once && !InTutorial)
            {
                SoundManager.Instance.Play(5);
                black.gameObject.SetActive(true);
                DOTween.Kill(black.color);
                once = true;
                InTutorial = true;
                canPressNextTutorial = false;
                DOTween.To(() => black.color, x => black.color = x, new Color(0, 0, 0, 1), 1).SetEase(Ease.InOutCubic).OnComplete(() =>
                {
                    if (alreadySeenTutorial)
                    {
                        DOTween.KillAll();
                        SceneManager.LoadScene(1);
                    }
                    else
                    {
                        EnableTutorial();
                    }
                });
            }
            else if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape) && InTutorial && canPressNextTutorial)
            {
                NextTutorial();
                SoundManager.Instance.Play(5);
                canPressNextTutorial = false;
            }
            if (Input.GetKeyDown(KeyCode.Escape) && InTutorial && canPressNextTutorial)
            {
                SoundManager.Instance.Play(5);
                black.gameObject.SetActive(true);
                canPressNextTutorial = false;
                DOTween.Kill(black.color);
                DOTween.To(() => black.color, x => black.color = x, new Color(0, 0, 0, 1), 1).SetEase(Ease.InOutCubic).OnComplete(() =>
                {
                    DOTween.KillAll();
                    alreadySeenTutorial = true;
                    SceneManager.LoadScene(1);
                });
            }
        }
    }

    [SerializeField] List<GameObject> TutorialPanels;
    [SerializeField] float PanelSlideTime;
    bool canPressNextTutorial;
    int currentTutorialPanel;
    public void EnableTutorial()
    {
        if (TutorialPanels.Count != 0)
        {
            TutorialPanels[0].GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            TutorialPanels[0].SetActive(true);
            currentTutorialPanel = 0;
            DOTween.Kill(black.color);
            DOTween.To(() => black.color, x => black.color = x, new Color(0, 0, 0, 0), 1).SetEase(Ease.InOutCubic).OnComplete(() => { canPressNextTutorial = true; black.gameObject.SetActive(false); });
        }
        else
        {

            alreadySeenTutorial = true;

        }

    }

    public void NextTutorial()
    {
        if (currentTutorialPanel + 1 >= TutorialPanels.Count)
        {
            black.gameObject.SetActive(true);
            DOTween.Kill(black.color);
            DOTween.To(() => black.color, x => black.color = x, new Color(0, 0, 0, 1), 1).SetEase(Ease.InOutCubic).OnComplete(() =>
            {
                DOTween.KillAll();
                alreadySeenTutorial = true;
                SceneManager.LoadScene(1);
            });
        }
        else
        {
            TutorialPanels[currentTutorialPanel + 1].SetActive(true);
            TutorialPanels[currentTutorialPanel + 1].GetComponent<RectTransform>().DOLocalMoveX(0, PanelSlideTime).SetEase(Ease.InOutQuad).OnComplete(() => canPressNextTutorial = true);
            currentTutorialPanel++;
        }
    }

}
