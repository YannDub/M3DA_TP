using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour {


	public List<Vector3> position; // control points position 
	public List<float> weight;     // control points weight
	Basis basis; // basis functions

	// Use this for initialization
	void Start () {
		position = new List<Vector3> ();
		weight = new List<float> ();
		basis = GameObject.Find ("BasisU").GetComponent<Basis> ();
		SetSegment ();
		basis.SetFromControlCount (position.Count);
	}

	public bool IsSamplePoint() {
		return (basis.currentT >= StartInterval () && basis.currentT <= EndInterval ());
	}

	public Vector3 SamplePoint() {
		return PointCurve (basis.currentT);
	}

	double StartInterval() {
		double res = 0.0;

		// TODO : start value of the definition of the curve

		return res;
	}

	double EndInterval() {
		double res = -1.0; // hack to avoid a green dot if TODO are not done.

		// TODO : end value of the definition of the curve

		return res;
	}

	public void Clear() {
		position.Clear ();
		weight.Clear ();
	}

	public void Add(Vector3 p) {
		position.Add (p);
		weight.Add (1.0f);
		basis.SetFromControlCount (position.Count);
	}


	public void SetSegment() {
		Clear ();
		Add (new Vector3 (-0.7f, -0.8f, 0));
		Add (new Vector3 (0, 0.8f, 0));
		Add (new Vector3 (0.6f, 0.5f, 0));
	}

	Vector3 PointCurve(double u) {
		Vector3 result = Vector3.zero;

		// TODO : compute the point of the curve at u

		return result; // * 1.0f / (float)w;
	}

	public List<Vector3> DrawNurbs() {
		List<Vector3> l=new List<Vector3>();

		// TODO : set the values of the points of l 

		return l;
	}

	// Update is called once per frame
	void Update () {
	}
}
