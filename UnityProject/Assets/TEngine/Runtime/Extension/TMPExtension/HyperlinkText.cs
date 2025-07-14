using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 文本控件,支持超链接
/// </summary>
public class HyperlinkText : TextMeshProUGUI, IPointerClickHandler
{
	/// <summary>
	/// 超链接信息类
	/// </summary>
	private class HyperlinkInfo
	{
		public int startIndex;

		public int endIndex;

		public string name;

		public readonly List<Rect> boxes = new List<Rect>();
	}

	/// <summary>
	/// 解析完最终的文本
	/// </summary>
	private string m_OutputText;

	/// <summary>
	/// 超链接信息列表
	/// </summary>
	private readonly List<HyperlinkInfo> m_HrefInfos = new List<HyperlinkInfo>();

	/// <summary>
	/// 文本构造器
	/// </summary>
	protected static readonly StringBuilder s_TextBuilder = new StringBuilder();

	[Serializable]
	public class HrefClickEvent : UnityEvent<string> { }

	[SerializeField]
	private HrefClickEvent m_OnHrefClick = new HrefClickEvent();

	/// <summary>
	/// 超链接点击事件
	/// </summary>
	public HrefClickEvent onHrefClick
	{
		get { return m_OnHrefClick; }
		set { m_OnHrefClick = value; }
	}


	/// <summary>
	/// 超链接正则
	/// </summary>
	private static readonly Regex s_HrefRegex = new Regex(@"<link=([^>\n\s]+)>(.*?)(</link>)", RegexOptions.Singleline);

	private HyperlinkText mHyperlinkText;

	[SerializeField]
	public string mLink = "www.baidu.com";
	[SerializeField]
	public string mName = "百度";
	public string GetHyperlinkInfo
	{
		get { return string.Format("<a href={0:link}>[{1:name}]</a>", mLink, mName); }
	}

	protected override void Awake()
	{
		base.Awake();
		mHyperlinkText = GetComponent<HyperlinkText>();
	}
	protected override void OnEnable()
	{
		base.OnEnable();
		mHyperlinkText.onHrefClick.AddListener(OnHyperlinkTextInfo);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		mHyperlinkText.onHrefClick.RemoveListener(OnHyperlinkTextInfo);
	}


	public override void SetVerticesDirty()
	{
		base.SetVerticesDirty();
#if UNITY_EDITOR
		if (UnityEditor.PrefabUtility.GetPrefabType(this) == UnityEditor.PrefabType.Prefab)
		{
			return;
		}
#endif
		//  m_OutputText = GetOutputText(text);
		//text = GetHyperlinkInfo;
		m_OutputText = GetOutputText(text);
	}


	/// <summary>
	/// 获取超链接解析后的最后输出文本
	/// </summary>
	/// <returns></returns>
	protected virtual string GetOutputText(string outputText)
	{
		s_TextBuilder.Length = 0;
		m_HrefInfos.Clear();
		var indexText = 0;
		foreach (Match match in s_HrefRegex.Matches(outputText))
		{
			s_TextBuilder.Append(outputText.Substring(indexText, match.Index - indexText));
			s_TextBuilder.Append("<color=blue>");  // 超链接颜色

			var group = match.Groups[1];
			var hrefInfo = new HyperlinkInfo
			{
				startIndex = s_TextBuilder.Length * 4, // 超链接里的文本起始顶点索引
				endIndex = (s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
				name = group.Value
			};
			m_HrefInfos.Add(hrefInfo);

			s_TextBuilder.Append(match.Groups[2].Value);
			s_TextBuilder.Append("</color>");
			indexText = match.Index + match.Length;
		}
		s_TextBuilder.Append(outputText.Substring(indexText, outputText.Length - indexText));
		return s_TextBuilder.ToString();
	}

	/// <summary>
	/// 点击事件检测是否点击到超链接文本
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerClick(PointerEventData eventData)
	{
		Vector2 lp = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out lp);
		var go = GameObject.Find("UIRoot/GUICamera");
		var c = go.GetComponent<Camera>();
		int linkIndex = TMP_TextUtilities.FindIntersectingLink(mHyperlinkText, Input.mousePosition,c);
		if (linkIndex == -1)
			return;

		TMP_LinkInfo linkInfo = mHyperlinkText.textInfo.linkInfo[linkIndex];
            
		string linkText = linkInfo.GetLinkText();
		
		string linkId = linkInfo.GetLinkID();
            
		if (string.IsNullOrEmpty(linkId))
			return;

		// 触发点击回调;
		if (null != this.onHrefClick)
			this.onHrefClick.Invoke(linkId);
		// Log.Error($"click linkText：{linkText}  linkId：{linkId}" );
	}
	/// <summary>
	/// 当前点击超链接回调
	/// </summary>
	/// <param name="info">回调信息</param>
	private void OnHyperlinkTextInfo(string info)
	{
		Debug.Log("超链接信息：" + info);
	}

}
