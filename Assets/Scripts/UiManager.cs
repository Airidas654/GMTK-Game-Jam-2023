using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiManager : MonoBehaviour
{
    [SerializeField] Image WaterBar;


    public static UiManager Instance;

    public int selectedBuilding { get; private set; }
    public bool inBuildMode { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        selectedBuilding = -1;
    }
    
    public void UpdateWaterBar(float percent)
    {
        WaterBar.fillAmount = percent;
    }

    private float defaultY;
    [SerializeField] float BuildingBlockOffsetY;
    [SerializeField] float BuildingBlockOffsetDuration;
    List<RectTransform> BuildingButtons;
    public void SelectBuilding(int building)
    {
        if(building == selectedBuilding)
        {
            inBuildMode = false;
            BuildingButtons[building].DOKill();
            BuildingButtons[building].DOMoveY(defaultY+BuildingBlockOffsetY,BuildingBlockOffsetDuration).SetEase(Ease.InOutCubic);
        }
        else
        {
            inBuildMode = true;
        }
    }

}
