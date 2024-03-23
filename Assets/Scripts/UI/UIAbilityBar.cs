using System;
using Defines;
using Keiwando.BigInteger;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class UIAbilityBar : UIBase
{
    [SerializeField] private HoldCheckerButton upgradeBtn;
    
    [SerializeField] private Image costImage;
    
    [SerializeField] private TMP_Text abilityLevelText;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text modifyStatusText;
    
    private AbilityInfo abilityInfo;

    [SerializeField] private RectTransform effectRect;

    public Transform GetButtonRect()
    {
        return upgradeBtn.transform;
    }
    public void ShowUI(AbilityInfo info)
    {
        abilityInfo = info;
        InitializeUI();

        CurrencyManager.instance.onCurrencyChanged += OnCurrencyUpdate;
    }

    public override void CloseUI()
    {
        base.CloseUI();

        CurrencyManager.instance.onCurrencyChanged -= OnCurrencyUpdate;
    }

    private void OnCurrencyUpdate(ECurrencyType type, string amount)
    {
        if (type == abilityInfo.currencyType)
        {
            if (abilityInfo.CheckUpgradeCondition())
            {
                // TODO 글씨 색 회색
                upgradeBtn.interactable = true;
                costText.color = Color.white;
            }
            else
            {
                // TODO 글씨 색 흰색 
                upgradeBtn.interactable = false;
                costText.color = Color.red;
            }
        }
    }

    protected void Awake()
    {
        InitializeBtn();
    }

    private void InitializeBtn()
    {
        upgradeBtn.onClick.AddListener(() => UpgradeBtn(abilityInfo.statusType));
        upgradeBtn.onExit.AddListener(CurrencyManager.instance.SaveCurrencies);
    }

    private void UpgradeBtn(EStatusType type)
    {
        // TODO currency manager를 통해서 돈 빼기!
        if (TryUpgrade(type))
        {
            UpdateUI();
        }
        else
        {
            MessageUIManager.instance.ShowCenterMessage(CustomText.SetColor("골드", Color.yellow) + "가 부족합니다.");
        }
    }

    private bool TryUpgrade(EStatusType type)
    {
        if (CurrencyManager.instance.SubtractCurrency(abilityInfo.currencyType, abilityInfo.cost))
        {
            if (abilityInfo.modifyStatusInt != 0)
                UpgradeManager.instance.ModifyStatus(abilityInfo);
            else
                UpgradeManager.instance.ModifyStatus(abilityInfo);
            
            // Show Effect
            UIEffectManager.instance.ShowUpgradeEffect(titleText.transform);

            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateUI()
    {
        titleText.text = abilityInfo.statusType.ToString();
        
        if (abilityInfo.modifyStatusInt != 0)
            modifyStatusText.text = $"(+{abilityInfo.modifyStatusInt})";
        else
            modifyStatusText.text = $"(+{(abilityInfo.modifyStatusFloat * 100):F2}%)";

        costText.text = abilityInfo.cost.ChangeToShort();
        
        upgradeBtn.interactable = abilityInfo.CheckUpgradeCondition();
    }

    private void InitializeUI()
    {
        costImage.sprite = CurrencyManager.instance.GetIcon(abilityInfo.currencyType);
       
        if (abilityInfo.modifyStatusInt != 0)
            modifyStatusText.text = abilityInfo.modifyStatusInt.ToString();
        else
            modifyStatusText.text = (abilityInfo.modifyStatusFloat * 100).ToString("F2") + "%";

        titleText.text = abilityInfo.statusType.ToString();
        
        UpdateUI();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
