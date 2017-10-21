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
		//SetSegment ();
		basis.SetFromControlCount (position.Count);
		SetCircle();
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
		res = basis.knot[basis.degree];

		return res;
	}

	double EndInterval() {
		double res = -1.0; // hack to avoid a green dot if TODO are not done.

		// TODO : end value of the definition of the curve
		res = basis.knot[basis.knot.Count - 1] - 0.00001;

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

	public void SetCircle() {
		Clear ();
		Add (new Vector3 (0.0f, 0.0f, 0.0f));
		Add (new Vector3 (-0.5f, 0.0f, 0.0f));
		Add (new Vector3 (-0.25f, Mathf.Sqrt(0.5f * 0.5f - 0.25f * 0.25f), 0.0f));
		Add (new Vector3 (-0.0f, Mathf.Sqrt(1f * 1f - 0.5f * 0.5f), 0.0f));
		Add (new Vector3 (0.25f, Mathf.Sqrt(0.5f * 0.5f - 0.25f * 0.25f), 0.0f));
		Add (new Vector3 (0.5f, 0.0f, 0.0f));
		Add (new Vector3 (0.0f, 0.0f, 0.0f));

		weight [1] = weight [3] = weight [5] = Mathf.Cos (Mathf.Deg2Rad * 60);
	}

	Vector3 PointCurve(double u) {
		Vector3 result = Vector3.zero;
		float w = 0;

		// TODO : compute the point of the curve at u
		//int nbBasis = basis.knot.Count - basis.degree - 1;
		int nbBasis = position.Count;
		for (int i = 0; i < nbBasis; i++) {
			float eval = ((float)basis.EvalNkp (i, basis.degree, u));
			result += eval * position[i] * weight[i];
			w += eval * weight [i];
		}

		result = result / w;
		return result; // * 1.0f / (float)w;
	}

	public List<Vector3> DrawNurbs() {
		List<Vector3> l=new List<Vector3>();

		// TODO : set the values of the points of l
		for (int i = 0; i < 30; i++) {
			double start = this.StartInterval ();
			double end = this.EndInterval ();
			double t = start + (end - start) * ((double)i / (30.0 - 1.0));
			Vector3 point = PointCurve (t);
			l.Add(point);
		}

		return l;
	}

	// Update is called once per frame
	void Update () {
	}
}
