using Unity.VisualScripting;
using UnityEngine;

public struct DialogUnit
{
    public string m_name;
    public string m_content;
    public DialogOption[] m_options;
}

public struct DialogOption
{
    public string m_discription;
    public DialogEffect m_effect;
}

public interface DialogEffect
{
    public abstract void Trigger(Player player);
}

public class DemoEffect1 : DialogEffect
{
    public void Trigger(Player p)
    {
        Debug.Log("玩家的san值降低了");
        p.sanity--;
    }
}


public class DemoEffect2 : DialogEffect
{
    public void Trigger(Player p)
    {
        Debug.Log("玩家的行动点增加了");
        p.sanity--;
    }
}