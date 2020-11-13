using UnityEngine;
using Tools.MathTools;
using TPMath;

public class EulerAnglesToQuaternionOrientation : MonoBehaviour
{
	[Header("Euler Angles")]
	[SerializeField]
	private Vector3 m_EulerAngles;

	// Use this for initialization
	void Update()
	{
		transform.rotation = Quaternion.Euler(m_EulerAngles);
	}
}
