using UnityEngine;
using Tools.MathTools;
using TPMath;

public class RotateAround : MonoBehaviour
{
	[Header("Rotation")]
	[SerializeField]
	private Transform m_PivotPoint;
	[SerializeField]
	private CoordSystem.Spherical m_SphVector;
	private Vector3 m_Vector;

	[SerializeField]
	private float m_RotAngularSpeed;

	[Header("Rotation Vector Display")]
	[SerializeField]
	private float m_VectorSizeMultiplier = 1f;
	[SerializeField]
	private Color m_VectorDisplayColor;

	// Use this for initialization
	void Start()
	{
		m_SphVector.ConvertThetaPhiToRad();
	}

	// Update is called once per frame
	void Update()
	{
		m_Vector = CoordSystem.SphericalToCartesian(m_SphVector);

		//on fait tourner l'objet
		transform.RotateAround(m_PivotPoint.position, m_Vector.normalized, m_RotAngularSpeed * Time.deltaTime);

		VectorDisplayService.Display(name + ".rotVector", m_Vector, m_PivotPoint.position, m_VectorDisplayColor, m_VectorSizeMultiplier);
	}
}
