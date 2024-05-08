using Codice.Client.BaseCommands.BranchExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditorInternal;
using UnityEngine;
using static DialogUnitSO;
using static UnityEngine.GraphicsBuffer;

// �Զ���༭�����ڵ���
[CanEditMultipleObjects]
[CustomEditor(typeof(CardBaseSO), true)]
public class CardBaseSOEditor : Editor
{



    #region ����Effect�б�
    private ReorderableList drawEffectList;
    private ReorderableList invokeEffectList;
    private ReorderableList battleStartEffectList;
    private ReorderableList liveEffectList;
    private ReorderableList deathEffectList;
    private ReorderableList beforeAttackEffectList;
    private ReorderableList afterAttackEffectList;

    private SerializedProperty drawEffectListProperty;
    private SerializedProperty invokeEffectListProperty;
    private SerializedProperty battleStartEffectListProperty;
    private SerializedProperty liveEffectListProperty;
    private SerializedProperty deathEffectListProperty;
    private SerializedProperty beforeAttackEffectListProperty;
    private SerializedProperty afterAttackEffectListProperty;
    void DrawDrawEffectListHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "trigger on draw");
    }
    void DrawInvokeEffectListHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "trigger on invoke");
    }
    void DrawBattleStartEffectListHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "trigger on battle start");
    }
    void DrawLiveEffectListHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "trigger on live");
    }
    void DrawDeathEffectListHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "trigger on death");
    }
    void DrawBeforeAttaceEffectHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "trigger before attack");
    }
    void DrawAfterAttaceEffectHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "trigger after attack");
    }
    void DrawHeight()
    {

    }
    void DrawListElement(Rect rect, int index, bool isActive, bool isFocused, ref ReorderableList list)
    {
        try
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            // ��ȡelement��Ӧ�Ķ���ʵ��
            object effectInstance = element.managedReferenceValue;

            // ��ȡeffectInstance��Type
            Type instanceType = effectInstance.GetType();

            // ����Type�Ƿ�ʵ����IResultReflectEffect�ӿ�
            if (instanceType.IsAssignableFrom(typeof(IResultReflectEffect)))
            {
                list.elementHeight = EditorGUIUtility.singleLineHeight * 7;
                // ������������
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "IResultReflectEffect");

                // ����pos���Ա���
                EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight), "Position:");

                // ����pos����
                (effectInstance as IResultReflectEffect).pos = (CardPos)EditorGUI.EnumPopup(
                    new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, 100, EditorGUIUtility.singleLineHeight),
                    (effectInstance as IResultReflectEffect).pos
                );

                // ����value���Ա���
                EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 3, rect.width, EditorGUIUtility.singleLineHeight), "Value:");

                // ����value����
                (effectInstance as IResultReflectEffect).value = EditorGUI.FloatField(
                    new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4, 100, EditorGUIUtility.singleLineHeight),
                    (effectInstance as IResultReflectEffect).value
                );
                // ����value���Ա���
                EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 5, rect.width, EditorGUIUtility.singleLineHeight), "Value:");

                // ����value����
                (effectInstance as IResultReflectEffect).rate = EditorGUI.FloatField(
                    new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 6, 100, EditorGUIUtility.singleLineHeight),
                    (effectInstance as IResultReflectEffect).rate
                );
            }

            //TODO:
            else if (instanceType.IsAssignableFrom(typeof(IAddCardEffect)))
            {
                list.elementHeight = EditorGUIUtility.singleLineHeight * 5;
                //
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "IAddCardEffect");
                //
                EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight), "Num:");
                //
                (effectInstance as IAddCardEffect).num = EditorGUI.IntField(

                    new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, 100, EditorGUIUtility.singleLineHeight),
                    (effectInstance as IAddCardEffect).num
                );
                EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 3, rect.width, EditorGUIUtility.singleLineHeight), "Type:");
                (effectInstance as IAddCardEffect).cardBaseType = (CardBaseType)EditorGUI.EnumPopup(

                    new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4, 100, EditorGUIUtility.singleLineHeight),
                    (effectInstance as IAddCardEffect).cardBaseType
                );
            }
            else if (instanceType.IsAssignableFrom(typeof(IAddDecision)))
            {
                list.elementHeight = EditorGUIUtility.singleLineHeight * 5;
                //
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "IAddDecision");
                //
                EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight), "Num:");
                //
                (effectInstance as IAddDecision).num = EditorGUI.IntField(

                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, 100, EditorGUIUtility.singleLineHeight),
                (effectInstance as IAddDecision).num
            );
        }

        else if (instanceType.IsAssignableFrom(typeof(IChangePossibility)))
        {
            list.elementHeight = EditorGUIUtility.singleLineHeight * 5;
            //
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "IChangePossibility");
            //
            EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight), "Possibility: 0-10�ɵ͵���");
            //
            (effectInstance as IChangePossibility).possibility = EditorGUI.IntField(

                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, 100, EditorGUIUtility.singleLineHeight),
                (effectInstance as IChangePossibility).possibility
            );
            EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 3, rect.width, EditorGUIUtility.singleLineHeight), "Type(0-4: ½���վ���)");
            (effectInstance as IChangePossibility).possibility = EditorGUI.IntField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4, 100, EditorGUIUtility.singleLineHeight),
                (effectInstance as IChangePossibility).type
            );
        }

        else if (instanceType.IsAssignableFrom(typeof(IAttackInstantly)))
        {
            list.elementHeight = EditorGUIUtility.singleLineHeight * 5;
            //
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "IAttackInstantly");
            //
            EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight), "damage:");
            //
            (effectInstance as IAttackInstantly).damage = EditorGUI.IntField(

                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, 100, EditorGUIUtility.singleLineHeight),
                (effectInstance as IAttackInstantly).damage
            );
            EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 3, rect.width, EditorGUIUtility.singleLineHeight), "target(0Ϊ�Ѿ���1Ϊ����)");
            (effectInstance as IAttackInstantly).target = EditorGUI.IntField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4, 100, EditorGUIUtility.singleLineHeight),
                (effectInstance as IAttackInstantly).target
            );
        }

            else if (instanceType.IsAssignableFrom(typeof(DelayDesisionValueEffect)))
            {
                list.elementHeight = EditorGUIUtility.singleLineHeight * 5;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "DelayDesisionvalueEffect");

                //delayTurn
                EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight), "DelayTurn:");
                (effectInstance as DelayDesisionValueEffect).delayTurn = EditorGUI.IntField(

                    new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, 100, EditorGUIUtility.singleLineHeight),
                    (effectInstance as DelayDesisionValueEffect).delayTurn
                );
                //Value
                EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 3, rect.width, EditorGUIUtility.singleLineHeight), "Value:");
                (effectInstance as DelayDesisionValueEffect).value = EditorGUI.IntField(

                    new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4, 100, EditorGUIUtility.singleLineHeight),
                    (effectInstance as DelayDesisionValueEffect).value
                );
            }
        }
        catch (Exception e)
        {
            list.ClearSelection();
        }

    }

    void AddEffect(ReorderableList reorderableList, String effectType)
    {
        EffectConfigurationWindow effectConfigurationWindow = new EffectConfigurationWindow();
        effectConfigurationWindow.ShowWindow(this, effectType);

    }

    #endregion



    private void OnEnable()
    {

        drawEffectListProperty = serializedObject.FindProperty("drawEffect");
        drawEffectList = new ReorderableList(serializedObject, drawEffectListProperty, true, true, true, true);
        drawEffectList.drawElementCallback = (rect, index, isActive, isFocused) => DrawListElement(rect, index, isActive, isFocused, ref drawEffectList);
        drawEffectList.drawHeaderCallback = DrawDrawEffectListHeader;
        drawEffectList.onAddCallback = (recordableList) => AddEffect(recordableList, "drawEffect");

        invokeEffectListProperty = serializedObject.FindProperty("invokeEffect");
        invokeEffectList = new ReorderableList(serializedObject, invokeEffectListProperty, true, true, true, true);
        invokeEffectList.drawElementCallback = (rect, index, isActive, isFocused) => DrawListElement(rect, index, isActive, isFocused, ref invokeEffectList);
        invokeEffectList.drawHeaderCallback = DrawInvokeEffectListHeader;
        invokeEffectList.onAddCallback = (recordableList) => AddEffect(recordableList, "invokeEffect");

        battleStartEffectListProperty = serializedObject.FindProperty("battleStartEffect");
        battleStartEffectList = new ReorderableList(serializedObject, battleStartEffectListProperty, true, true, true, true);
        battleStartEffectList.drawElementCallback = (rect, index, isActive, isFocused) => DrawListElement(rect, index, isActive, isFocused, ref battleStartEffectList);
        battleStartEffectList.drawHeaderCallback = DrawBattleStartEffectListHeader;
        battleStartEffectList.onAddCallback = (recordableList) => AddEffect(recordableList, "battleStartEffect");

        liveEffectListProperty = serializedObject.FindProperty("liveEffect");
        liveEffectList = new ReorderableList(serializedObject, liveEffectListProperty, true, true, true, true);
        liveEffectList.drawElementCallback = (rect, index, isActive, isFocused) => DrawListElement(rect, index, isActive, isFocused, ref liveEffectList);
        liveEffectList.drawHeaderCallback = DrawLiveEffectListHeader;
        liveEffectList.onAddCallback = (recordableList) => AddEffect(recordableList, "liveEffect");

        deathEffectListProperty = serializedObject.FindProperty("deathEffect");
        deathEffectList = new ReorderableList(serializedObject, deathEffectListProperty, true, true, true, true);
        deathEffectList.drawElementCallback = (rect, index, isActive, isFocused) => DrawListElement(rect, index, isActive, isFocused, ref deathEffectList);
        deathEffectList.drawHeaderCallback = DrawDeathEffectListHeader;
        deathEffectList.onAddCallback = (recordableList) => AddEffect(recordableList, "deathEffect");

        beforeAttackEffectListProperty = serializedObject.FindProperty("beforeAttackEffect");
        beforeAttackEffectList = new ReorderableList(serializedObject, beforeAttackEffectListProperty, true, true, true, true);
        beforeAttackEffectList.drawElementCallback = (rect, index, isActive, isFocused) => DrawListElement(rect, index, isActive, isFocused, ref beforeAttackEffectList);
        beforeAttackEffectList.drawHeaderCallback = DrawBeforeAttaceEffectHeader;
        beforeAttackEffectList.onAddCallback = (recordableList) => AddEffect(recordableList, "beforeAttackEffect");

        afterAttackEffectListProperty = serializedObject.FindProperty("afterAttackEffect");
        afterAttackEffectList = new ReorderableList(serializedObject, afterAttackEffectListProperty, true, true, true, true);
        afterAttackEffectList.drawElementCallback = (rect, index, isActive, isFocused) => DrawListElement(rect, index, isActive, isFocused, ref afterAttackEffectList);
        afterAttackEffectList.drawHeaderCallback = DrawAfterAttaceEffectHeader;
        afterAttackEffectList.onAddCallback = (recordableList) => AddEffect(recordableList, "afterAttackEffect");

    }

    public override void OnInspectorGUI()
    {
        // ����Ĭ�ϵ�Inspector����

        DrawDefaultInspector();
        serializedObject.Update();
        drawEffectList.DoLayoutList();
        invokeEffectList.DoLayoutList();
        liveEffectList.DoLayoutList();
        deathEffectList.DoLayoutList();
        battleStartEffectList.DoLayoutList();
        beforeAttackEffectList.DoLayoutList();
        afterAttackEffectList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

// �Զ��崰����
public class EffectConfigurationWindow : EditorWindow
{
    // ���ڴ洢�û�ѡ�����������
    private Type selectedEffectType;

    // ���ڴ洢�û�����Ĳ���
    CardBaseSOEditor owner;
    string propertyName;
    private System.Collections.Generic.IEnumerable<Type> effectTypes;

    object[] args;

    public void ShowWindow(CardBaseSOEditor owner, string propertyName)
    {
        this.owner = owner;
        this.propertyName = propertyName;
        GetWindow<EffectConfigurationWindow>("Effect Configuration");
        // ��ȡ����ʵ����IEffect�ӿڵ�����
        effectTypes = Assembly.GetAssembly(typeof(IEffect)).GetTypes()
            .Where(t => t.IsSubclassOf(typeof(IEffect)) && !t.Equals(typeof(IEffect)));

        // ���������˵���ѡ��
        selectedEffectType = effectTypes.FirstOrDefault();
        args = null;
    }

    private string effectTypeName = "";
    private int intValue = 0;
    private GameObject prefabValue = null;

    private int lastResult = -1;

    private void OnGUI()
    {
        // ���������ֶ�
        // �����˵�ѡ����������
        int result = EditorGUILayout.Popup(
                "Effect Type",
                System.Array.IndexOf(effectTypes.ToArray(), selectedEffectType),
                effectTypes.Select(t => t.Name).ToArray());

        if (result < 0) { return; }

        selectedEffectType = effectTypes.ToArray()[result];

        // ����ѡ����������ͻ��Ʋ�ͬ�Ĳ����������
        switch (selectedEffectType.ToString())
        {
            case "IResultReflectEffect":
                if(lastResult != result)
                args = new object[3] { (CardPos.LandPutArea), 1f, 1f };
                EditorGUILayout.LabelField("影响位置");
                args[0] = EditorGUILayout.EnumPopup((CardPos)args[0]);
                EditorGUILayout.LabelField("value(加算)");
                args[1] = EditorGUILayout.FloatField((float)args[1]);
                EditorGUILayout.LabelField("rate(乘算)");
                args[2] = EditorGUILayout.FloatField((float)args[2]);
                break;
            case "IAddCardEffect":
                if(lastResult != result)
                args = new object[2] { 1, CardBaseType.Army };
                EditorGUILayout.LabelField("添加数量");
                args[0] = EditorGUILayout.IntField((int)args[0]);
                EditorGUILayout.LabelField("所添加卡牌类型");
                args[1] = EditorGUILayout.EnumPopup((CardBaseType)args[1]);
                break;


            case "IAddDecision":
                if(args == null)
                args = new object[1] { 1 };
                EditorGUILayout.LabelField("决策点添加数量");
                args[0] = EditorGUILayout.IntField((int)args[0]);
                break;

            case "IChangePossibility":
                if(lastResult != result)
                args = new object[2] { 0, 5 };
                EditorGUILayout.LabelField("种类(0-5, 陆、海、空、军队、法术)");
                args[0] = EditorGUILayout.IntField((int)args[0]);
                EditorGUILayout.LabelField("概率大小(0-10)");
                args[1] = EditorGUILayout.IntField((int)args[1]);
                break;

            case "IAttackInstantly":
                if(lastResult != result)
                args = new object[2] { 0,0 };
                EditorGUILayout.LabelField("造成伤害数值");
                args[0] = EditorGUILayout.IntField((int)args[0]);
                EditorGUILayout.LabelField("目标（0代表自己、1代表敌人）");
                args[1] = EditorGUILayout.IntField((int)args[1]);
                break;
            case "DelayDesisionValueEffect":
                if(lastResult != result)
                args = new object[2] { 0, 1 };
                EditorGUILayout.LabelField("延迟回合数");
                args[0] = EditorGUILayout.IntField((int)args[0]);
                EditorGUILayout.LabelField("增长决策点");
                args[1] = EditorGUILayout.IntField((int)args[1]);
                break;
            case "IDelayLock":
                if(lastResult != result)
                args = new object[3] {0,false,CardPos.LandPutArea };
                EditorGUILayout.LabelField("延迟回合数");
                args[0] = EditorGUILayout.IntField((int)args[0]);
                EditorGUILayout.LabelField("锁的是玩家的战区？");
                args[1] = EditorGUILayout.Toggle((bool)args[1]);
                EditorGUILayout.LabelField("海陆空");
                args[1] = EditorGUILayout.EnumPopup((CardPos)args[2]);
                break;
            default:
                Debug.Log(selectedEffectType); break;
        }
        lastResult = result;

        // ȷ�ϰ�ť
        if (GUILayout.Button("Create"))
        {
            CreateEffect();



            // �رմ���
            this.Close();
        }
    }
    // ����Ч������ķ���
    private void CreateEffect()
    {
        if (!string.IsNullOrEmpty(selectedEffectType.ToString()))
        {
            Type effectType = Assembly.GetAssembly(typeof(IEffect)).GetTypes()
                .FirstOrDefault(t => t.Name == selectedEffectType.ToString());
            if (effectType != null)
            {
                IEffect effectInstance = null;
                // �������ʹ���ʵ�������ò���
                switch (selectedEffectType.ToString())
                {
                    case "IResultReflectEffect":
                        effectInstance = (IResultReflectEffect)Activator.CreateInstance(effectType, (CardPos)args[0], (float)args[1], (float)args[2]);
                        break;
                    case "IAddCardEffect":
                        effectInstance = (IAddCardEffect)Activator.CreateInstance(effectType, (int)args[0], (CardBaseType)args[1]);
                        break;
                    case "IAddDecision":
                        effectInstance = (IAddDecision)Activator.CreateInstance(effectType, (int)args[0]);
                        break;
                    case "IChangePossibility":
                        effectInstance = (IChangePossibility)Activator.CreateInstance(effectType, (int)args[0], (int)args[1]);
                        break;
                    case "IAttackInstantly":
                        effectInstance = (IAttackInstantly)Activator.CreateInstance(effectType, (int)args[0], (int)args[1]);
                        break;

                    case "DelayDesisionValueEffect":
                        effectInstance = (DelayDesisionValueEffect)Activator.CreateInstance(effectType, args[0], args[1]);
                        break;
                }

                // ���´�����Ч���������ӵ�drawEffect�б���
                switch (propertyName)
                {
                    case "drawEffect":
                        (owner.target as CardBaseSO).drawEffect.Add(effectInstance);
                        break;
                    case "invokeEffect":
                        (owner.target as CardBaseSO).invokeEffect.Add(effectInstance);
                        break;
                    case "battleStartEffect":
                        (owner.target as CardBaseSO).battleStartEffect.Add(effectInstance);
                        break;
                    case "liveEffect":
                        (owner.target as CardBaseSO).liveEffect.Add(effectInstance);
                        break;
                    case "deathEffect":
                        (owner.target as CardBaseSO).deathEffect.Add(effectInstance);
                        break;
                    case "beforeAttackEffect":
                        (owner.target as CardBaseSO).beforeAttackEffect.Add(effectInstance);
                        break;
                    case "afterAttackEffect":
                        (owner.target as CardBaseSO).afterAttactEffect.Add(effectInstance);
                        break;


                }
            }
        }
    }
}