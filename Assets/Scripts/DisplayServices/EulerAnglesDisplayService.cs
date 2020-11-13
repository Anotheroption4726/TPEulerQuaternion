using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TPMath;
using Tools.MathTools;

public class LogicalEulerAnglesDisplay
{
	public LogicalEulerAnglesDisplay(string name)
	{
		this.m_Name = name;
	}

	public void Display(Vector3 eulerAngles, Transform displayTransform, float distanceOnAxis , Color color)
	{
		this.m_EulerAngles = eulerAngles;
		this.m_DisplayTransform= displayTransform;
		this.m_DistanceOnAxis = distanceOnAxis;
		this.m_Color = color;

		m_LatestDisplayFrame = Time.frameCount;
	}

	public void DisplayUI()
	{
		GUI.color = m_Color;

		Vector3 eulerXWorldPos = Vector3.right * m_DistanceOnAxis;
		Vector3 eulerYWorldPos = Vector3.up * m_DistanceOnAxis;
		Vector3 eulerZWorldPos = Vector3.forward * m_DistanceOnAxis;

		if (m_DisplayTransform)
		{
			eulerXWorldPos = m_DisplayTransform.TransformPoint(eulerXWorldPos);
			eulerYWorldPos = m_DisplayTransform.TransformPoint(eulerYWorldPos);
			eulerZWorldPos = m_DisplayTransform.TransformPoint(eulerZWorldPos);
		}

		EulerAnglesDisplayService.DisplayText("euler.x = " + m_EulerAngles.x.ToString("N02") + " °", eulerXWorldPos);
		EulerAnglesDisplayService.DisplayText("euler.y = " + m_EulerAngles.y.ToString("N02") + " °", eulerYWorldPos);
		EulerAnglesDisplayService.DisplayText("euler.z = " + m_EulerAngles.z.ToString("N02") + " °", eulerZWorldPos);
	}

	private string m_Name;
	public string name { get { return m_Name; } }

	private float m_DistanceOnAxis = 5f;

	private Color m_Color;

	private Vector3 m_EulerAngles;
	private Transform m_DisplayTransform;

	private int m_LatestDisplayFrame;
	public int latestDisplayFrame { get { return m_LatestDisplayFrame; } }
}

public class EulerAnglesDisplayService : MonoBehaviour
{
	private static EulerAnglesDisplayService _instance;
	public static EulerAnglesDisplayService instance
	{
		get { return _instance; }
	}

	private static Dictionary<string, LogicalEulerAnglesDisplay> dicoEulerDisplay;

	public static void Display(string eulerName, Vector3 eulerAngles, Transform displayTransform, float distanceOnAxis, Color color)
	{
		if (!instance)
		{
			Debug.LogError("No EulerAnglesDisplayService in the scene!");
			return;
		}

		if (dicoEulerDisplay == null) dicoEulerDisplay = new Dictionary<string, LogicalEulerAnglesDisplay>();

		LogicalEulerAnglesDisplay eulerDisplay;
		if (!dicoEulerDisplay.TryGetValue(eulerName, out eulerDisplay))
		{
			eulerDisplay = new LogicalEulerAnglesDisplay(eulerName);
			dicoEulerDisplay.Add(eulerName, eulerDisplay);
		}

		eulerDisplay.Display(eulerAngles, displayTransform, distanceOnAxis, color);
	}

	[Header("EulerAngles Display")]
	[SerializeField]
	private GUIStyle m_DisplayStyleUI;

	void Awake()
	{
		_instance = this;
	}

	public static void DisplayText(string text, Vector3 worldPos)
	{
		DisplayTextMiddleCenterUI(text, worldPos, instance.m_DisplayStyleUI);
	}

	public static void DisplayTextMiddleCenterUI(string text, Vector3 worldPos, GUIStyle style)
	{
		Vector2 screenPos = (Vector2)Camera.main.WorldToScreenPoint(worldPos);
		Vector2 textSize = style.CalcSize(new GUIContent(text));
		GUI.Label(new Rect(new Vector2(screenPos.x - textSize.x * .5f, Screen.height - screenPos.y - textSize.y * .5f), textSize), text, style);
	}

	void Update()
	{
		if (dicoEulerDisplay!= null && dicoEulerDisplay.Count > 0)
		{
			List<LogicalEulerAnglesDisplay> toBeRemoved = new List<LogicalEulerAnglesDisplay>();

			foreach (var item in dicoEulerDisplay.Values)
			{
				if (Time.frameCount - item.latestDisplayFrame > 1)
					toBeRemoved.Add(item);
			}

			foreach (var item in toBeRemoved)
			{
				dicoEulerDisplay.Remove(item.name);
			}
		}
	}

	void OnGUI()
	{
		if (dicoEulerDisplay == null || dicoEulerDisplay.Count == 0) return;

		foreach (var item in dicoEulerDisplay)
		{
			LogicalEulerAnglesDisplay qDisplay = item.Value;
			qDisplay.DisplayUI();
		}
	}
}



