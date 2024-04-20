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

// 自定义编辑器窗口的类
[CanEditMultipleObjects]
[CustomEditor(typeof(CardBaseSO), true)]
public class CardBaseSOEditor : Editor
{
    


    #region 绘制Effect列表
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
    void DrawListElement(Rect rect, int index, bool isActive, bool isFocused,ref ReorderableList list)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
        // 获取element对应的对象实例
        object effectInstance = element.managedReferenceValue;

        // 获取effectInstance的Type
        Type instanceType = effectInstance.GetType();

        // 检查该Type是否实现了IResultReflectEffect接口
        if (instanceType.IsAssignableFrom(typeof(IResultReflectEffect)))
        {
            list.elementHeight = EditorGUIUtility.singleLineHeight *5;
            // 绘制类名标题
            EditorGUI.LabelField(new Rect(rect.x, rect.y , rect.width, EditorGUIUtility.singleLineHeight), "IResultReflectEffect");

            // 绘制pos属性标题
            EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight), "Position:");

            // 绘制pos属性
            (effectInstance as IResultReflectEffect).pos = (CardPos)EditorGUI.EnumPopup(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight*2, 100, EditorGUIUtility.singleLineHeight),
                (effectInstance as IResultReflectEffect).pos
            );

            // 绘制value属性标题
            EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight*3, rect.width, EditorGUIUtility.singleLineHeight), "Value:");

            // 绘制value属性
            (effectInstance as IResultReflectEffect).value = EditorGUI.FloatField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4, 100, EditorGUIUtility.singleLineHeight),
                (effectInstance as IResultReflectEffect).value
            );
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
        drawEffectList.drawElementCallback = (rect, index, isActive, isFocused) =>DrawListElement(rect, index, isActive, isFocused,ref drawEffectList);
        drawEffectList.drawHeaderCallback = DrawDrawEffectListHeader;
        drawEffectList.onAddCallback = (recordableList) =>AddEffect(recordableList, "drawEffect");

        invokeEffectListProperty = serializedObject.FindProperty("invokeEffect");
        invokeEffectList = new ReorderableList(serializedObject, invokeEffectListProperty, true, true, true, true);
        invokeEffectList.drawElementCallback = (rect, index, isActive, isFocused) => DrawListElement(rect, index, isActive, isFocused,ref invokeEffectList);
        invokeEffectList.drawHeaderCallback = DrawInvokeEffectListHeader;
        invokeEffectList.onAddCallback = (recordableList) => AddEffect(recordableList, "invokeEffect");

        battleStartEffectListProperty = serializedObject.FindProperty("battleStartEffect");
        battleStartEffectList = new ReorderableList(serializedObject, battleStartEffectListProperty, true, true, true, true);
        battleStartEffectList.drawElementCallback = (rect, index, isActive, isFocused) => DrawListElement(rect, index, isActive, isFocused,ref battleStartEffectList);
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
        // 绘制默认的Inspector界面

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

// 自定义窗口类
public class EffectConfigurationWindow : EditorWindow
{
    // 用于存储用户选择的子类类型
    private Type selectedEffectType;

    // 用于存储用户输入的参数
    CardBaseSOEditor owner;
    string propertyName;
    private System.Collections.Generic.IEnumerable<Type> effectTypes;

    object[] args;
    
    public void ShowWindow(CardBaseSOEditor owner, string propertyName)
    {
        this.owner = owner;
        this.propertyName = propertyName;
        GetWindow<EffectConfigurationWindow>("Effect Configuration");
        // 获取所有实现了IEffect接口的子类
        effectTypes = Assembly.GetAssembly(typeof(IEffect)).GetTypes()
            .Where(t => t.IsSubclassOf(typeof(IEffect)) && !t.Equals(typeof(IEffect)));

        // 设置下拉菜单的选项
        selectedEffectType = effectTypes.FirstOrDefault();
    }

    private string effectTypeName = "";
    private int intValue = 0;
    private GameObject prefabValue = null;



    private void OnGUI()
    {
        // 绘制配置字段
        // 下拉菜单选择子类类型
        int result = EditorGUILayout.Popup(
                "Effect Type",
                System.Array.IndexOf(effectTypes.ToArray(), selectedEffectType),
                effectTypes.Select(t => t.Name).ToArray());

        if (result < 0) { return; }

        selectedEffectType = effectTypes.ToArray()[result];

        // 根据选择的子类类型绘制不同的参数输入界面
        switch (selectedEffectType.ToString())
        {
            case "IResultReflectEffect":
                args = new object[3] {(CardPos.LandPutArea),1f,1f }; 
                EditorGUILayout.LabelField("影响位置");
                args[0] = EditorGUILayout.EnumPopup((CardPos)args[0]);
                EditorGUILayout.LabelField("value(加算)");
                args[1] = EditorGUILayout.FloatField((float)args[1]);
                EditorGUILayout.LabelField("rate(乘算)");
                args[2] = EditorGUILayout.FloatField((float)args[2]);
                break;
            
            // 添加更多的case来处理其他子类类型的参数
            default:
                Debug.Log(selectedEffectType); break;
        }

        // 确认按钮
        if (GUILayout.Button("Create"))
        {
            // 创建新的Effect对象
            CreateEffect();

            

            // 关闭窗口
            this.Close();
        }
    }
    // 创建效果对象的方法
    private void CreateEffect()
    {
        if (!string.IsNullOrEmpty(selectedEffectType.ToString()))
        {
            Type effectType = Assembly.GetAssembly(typeof(IEffect)).GetTypes()
                .FirstOrDefault(t => t.Name == selectedEffectType.ToString());
            if (effectType != null)
            {
                IEffect effectInstance = null;
                // 根据类型创建实例并设置参数
                switch (selectedEffectType.ToString())
                {
                    case "IResultReflectEffect":
                        effectInstance = (IResultReflectEffect)Activator.CreateInstance(effectType, (CardPos)args[0], (float)args[1], (float)args[2]);
                        break;
                        // 添加更多的case来处理其他子类类型的实例化
                }

                // 将新创建的效果对象添加到drawEffect列表中
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