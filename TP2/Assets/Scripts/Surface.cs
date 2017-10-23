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

	private bool move = false;
	private int selected = -1; 

	// Use this for initialization
	void Start () {
		position = new List<Vector3> ();
		weight = new List<float> ();
		basisU = GameObject.Find ("BasisU").GetComponent<Basis> ();
		basisV = GameObject.Find ("BasisV").GetComponent<Basis> ();
		//SetGrid ();  // TODO : or SetRevolution
		setRevolution();
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

	void setRevolution() {
		nbControlU = 30;
		nbControlV = 5;
		position.Clear ();
		weight.Clear ();
		float u=-1;
		float v=0;
		float angle = 0;
		float r = 1;
		float stepV=2.0f/(nbControlV-1);

		for (int i = 0; i < nbControlV; ++i) {
			r = Random.Range (0.5f, 2f);
			for (int j = 0; j < nbControlU; ++j) {
				position.Add (new Vector3 (r * Mathf.Cos(Mathf.Deg2Rad * angle), r * Mathf.Sin(Mathf.Deg2Rad * angle), v));
				weight.Add (1);
				angle += 360.0f / (nbControlU - 1);
			}
			angle = 0;
			v += stepV;
		}

		basisU.SetFromControlCount (nbControlU);
		basisV.SetFromControlCount (nbControlV);
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

	int Closest(Ray mouse, float cap) {
		int c = -1;
		if (this.position.Count != 0) {
			float d = Vector3.Cross (mouse.direction, position [0] - mouse.origin).magnitude;
			c = 0;
			for (int i = 0; i < this.position.Count; ++i) {
				float r = Vector3.Cross (mouse.direction, this.position [i] - mouse.origin).magnitude;
				Debug.Log (r);
				if (r < d) {
					d = r;
					c = i;
				}
			}
			if (d > cap) {
				c = -1;
			}
		}
		return c;
	}

	private Ray mouseLocal() {
		return Camera.main.ScreenPointToRay (Input.mousePosition);
	}

	// Update is called once per frame
	void Update () {
		basisU.SetFromControlCount (nbControlU);
		basisV.SetFromControlCount (nbControlV);

		if (Input.GetMouseButtonDown (1)) {
			Ray p = mouseLocal();
			selected = Closest (p, 0.1f);
			if (selected != -1)
				move = true;
		}
		if (Input.GetMouseButton (1)) {
			if (move && selected != -1) {
				Ray p = mouseLocal();
				this.position [selected] += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
			}
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			Ray p = mouseLocal();
			int change = Closest (p, 0.1f);
			if (change != -1)
				this.weight [change] *= 0.9f;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			Ray p = mouseLocal();
			int change = Closest (p, 0.1f);
			if (change != -1)
				this.weight [change] *= 1.1f;
		}
	}
}
