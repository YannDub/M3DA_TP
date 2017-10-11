using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveInteractive : MonoBehaviour {

	Curve curve;
	int selected =-1;
	bool move=false;

	LineRender l;
	Camera cam;


	Vector3 mouseToLocal() {
		Vector3 p = Input.mousePosition;
		return curve.transform.InverseTransformPoint (cam.ScreenToWorldPoint (p))-new Vector3(0,0,cam.transform.position.z);
	}


	void Start() {
		l = transform.Find ("LineRender").GetComponent<LineRender> ();
		cam = transform.Find ("Camera").GetComponent<Camera> ();

		curve = transform.parent.gameObject.GetComponent<Curve> ();
	}


	int Closest(Vector3 mouse, float cap) {
		int c = -1;
		if (curve.position.Count != 0) {
			float d = Vector3.Distance (mouse, curve.position [0]);
			c = 0;
			for (int i = 0; i < curve.position.Count; ++i) {
				float r = Vector3.Distance (mouse, curve.position [i]);
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



	// Update is called once per frame
	void Update () {
		if (cam.pixelRect.Contains (Input.mousePosition)) {
			if (Input.GetMouseButtonDown (0)) {
				Vector3 p = mouseToLocal ();
				curve.Add (p);
			}
			if (Input.GetMouseButtonDown (1)) {
				Vector3 p = mouseToLocal ();
				selected = Closest (p, 0.1f);
				if (selected != -1)
					move = true;
			}
			if (Input.GetMouseButton (1)) {
				if (move && selected != -1) {
					Vector3 p = mouseToLocal ();
					curve.position [selected] = p;
				}
			}
			if (Input.GetKeyDown (KeyCode.X)) {
				curve.position.Clear ();
			}
			if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				Vector3 p = mouseToLocal ();
				int change = Closest (p, 0.1f);
				if (change != -1)
					curve.weight [change] *= 0.9f;
			}
			if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
				Vector3 p = mouseToLocal ();
				int change = Closest (p, 0.1f);
				if (change != -1)
					curve.weight [change] *= 1.1f;
			}
		}
		l.Clear ();
		l.currentColor = Color.blue;
		l.AddLine (curve.position);
		l.currentColor = Color.red;
		l.AddLine (curve.DrawNurbs ());
	}

	void OnDrawGizmos() {
		if (curve == null)
			return;
		if (curve.IsSamplePoint ()) {
			Gizmos.color = Color.green;
			Gizmos.DrawSphere (curve.transform.TransformPoint (curve.SamplePoint ()),0.02f);
		}
	}
}
