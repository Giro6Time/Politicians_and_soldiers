using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;


public class DialogPanel : MonoBehaviour
{
	[Header("属性（策划自用9成新）")]
	public AnimationCurve curve;
	public float moveDuration = 5f;

	[Header("关联组件")]
	public TMP_Text m_name;
	public TMP_Text m_content;
	public Button[] m_buttons;
	public Image m_textField;
	public Image m_talker1;
	public Image m_talker2;
	public RectTransform panelRect;


	public Action onEntered;
	public Action onSubPanelShown;
	public Action onImageShown;
	public Action onTextShown;
	public Action onClosed;

	DialogUnit m_dialogUnit;
	private Vector2 screenPosition;
	private Vector2 waitingPosition;
	private bool opening = false;
	private bool closing = false;
	private float startTime;

	private Vector3 talker1Anchor;
	private Vector3 talker2Anchor;

	void Awake()
	{
		talker1Anchor = m_talker1.transform.position;
		talker2Anchor = m_talker2.transform.position;
	}

	public void Open()
	{
        if (opening)
        {
			return;
        }
        startTime = Time.time;
		opening = true;
		gameObject.SetActive(true);
	}

	public void StartPlay(DialogUnit dialogUnit)
	{
		m_name.gameObject.SetActive(true);
		m_name.text = dialogUnit.m_name;
		m_content.gameObject.SetActive(true);
		m_content.text = dialogUnit.m_content;
		m_dialogUnit = dialogUnit;
		m_textField.gameObject.SetActive(true);

		if (dialogUnit.m_SR1)
		{
			m_talker1.color = new Color(1, 1, 1, 1);
			m_talker1.sprite = dialogUnit.m_SR1;
			m_talker1.transform.position = talker1Anchor + dialogUnit.m_SR1_Offset;
			m_talker1.SetNativeSize();
		}
		if (dialogUnit.m_SR2)
		{
			m_talker2.color = new Color(1, 1, 1, 1);
			m_talker2.sprite = dialogUnit.m_SR2;
			m_talker2.transform.position = talker2Anchor + dialogUnit.m_SR2_Offset;
			m_talker2.SetNativeSize();
        }
		for (int i = 0; i < dialogUnit.m_options.Count; i++)
		{
			int optionIndex = i;
			m_buttons[i].gameObject.SetActive(true);
			m_buttons[i].onClick.RemoveAllListeners();
			foreach(DialogEffect dialogEffect in dialogUnit.m_options[optionIndex].m_effect)
			{
				m_buttons[i].onClick.AddListener(() => OnOptionSelected(dialogEffect));
			}
			m_buttons[i].transform.Find("Content").GetComponent<TMP_Text>().text = m_dialogUnit.m_options[optionIndex].m_description;
		}
	}

	public void OnOptionSelected(DialogEffect effect)
	{
		effect.Trigger(Player.Instance);
		UnitOver();
	}

	public void UnitOver()
	{
		m_name.text = "";
		m_content.text = "";
		m_textField.gameObject.SetActive(false);

		m_talker1.color = new Color(1, 1, 1, 0);
		m_talker1.sprite = null;
		m_talker2.color = new Color(1, 1, 1, 0);
		m_talker2.sprite = null;
		foreach (var button in m_buttons)
		{
			button.gameObject.SetActive(false);
		}
		DialogManager.Instance.OnDialogUnitOver(null);
	}

	public void Close()
	{
		if(closing) return;
		startTime = Time.time;
		closing = true;
	}

	#region

	private void Start()
	{
		panelRect.anchoredPosition = new Vector2(Screen.width, panelRect.anchoredPosition.y);
		screenPosition = new Vector2(0, panelRect.anchoredPosition.y);
		waitingPosition = panelRect.anchoredPosition;
	}



	private void Update()
	{
		if (opening)
		{
			float t = (Time.time - startTime) / moveDuration;
			float step = curve.Evaluate(t);
			panelRect.anchoredPosition = Vector2.Lerp(waitingPosition, screenPosition, step);

			if (t >= 1.0f)
			{
				opening = false;
				onEntered?.Invoke();
			}
		}

		if (closing)
		{
			float t = (Time.time - startTime) / moveDuration;
			float step = curve.Evaluate(t);
			panelRect.anchoredPosition = Vector2.Lerp(screenPosition, waitingPosition, step);

			if (t >= 1.0f)
			{
				closing = false;
				onClosed?.Invoke();


				//UPDATE:显示选择界面功能
				GameManager.Instance.gameFlowController.OpenMiniGamePanel();
			}
		}
	}

	#endregion

}
