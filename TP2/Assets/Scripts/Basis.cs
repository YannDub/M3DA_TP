using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Basis: MonoBehaviour {


	public List<double> knot; // 
	public int degree=2;      //
	private int integration = 0;

	public double currentT =0; // to get a sample point (represented by the blue vertical bar).

	// Use this for initialization
	void Start () {
		degree = 2;
		//SetUniform (5); // TODO : replace with SetOpenUniform, SetBezier, ...
		switch (integration) {
		case 0:
			SetUniform (5);
			break;
		case 1:
			SetOpenUniform (5);
			break;
		default:
			SetUniform (5);
			break;
		}
	}


	public void SetUniform(int nb) {
		knot.Clear();

		for (int i = 0; i < nb; i++) {
			knot.Add ((i * 1.0) / (nb * 1.0));
		}
	}

	public void SetOpenUniform(int nb) {
		knot.Clear ();

		if (2 * (degree + 1) <= nb) {
			for (int i = 0; i < degree; i++) {
				knot.Add (0.0);
			}

			for (int i = 0; i <= nb - (2 * degree) - 1; i++) {
				knot.Add ((i * 1.0) / ((nb - (2 * degree) - 1) * 1.0));
			}

			for (int i = 0; i < degree; i++) {
				knot.Add (1.0);
			}
		}

	}

	public void setBezier() {

	}

	public List<Vector3> DrawBasis(int k) {
		int nbPoint = 30;
		List<Vector3> res=new List<Vector3>();

		for (int i = 0; i < nbPoint; i++) {
			double t = (i * 1.0) / (nbPoint * 1.0);
			Vector3 point = new Vector3 ((float)t, (float)EvalNkp (k, degree, t), 0);
			res.Add(point);
		}

		return res;		
	}




	public double EvalNkp(int k,int p,double t) {
		double res = 0.0;

		if (p > 0) {
			double n1 = (t - knot [k]);
			double d1 = (knot [k + p] - knot [k]);
			double left = 0.0;
			if (Math.Abs(d1) > 0.0001)
				left = n1 / d1;

			double n2 = (knot [p + k + 1] - t);
			double d2 = (knot [p + k + 1] - knot [k + 1]);
			double right = 0.0;
			if (Math.Abs(d2) > 0.0001)
				right = n2 / d2;
			
			res = left * EvalNkp (k, p - 1, t) + right * EvalNkp (k + 1, p - 1, t); 
		} else if (t >= knot [k] && t < knot [k + 1])
			res = 1.0;


		return res;
	}


	public void SetFromControlCount(int nb) {
		if (degree + nb + 1 != knot.Count) {
			switch (integration) {
			case 0:
				SetUniform (degree + nb + 1);
				break;
			case 1:
				SetOpenUniform (degree + nb + 1);
				break;
			default:
				SetUniform (degree + nb + 1);
				break;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.C)) {
			this.integration++;
			this.integration = integration % 2;
		}
	}

}
