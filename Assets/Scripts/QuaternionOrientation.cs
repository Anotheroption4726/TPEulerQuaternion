using UnityEngine;
using Tools.MathTools;
using TPMath;

public class QuaternionOrientation : MonoBehaviour
{
	[Header("Quaternion")]
	[SerializeField]
	private Vector3 m_Vector;
	[SerializeField]
	private float m_Angle;

	void Start()
	{
	}

	// Use this for initialization
	void Update()
	{
		transform.rotation = Quaternion.AngleAxis(m_Angle, m_Vector);
	}
}
