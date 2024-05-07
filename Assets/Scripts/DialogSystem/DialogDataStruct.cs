
using System.Collections.Generic;
using UnityEngine;

public class DialogUnit
{
	public string m_name;
	public string m_content;
	public List<DialogOption> m_options = new();
	public Sprite m_SR1;
	public Vector3 m_SR1_Offset;
	public Sprite m_SR2;
    public Vector3 m_SR2_Offset;


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
		MessageView._Instance.ShowMessage("San值" + value.ToString() + "点");
		Debug.Log("San值" + value.ToString() + "点");
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
		MessageView._Instance.ShowMessage("决策点" + value.ToString() + "点");
        Debug.Log("决策点" + value.ToString() + "点");
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
