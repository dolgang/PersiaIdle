using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 어빌리티 확률 보유용 클래스
[CreateAssetMenu(fileName = "AbilityProbability", menuName = "ScriptableObject/AbilityProbability", order = 0)]
[Serializable]
public class AbilityRerollProbability : ScriptableObject
{
    public AbilityPer[] eachWeight;
    private int totalWeight;

    public AbilityRerollProbability()
    {
        eachWeight = new AbilityPer[Enum.GetNames(typeof(EAbilityRarity)).Length-1];
        totalWeight = 0;
    }

    public virtual int Reroll()
    {
        InitWeight();

        var ran = UnityEngine.Random.Range(1, totalWeight + 1);
        int grade = GetRarity(ran);

        return grade;
    }

    protected virtual int GetRarity(int ran)
    {
        int current = 0;
        int rarity = 0;
        foreach (var perRarityLevel in eachWeight)
        {
            current += perRarityLevel.rarityWeight;
            if (ran <= current)
            {
                return rarity;
            }
            rarity++;
        }

        // 그럴 일은 없겠지만 끝까지 간 경우에 대한 예외처리
        Debug.Assert(false, "확률이 이상합니다.");
        return eachWeight.Length - 1;
    }

    public virtual void InitWeight()
    {
        int ret = 0;
        foreach (var abilityPer in eachWeight)
        {
            ret += abilityPer.rarityWeight;
        }

        totalWeight = ret;
    }

    public virtual float GetPercentage(EAbilityRarity rarity)
    {
        return eachWeight[(int)rarity].GetPercentage(totalWeight);
    }
}

// 각 Rarity당 가챠 확률을 구성하는 데이터입니다.
[Serializable]
public class AbilityPer
{
    [SerializeField] public int rarityWeight;

    public int GetWeight()
    {
        return rarityWeight;
    }

    public float GetPercentage(float totalWeight)
    {
        float current = GetWeight();
        return current / totalWeight;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AbilityRerollProbability))]
public class AbilityProbabilityEditor : Editor
{
    private AbilityRerollProbability abilityProbability;

    private void OnEnable()
    {
        abilityProbability = target as AbilityRerollProbability;
        int lengthCondition = Enum.GetNames(typeof(EAbilityRarity)).Length - 1;

        if (abilityProbability.eachWeight.Length != lengthCondition)
        {
            List<AbilityPer> temp = new List<AbilityPer>();
            temp.AddRange(abilityProbability.eachWeight);

            if (temp.Count < lengthCondition)
            {
                while (temp.Count < lengthCondition)
                    temp.Add(new AbilityPer());
            }
            else if (temp.Count > lengthCondition)
            {
                while (temp.Count > lengthCondition)
                    temp.RemoveAt(temp.Count - 1);
            }

            abilityProbability.eachWeight = temp.ToArray();
            EditorUtility.SetDirty(target);
        }
    }

    public override void OnInspectorGUI()
    {
        abilityProbability.InitWeight();
        for (int i = 0; i < Enum.GetNames(typeof(EAbilityRarity)).Length-1; ++i)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"{(EAbilityRarity)i} 확률");
            GUILayout.Label((100 * abilityProbability.GetPercentage((EAbilityRarity)i)).ToString("F5") + "%");
            int total = EditorGUILayout.IntField("Total Weight", abilityProbability.eachWeight[i].rarityWeight);
            if (total != abilityProbability.eachWeight[i].rarityWeight)
            {
                abilityProbability.eachWeight[i].rarityWeight = total;
                EditorUtility.SetDirty(target);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}
#endif