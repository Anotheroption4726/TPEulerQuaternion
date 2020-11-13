using UnityEngine;
using Tools.MathTools;
using TPMath;

public class Rotate : MonoBehaviour {

	[Header("Rotation")]
	[SerializeField]
	private CoordSystem.Spherical m_SphVector;
	private Vector3 m_Vector;
	[SerializeField]
	private Space m_RelativeTo;
	[SerializeField]
	private float m_RotAngularSpeed;

	[Header("Rotation Vector Display")]
	[SerializeField]
	private float m_VectorSizeMultiplier = 1f;
	[SerializeField]
	private Color m_VectorDisplayColor;

	// Use this for initialization
	void Start () {
		m_SphVector.ConvertThetaPhiToRad();
	}

	// Update is called once per frame
	void Update () {

		m_Vector = CoordSystem.SphericalToCartesian(m_SphVector);

		//on fait tourner l'objet
		transform.Rotate(m_Vector.normalized, m_RotAngularSpeed * Time.deltaTime, m_RelativeTo);

		//On dessine le vecteur de rotation
		switch (m_RelativeTo)
		{
			case Space.World:
				VectorDisplayService.Display(name + ".vector", m_Vector, transform.position, m_VectorDisplayColor, m_VectorSizeMultiplier);
				break;
			case Space.Self:
				VectorDisplayService.Display(name + ".vector", transform.TransformDirection(m_Vector), transform.position, m_VectorDisplayColor, m_VectorSizeMultiplier);
				break;
		}
	}
}
