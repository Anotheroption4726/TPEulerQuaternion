using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TPMath;
using Tools.MathTools;

public class LogicalQuaternionDisplay
{
	public LogicalQuaternionDisplay(string name, VectorDisplay vectorDisplay)
	{
		this.m_Name = name;
		this.m_VectorDisplay = vectorDisplay;
	}

	public void DisplayVector(Quaternion q, Vector3 anchorPos, Color color, float vectorSizeMultiplier = 1f, bool displayAngle = true)
	{
		this.m_Q = q;
		this.m_AnchorPos = anchorPos;
		this.m_VectorSizeMultiplier = vectorSizeMultiplier;
		this.m_DisplayAngle = displayAngle;
		this.m_Color = color;

		Vector3 vector;
		float angle;

		m_Q.ToAngleAxis(out angle, out vector);

		if (m_VectorDisplay)
		{
			if (vector.IsVector3Valid())
			{
				m_VectorDisplay.SetVectorAndAnchorPosition(vector * m_VectorSizeMultiplier, anchorPos);
			}
			m_VectorDisplay.SetColor(color);
			m_LatestDisplayFrame = Time.frameCount;
		}

		
	}

	public void DisplayUI()
	{
		if (!m_DisplayAngle) return;

		GUI.color = m_Color;

		Vector3 qVector;
		float qAngle;

		m_Q.ToAngleAxis(out qAngle, out qVector);

		if (qVector.IsVector3Valid())
		{
			QuaternionDisplayService.DisplayText("angle = " + qAngle.ToString("N02") + " °",
				m_AnchorPos + qVector * m_VectorSizeMultiplier * .8f);
		}
	}

	private string m_Name;
	public string name { get { return m_Name; } }

	private bool m_DisplayAngle;

	private float m_VectorSizeMultiplier = 1f;

	private Color m_Color;

	private VectorDisplay m_VectorDisplay;
	public GameObject gameObject { get { return m_VectorDisplay != null ? m_VectorDisplay.gameObject : null; } }

	private Vector3 m_AnchorPos;

	private Quaternion m_Q;

	private int m_LatestDisplayFrame;
	public int latestDisplayFrame { get { return m_LatestDisplayFrame; } }
}

public class QuaternionDisplayService : MonoBehaviour
{
	private static QuaternionDisplayService _instance;
	public static QuaternionDisplayService instance
	{
		get { return _instance; }
	}

	private static Dictionary<string, LogicalQuaternionDisplay> dicoQuatDisplay;

	public static void Display(string qName, Quaternion q, Vector3 anchorPos, Color color, float vectorSizeMultiplier,bool displayAngle)
	{
		if (!instance)
		{
			Debug.LogError("No QuaternionDisplayService in the scene!");
			return;
		}

		if(dicoQuatDisplay == null) dicoQuatDisplay = new Dictionary<string, LogicalQuaternionDisplay>();

		LogicalQuaternionDisplay qDisplay;
		if (!dicoQuatDisplay.TryGetValue(qName,out qDisplay))
		{
			VectorDisplay vectorDisplay= Instantiate( instance.m_VectorDisplayPrefab);
			vectorDisplay.name = qName + "_VECTOR_DISPLAY";
			vectorDisplay.transform.SetParent(instance ? instance.transform : null);
			qDisplay = new LogicalQuaternionDisplay(qName, vectorDisplay);
			dicoQuatDisplay.Add(qName, qDisplay);
		}

		qDisplay.DisplayVector(q,anchorPos,color, vectorSizeMultiplier, displayAngle);
	}

	[Header("Quaternion Vector Display")]
	[SerializeField]
	private VectorDisplay m_VectorDisplayPrefab;

	[Header("Quaternion Angle Display")]
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

	public static void DisplayTextMiddleCenterUI(string text, Vector3 worldPos,GUIStyle style)
	{
		Vector2 screenPos = (Vector2)Camera.main.WorldToScreenPoint(worldPos);
		Vector2 textSize = style.CalcSize(new GUIContent(text));
		GUI.Label(new Rect(new Vector2(screenPos.x - textSize.x * .5f, Screen.height - screenPos.y - textSize.y * .5f), textSize), text, style);
	} 

	void Update()
	{
		if (dicoQuatDisplay != null && dicoQuatDisplay.Count > 0)
		{
			List<LogicalQuaternionDisplay> toBeRemoved = new List<LogicalQuaternionDisplay>();

			foreach (var item in dicoQuatDisplay.Values)
			{
				if (Time.frameCount - item.latestDisplayFrame > 1)
					toBeRemoved.Add(item);
			}

			for (int i = 0; i < toBeRemoved.Count; i++)
			{
				LogicalQuaternionDisplay qDisplay = toBeRemoved[i];
				Destroy(qDisplay.gameObject);
				dicoQuatDisplay.Remove(qDisplay.name);
			}
		}
	}

	void OnGUI()
	{
		if (dicoQuatDisplay == null || dicoQuatDisplay.Count == 0) return;

		foreach (var item in dicoQuatDisplay)
		{
			LogicalQuaternionDisplay qDisplay = item.Value;
			qDisplay.DisplayUI();
		}
	}
}
