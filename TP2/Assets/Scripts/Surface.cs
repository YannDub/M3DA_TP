using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour {
	public List<Vector3> position; // control points position
	public List<float> weight;     // control points weight

	public int nbControlU; // in direction U
	public int nbControlV; // in direction V

	public Basis basisU; // basis functions in direction U
	public Basis basisV; // basis functions in direction V

	// Use this for initialization
	void Start () {
		position = new List<Vector3> ();
		weight = new List<float> ();
		basisU = GameObject.Find ("BasisU").GetComponent<Basis> ();
		basisV = GameObject.Find ("BasisV").GetComponent<Basis> ();
		SetGrid ();  // TODO : or SetRevolution
		basisU.SetFromControlCount (nbControlU);
		basisV.SetFromControlCount (nbControlV);
	}


	public bool IsSamplePoint() {
		return (basisU.currentT >= StartInterval (0) && basisU.currentT <= EndInterval (0) &&
			basisV.currentT >= StartInterval (1) && basisV.currentT <= EndInterval (1));
	}

	public Vector3 SamplePoint() {
		return PointSurface (basisU.currentT,basisV.currentT);
	}

	public double StartInterval(int axis) {
		if (axis==0) return basisU.knot [basisU.degree];
		else return basisV.knot [basisV.degree];
	}

	public double EndInterval(int axis) {
		if (axis==0) return basisU.knot [nbControlU]-0.0005;
		else return basisV.knot [nbControlV]-0.0005;
	}

	void SetGrid() {
		nbControlU=5;
		nbControlV=4;
		position.Clear();
		weight.Clear ();
		float u=-1;
		float v=-1;
		float stepU=2.0f/(nbControlU-1);
		float stepV=2.0f/(nbControlV-1);
		for(int i=0;i<nbControlV;++i) {
			u=-1;
			for(int j=0;j<nbControlU;++j) {
				position.Add(new Vector3(u,v,Random.Range(-0.5f,0.5f)));
				weight.Add(1);
				u+=stepU;
			}
			v+=stepV;
		}
		basisU.SetFromControlCount(nbControlU);
		basisV.SetFromControlCount(nbControlV);
	}



	public Vector3 PointSurface(double u,double v) {
		Vector3 result=Vector3.zero;
		float w=0.0f;


		for (int l = 0; l < nbControlV; l++) {
			Vector3 pkl = Vector3.zero;
			float wkl = 0.0f;

			for (int k = 0; k < nbControlU; k++) {
				float evalU = ((float)basisU.EvalNkp (k, basisU.degree, u));
				pkl += evalU * position [k + l * nbControlU] * weight [k + l * nbControlU];
				wkl += evalU * weight [k + l * nbControlU];
			}

			float evalV = ((float)basisV.EvalNkp (l, basisV.degree, v));
			result += evalV * pkl * wkl /** weight[0 + l * nbControlU]*/;
			w += evalV * wkl /** weight [0 + l * nbControlU]*/;
		}

		return result / w; // * 1.0f / (float)w;
	}

	// Update is called once per frame
	void Update () {
		basisU.SetFromControlCount (nbControlU);
		basisV.SetFromControlCount (nbControlV);
	}
}
