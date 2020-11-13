using UnityEngine;
using System.Collections;

public class DisplayQuaternionAndEulerAngles : MonoBehaviour {

	[Header("Display Quaternion")]
	[SerializeField]
	private bool m_DisplayQuaternion = true;
	[SerializeField]
	private Color m_DisplayQuaternionColor = Color.white;
	[SerializeField]
	private float m_DisplayQuaternionVectorSizeMultiplier = 5f;

	[Header("Display EulerAngles")]
	[SerializeField]
	private bool m_DisplayEulerAngles = true;
	[SerializeField]
	private Space m_DisplayEulerAnglesSpace = Space.Self;
	[SerializeField]
	private Color m_DisplayEulerAnglesColor = Color.white;
	[SerializeField]
	private float m_DisplayEulerAnglesDistanceOnAxis = 7f;

	// Update is called once per frame
	void Update () {
		if (m_DisplayQuaternion)
			QuaternionDisplayService.Display(transform.name+".rotation", transform.rotation, transform.position, m_DisplayQuaternionColor, m_DisplayQuaternionVectorSizeMultiplier, true);

		if (m_DisplayEulerAngles)
			EulerAnglesDisplayService.Display(transform.name + ".eulerAngles", transform.eulerAngles, m_DisplayEulerAnglesSpace == Space.Self?transform:null, m_DisplayEulerAnglesDistanceOnAxis, m_DisplayEulerAnglesColor);
	}
}
