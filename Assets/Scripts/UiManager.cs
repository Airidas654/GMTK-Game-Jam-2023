using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    }

    public void UpdateWaterBar(float percent)
    {
        DOTween.Kill(WaterBar.fillAmount);
        DOTween.To(() => WaterBar.fillAmount, x => WaterBar.fillAmount = x, percent, WaterBarDuration);
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
            BuildingButtons[building].DOKill();
            BuildingButtons[building].DOMoveY(defaultY, BuildingBlockOffsetDuration).SetEase(Ease.InOutCubic);
            selectedBuilding = -1;
            DisableRequiredWaterBar();
        }
        else
        {
            inBuildMode = true;
            BuildingButtons[building].DOKill();
            BuildingButtons[building].DOMoveY(defaultY + BuildingBlockOffsetY, BuildingBlockOffsetDuration).SetEase(Ease.InOutCubic);
            if (selectedBuilding != -1)
            {
                BuildingButtons[selectedBuilding].DOKill();
                BuildingButtons[selectedBuilding].DOMoveY(defaultY, BuildingBlockOffsetDuration).SetEase(Ease.InOutCubic);
            }
            selectedBuilding = building;
            EnableRequiredWaterBar(0.5f);
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
