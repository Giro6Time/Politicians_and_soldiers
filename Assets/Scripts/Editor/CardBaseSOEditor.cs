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
    void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = drawEffectList.serializedProperty.GetArrayElementAtIndex(index);
        // ��ȡelement��Ӧ�Ķ���ʵ��
        object effectInstance = element.managedReferenceValue;

        // ��ȡeffectInstance��Type
        Type instanceType = effectInstance.GetType();

        // ����Type�Ƿ�ʵ����IResultReflectEffect�ӿ�
        if (instanceType.IsAssignableFrom(typeof(IResultReflectEffect)))
        {
            drawEffectList.elementHeight = EditorGUIUtility.singleLineHeight *5;
            // ������������
            EditorGUI.LabelField(new Rect(rect.x, rect.y , rect.width, EditorGUIUtility.singleLineHeight), "IResultReflectEffect");

            // ����pos���Ա���
            EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight), "Position:");

            // ����pos����
            (effectInstance as IResultReflectEffect).pos = (CardPos)EditorGUI.EnumPopup(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight*2, 100, EditorGUIUtility.singleLineHeight),
                (effectInstance as IResultReflectEffect).pos
            );

            // ����value���Ա���
            EditorGUI.LabelField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight*3, rect.width, EditorGUIUtility.singleLineHeight), "Value:");

            // ����value����
            (effectInstance as IResultReflectEffect).value = EditorGUI.FloatField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4, 100, EditorGUIUtility.singleLineHeight),
                (effectInstance as IResultReflectEffect).value
            );
        }


    }

    void AddEffect(ReorderableList reorderableList)
    {
        EffectConfigurationWindow effectConfigurationWindow = new EffectConfigurationWindow();
        effectConfigurationWindow.ShowWindow(this, "drawEffect");

    }

    #endregion



    private void OnEnable()
    {
        (target as CardBaseSO).drawEffect = new();

        drawEffectListProperty = serializedObject.FindProperty("drawEffect");
        drawEffectList = new ReorderableList(serializedObject, drawEffectListProperty, true, true, true, true);
        drawEffectList.drawElementCallback = DrawListElement;
        drawEffectList.drawHeaderCallback = DrawDrawEffectListHeader;
        drawEffectList.onAddCallback = AddEffect;

        invokeEffectListProperty = serializedObject.FindProperty("invokeEffect");
        invokeEffectList = new ReorderableList(serializedObject, invokeEffectListProperty, true, true, true, true);
        invokeEffectList.drawElementCallback = DrawListElement;
        invokeEffectList.drawHeaderCallback = DrawInvokeEffectListHeader;
        invokeEffectList.onAddCallback = AddEffect;

        battleStartEffectListProperty = serializedObject.FindProperty("battleStartEffect");
        battleStartEffectList = new ReorderableList(serializedObject, battleStartEffectListProperty, true, true, true, true);
        battleStartEffectList.drawElementCallback = DrawListElement;
        battleStartEffectList.drawHeaderCallback = DrawBattleStartEffectListHeader;
        battleStartEffectList.onAddCallback = AddEffect;

        liveEffectListProperty = serializedObject.FindProperty("liveEffect");
        liveEffectList = new ReorderableList(serializedObject, liveEffectListProperty, true, true, true, true);
        liveEffectList.drawElementCallback = DrawListElement;
        liveEffectList.drawHeaderCallback = DrawLiveEffectListHeader;
        liveEffectList.onAddCallback = AddEffect;

        deathEffectListProperty = serializedObject.FindProperty("deathEffect");
        deathEffectList = new ReorderableList(serializedObject, deathEffectListProperty, true, true, true, true);
        deathEffectList.drawElementCallback = DrawListElement;
        deathEffectList.drawHeaderCallback = DrawDeathEffectListHeader;
        deathEffectList.onAddCallback = AddEffect;

        beforeAttackEffectListProperty = serializedObject.FindProperty("beforeAttackEffect");
        beforeAttackEffectList = new ReorderableList(serializedObject, beforeAttackEffectListProperty, true, true, true, true);
        beforeAttackEffectList.drawElementCallback = DrawListElement;
        beforeAttackEffectList.drawHeaderCallback = DrawBeforeAttaceEffectHeader;
        beforeAttackEffectList.onAddCallback = AddEffect;

        afterAttackEffectListProperty = serializedObject.FindProperty("afterAttackEffect");
        afterAttackEffectList = new ReorderableList(serializedObject, afterAttackEffectListProperty, true, true, true, true);
        afterAttackEffectList.drawElementCallback = DrawListElement;
        afterAttackEffectList.drawHeaderCallback = DrawAfterAttaceEffectHeader;
        afterAttackEffectList.onAddCallback = AddEffect;

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
    }

    private string effectTypeName = "";
    private int intValue = 0;
    private GameObject prefabValue = null;



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
                args = new object[1] {1f };
                args[0] = EditorGUILayout.FloatField((float)args[0]);
                break;
            
            // ��Ӹ����case�����������������͵Ĳ���
            default:
                Debug.Log(selectedEffectType); break;
        }

        // ȷ�ϰ�ť
        if (GUILayout.Button("Create"))
        {
            // �����µ�Effect����
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
                        effectInstance = (IResultReflectEffect)Activator.CreateInstance(effectType, (owner.target as CardBaseSO).matchedPos, (float)args[0]);
                        break;
                        // ��Ӹ����case�����������������͵�ʵ����
                }

                // ���´�����Ч��������ӵ�drawEffect�б���
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