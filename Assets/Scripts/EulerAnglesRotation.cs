using UnityEngine;
using System.Collections;


public class EulerAnglesRotation : MonoBehaviour
{
	[SerializeField]
	private float m_RotSpeed = 80;

	// Update is called once per frame
	void Update()
	{
		Vector3 eulerAngles = transform.eulerAngles;
		Debug.Log("BEFORE : " + transform.eulerAngles);
		eulerAngles.y += Time.deltaTime * m_RotSpeed;
		transform.eulerAngles = eulerAngles;
		Debug.Log("AFTER : " + transform.eulerAngles);
	}
}
