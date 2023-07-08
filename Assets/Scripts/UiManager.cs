using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] Image WaterBar;
    [SerializeField] float WaterBarDuration;
    [SerializeField] Image WaterBarRequired;



    public static UiManager Instance;

    private float defaultY;
    [SerializeField] float BuildingBlockOffsetY;
    [SerializeField] float BuildingBlockOffsetDuration;
    [SerializeField] List<RectTransform> BuildingButtons;

    [SerializeField] TextMeshProUGUI timer;

    public int selectedBuilding { get; private set; }
    public bool inBuildMode { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        selectedBuilding = -1;
    }

    private void Start()
    {
        defaultY = BuildingButtons[0].position.y;
        DOTween.Init();
        canShakeAgain = true;
    }

    public void UpdateTimer(int time)
    {
        timer.text = string.Format("{0:D2}:{1:D2}", time / 60, time % 60);
    }

    public void UpdateWaterBar(float percent)
    {
        DOTween.Kill(WaterBar.fillAmount);
        DOTween.To(() => WaterBar.fillAmount, x => WaterBar.fillAmount = x, percent, WaterBarDuration);
    }

    bool canShakeAgain;
    public void DoWaterShake()
    {
        if (canShakeAgain)
        {
            Transform obj = WaterBar.transform.parent;
            canShakeAgain = false;
            obj.DOShakeRotation(1, 35, 7, 45, true).OnComplete(()=>canShakeAgain=true);
        }
    }

    public void EnableRequiredWaterBar(float percent)
    {
        DOTween.Kill(WaterBarRequired.fillAmount);
        DOTween.To(() => WaterBarRequired.fillAmount, x => WaterBarRequired.fillAmount = x, percent, WaterBarDuration);
    }

    public void DisableRequiredWaterBar()
    {
        DOTween.Kill(WaterBarRequired.fillAmount);
        DOTween.To(() => WaterBarRequired.fillAmount, x => WaterBarRequired.fillAmount = x, 0, WaterBarDuration);
    }

    public void SelectBuilding(int building)
    {
        if (building == selectedBuilding)
        {
            inBuildMode = false;

            BuildingManager.Instance.BuildMode = false;

            BuildingButtons[building].DOKill();
            BuildingButtons[building].DOMoveY(defaultY, BuildingBlockOffsetDuration).SetEase(Ease.InOutCubic);
            selectedBuilding = -1;
            DisableRequiredWaterBar();
        }
        else
        {
            BuildingManager.Instance.SelectedBuilding = building;
            BuildingManager.Instance.BuildMode = true;

            inBuildMode = true;
            BuildingButtons[building].DOKill();
            BuildingButtons[building].DOMoveY(defaultY + BuildingBlockOffsetY, BuildingBlockOffsetDuration).SetEase(Ease.InOutCubic);
            if (selectedBuilding != -1)
            {
                BuildingButtons[selectedBuilding].DOKill();
                BuildingButtons[selectedBuilding].DOMoveY(defaultY, BuildingBlockOffsetDuration).SetEase(Ease.InOutCubic);
            }
            selectedBuilding = building;
            EnableRequiredWaterBar((float)GameManager.Instance.BuildingCosts[selectedBuilding] / GameManager.Instance.MaxWaterInBar);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectBuilding(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectBuilding(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectBuilding(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectBuilding(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectBuilding(4);
        }
    }

}
