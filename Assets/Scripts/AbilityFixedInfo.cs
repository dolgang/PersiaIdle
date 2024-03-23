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
    public EStatusType statusType;

    [Header("ATK, HP, MP, MP_RECO, CRIT_DMG")]
    public int modifyStatusInt;

    [Header("DMG_REDU, CRIT_CH, ATK_SPD, ATK_RAN, MOV_SPD")]
    public float modifyStatusFloat;

    // 비용 관련
    public ECurrencyType currencyType;
    public int baseCost;
}
