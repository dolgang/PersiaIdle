using Defines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "SO/AbilityRerollInfo", fileName = "AbilityProbability")]
public class AbilityFixedInfo : ScriptableObject
{
    public int abilityLevel;

    // 업글 관련
    public List<AbilityStatusGrade> abilityStatusGrade;

    // 비용 관련
    public ECurrencyType currencyType;
    public int baseCost;
}

[Serializable]
public class AbilityStatusGrade
{
    public EAbilityRarity abilityRarity;
    public List<AbilityStatusValue> abilityStatusValues;
}

[Serializable]
public class AbilityStatusValue
{
    public EStatusType statusType;
    [Header("ATK, HP, MP, MP_RECO, CRIT_DMG")]
    public int minModifyStatusInt;
    public int maxModifyStatusInt;

    [Header("DMG_REDU, CRIT_CH, ATK_SPD, ATK_RAN, MOV_SPD")]
    public float minModifyStatusFloat;
    public float manModifyStatusFloat;
}
