using UnityEngine;
using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TPMath;
using Tools.MathTools;

public class LogicalVectorDisplay
{
	public LogicalVectorDisplay(string name, VectorDisplay vectorDisplay)
	{
		this.m_Name = name;
		this.m_VectorDisplay = vectorDisplay;
	}

	public void Display(Vector3 vector, Vector3 anchorPos, Color color, float vectorSizeMultiplier = 1f)
	{
		this.m_Vector = vector;
		this.m_AnchorPos = anchorPos;
		this.m_VectorSizeMultiplier = vectorSizeMultiplier;
		this.m_Color = color;

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

	private string m_Name;
	public string name { get { return m_Name; } }

	private float m_VectorSizeMultiplier = 1f;

	private Color m_Color;

	private VectorDisplay m_VectorDisplay;
	public GameObject gameObject { get { return m_VectorDisplay != null ? m_VectorDisplay.gameObject : null; } }

	private Vector3 m_AnchorPos;

	private Vector3 m_Vector;

	private int m_LatestDisplayFrame;
	public int latestDisplayFrame { get { return m_LatestDisplayFrame; } }
}

public class VectorDisplayService : MonoBehaviour
{
	private static VectorDisplayService _instance;
	public static VectorDisplayService instance
	{
		get { return _instance; }
	}

	private static Dictionary<string, LogicalVectorDisplay> dicoVectorDisplay;

	public static void Display(string vName, Vector3 vector, Vector3 anchorPos, Color color, float vectorSizeMultiplier)
	{
		if (!instance)
		{
			Debug.LogError("No VectorDisplayService in the scene!");
			return;
		}

		if(dicoVectorDisplay == null) dicoVectorDisplay = new Dictionary<string, LogicalVectorDisplay>();

		LogicalVectorDisplay logVectorDisplay;
		if (!dicoVectorDisplay.TryGetValue(vName, out logVectorDisplay))
		{
			VectorDisplay vectorDisplay= Instantiate( instance.m_VectorDisplayPrefab);
			vectorDisplay.name = vName + "_VECTOR_DISPLAY";
			vectorDisplay.transform.SetParent(instance ? instance.transform : null);
			logVectorDisplay = new LogicalVectorDisplay(vName, vectorDisplay);
			dicoVectorDisplay.Add(vName, logVectorDisplay);
		}

		logVectorDisplay.Display(vector, anchorPos,color, vectorSizeMultiplier);
	}

	[Header("Vector Display")]
	[SerializeField]
	private VectorDisplay m_VectorDisplayPrefab;

	void Awake()
	{
		_instance = this;
	}
	
	void Update()
	{
		if (dicoVectorDisplay != null && dicoVectorDisplay.Count > 0)
		{
			List<LogicalVectorDisplay> toBeRemoved = new List<LogicalVectorDisplay>();

			foreach (var item in dicoVectorDisplay.Values)
			{
				if (Time.frameCount - item.latestDisplayFrame > 1)
					toBeRemoved.Add(item);
			}

			for (int i = 0; i < toBeRemoved.Count; i++)
			{
				LogicalVectorDisplay vDisplay = toBeRemoved[i];
				Destroy(vDisplay.gameObject);
				dicoVectorDisplay.Remove(vDisplay.name);
			}
		}
	}
}
