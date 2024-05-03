
using System.Collections.Generic;
using UnityEngine;

public class DialogUnit
{
	public string m_name;
	public string m_content;
	public List<DialogOption> m_options = new();
	public Sprite m_SR1;
	public Sprite m_SR2;


}
public class DialogOption
{
	public string m_description;
	public List<DialogEffect> m_effect = new();
	public string m_result;
}

public abstract class DialogEffect
{
	public string m_name;
	public float value;
	public DialogEffect(float value)
	{ this.value = value; }
	public abstract void Trigger(Player player);
}

public class SanityEffect : DialogEffect
{
	public SanityEffect(float value) : base(value)
	{
	}

	public override void Trigger(Player p)
	{
		p.sanity += value;
		Debug.Log("玩家的San值增加了" + value.ToString() + "点！");
	}
}


public class DecisionValueEffect : DialogEffect
{
	public DecisionValueEffect(float value) : base(value)
	{
	}

	public override void Trigger(Player p)
	{
		p.decisionValue += (int)value;
		Debug.Log("玩家的决策点增加了" + value.ToString() + "点！");
	}
}


public class ArmamentEffect : DialogEffect
{
	public ArmamentEffect(float value) : base(value)
	{
	}

	public override void Trigger(Player p)
	{
		p.armament += value;
	}
}

public class FundEffect : DialogEffect
{
	public FundEffect(float value) : base(value)
	{
	}

	public override void Trigger(Player p)
	{
		p.fund += value;
	}
}


public class PopularSupportEffect : DialogEffect
{
	public PopularSupportEffect(float value) : base(value)
	{
	}

	public override void Trigger(Player p)
	{
		p.popularSupport += value;
	}
}

public class TroopIncreaseEffect : DialogEffect
{
	public TroopIncreaseEffect(float value) : base(value)
	{
	}

	public override void Trigger(Player p)
	{
		p.troopIncrease += value;
	}
}
