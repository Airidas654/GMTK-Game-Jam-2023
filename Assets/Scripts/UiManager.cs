using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    
    public void SelectBuilding(int building)
    {
        if(building == selectedBuilding)
        {

        }
    }

}
