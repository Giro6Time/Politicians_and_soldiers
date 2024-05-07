
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Xml.Schema;

[Serializable]
[CreateAssetMenu(fileName = "DialogUnit", menuName = "DialogData/Unit")]
public class DialogUnitSO : ScriptableObject
{
	public string m_name;
	public string m_content;
	[SerializeField]
	public Option[] m_options;
	public Sprite m_SR1;
	public Vector3 m_SR1_Offset;
	public Sprite m_SR2;
    public Vector3 m_SR2_Offset;


    public DialogUnit GenerateDialogUnit()
	{
		DialogUnit dialogUnit = new DialogUnit();
		dialogUnit.m_name = m_name;
		dialogUnit.m_content = m_content;
		dialogUnit.m_SR1 = m_SR1;
		dialogUnit.m_SR1_Offset = m_SR1_Offset;
		dialogUnit.m_SR2 = m_SR2;
		dialogUnit.m_SR2_Offset = m_SR2_Offset;
		foreach (var option in m_options)
        {
			//创建DialogOption对象，并添加到DialogOptionList中
			DialogOption dialogOption = new DialogOption();
			dialogOption.m_description = option.m_description;
			dialogOption.m_result = option.m_result;
			foreach (var effect in option.m_effects)
			{
				//根据配置生成DialogEffect对象， 并添加到DialogOption中
				switch (effect.m_type)
				{
					default:
						continue;
					case EffectType.DecitionValue:
						dialogOption.m_effect.Add(new DecisionValueEffect(effect.m_value));
						break;
					case EffectType.Sanity:
						dialogOption.m_effect.Add(new SanityEffect(effect.m_value));
						break;
					case EffectType.Armament:
						dialogOption.m_effect.Add(new ArmamentEffect(effect.m_value));
						break;
					case EffectType.Fund:
						dialogOption.m_effect.Add(new FundEffect(effect.m_value));
						break;
					case EffectType.PopularSupport:
						dialogOption.m_effect.Add(new PopularSupportEffect(effect.m_value));
						break;
					case EffectType.TroopIncrease:
						dialogOption.m_effect.Add(new TroopIncreaseEffect(effect.m_value));
						break;
				}
			}
			dialogUnit.m_options.Add(dialogOption);
		}
		return dialogUnit;
	}

	[Serializable]
	public class Option
	{
		public string m_description;
		public string m_result;
		[SerializeField]
		public Effect[] m_effects;
	}

	[Serializable]
	public class Effect
	{
		public EffectType m_type;
		public float m_value;
	}

	public enum EffectType
	{
		None,
		DecitionValue,
		Sanity,
		Armament,
		Fund,
		PopularSupport,
		TroopIncrease,
	}
}
