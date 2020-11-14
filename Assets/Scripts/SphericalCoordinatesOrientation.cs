using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalCoordinatesOrientation : MonoBehaviour
{
	[Header("Spherical Coordinates ρ θ φ")]
	[SerializeField]
	private float rayon;    //	radius
	[SerializeField]
	private float longitude;    //	polar
	[SerializeField]
	private float latitude;   //	elevation


	[Header("Quaternion angle")]
	[SerializeField]
	private float m_Angle;

	void Start()
	{
	}

	// Use this for initialization
	void Update()
	{
		transform.rotation = Quaternion.AngleAxis(m_Angle, SphericalToCartesian(rayon, DegreesToRadiants(longitude), DegreesToRadiants(latitude)));
	}

	private Vector3 SphericalToCartesian(float radius, float polar, float elevation)
	{
		Vector3 outCart = new Vector3();

		float a = radius * Mathf.Cos(elevation);
		outCart.x = a * Mathf.Cos(polar);
		outCart.y = radius * Mathf.Sin(elevation);
		outCart.z = a * Mathf.Sin(polar);

		return outCart;
	}

	private float DegreesToRadiants(float degrees)
	{
		float radiants;

		radiants = Convert.ToSingle(Math.PI * degrees / 180.0);
		//Mathf.DegtoRad ou un truc comme ça

		return radiants;
	}
}
