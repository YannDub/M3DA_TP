using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceInteractive : MonoBehaviour {
	Surface surface;
	int selected =-1;
	bool move=false;

	LineRender l;
	Camera cam;

	Vector3 mouseOld;
	float distance = 0;

	Vector3 mouseToLocal() {
		Vector3 p = Input.mousePosition;
		return surface.transform.InverseTransformPoint (cam.ScreenToWorldPoint (p))-new Vector3(0,0,cam.transform.position.z);
	}


	List<Vector3> DrawSurface(int nbSlice,int nbStack) {
		List<Vector3> res=new List<Vector3>();
		double startU=surface.StartInterval(0);
		double endU=surface.EndInterval(0);
		double stepU=(endU-startU)/(nbSlice-1);
		double u=startU;

		double startV=surface.StartInterval(1);
		double endV=surface.EndInterval(1);
		double stepV=(endV-startV)/(nbStack-1);
		double v=startV;


		Vector3 p;
		for(int i=0;i<nbStack;++i) {
			u=startU;
			for(int j=0;j<nbSlice;++j) {
				res.Add(surface.PointSurface(u,v));
				u+=stepU;
			}	
			v+=stepV;

		}
		return res;
	}

	


	void Start() {
		l = transform.Find ("LineRender").GetComponent<LineRender> ();
		cam = GameObject.Find ("CameraSurface").GetComponent<Camera> ();

		surface = transform.parent.gameObject.GetComponent<Surface> ();
	}


	// Update is called once per frame
	void Update () {
		if (cam.pixelRect.Contains (Input.mousePosition)) {
			if (Input.GetMouseButtonDown (0)) {
				mouseOld = Input.mousePosition;
			}
			if (Input.GetMouseButton (0)) {
				Vector3 mouseNew=Input.mousePosition;
				surface.transform.Rotate (Vector3.forward,(mouseNew.x - mouseOld.x) * 2);
				Vector3 xCam = cam.transform.TransformDirection (Vector3.right);
				surface.transform.Rotate (xCam, (mouseNew.y - mouseOld.y) * 2,Space.World);
				mouseOld = mouseNew;
			}
		}


		l.Clear ();
		l.currentColor = Color.blue;
		l.AddGrid (surface.position, surface.nbControlU, surface.nbControlV);
		l.currentColor = Color.red;
		l.AddGrid (DrawSurface (50, 50), 50, 50);

		if (Input.GetMouseButtonDown (1)) {
			Ray p = mouseLocal();
			selected = Closest (p, 0.1f);
			if (selected != -1) {
				distance = Vector3.Distance (p.origin, surface.transform.TransformPoint (surface.position [selected]));
				move = true;
			}
		}
		if (Input.GetMouseButton (1)) {
			if (move && selected != -1) {
				Ray p = mouseLocal();
				Debug.Log (distance);
				surface.position [selected] = surface.transform.InverseTransformPoint (p.GetPoint (distance));
			}
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			Ray p = mouseLocal();
			int change = Closest (p, 0.1f);
			if (change != -1)
				surface.weight [change] *= 0.9f;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			Ray p = mouseLocal();
			int change = Closest (p, 0.1f);
			if (change != -1)
				surface.weight [change] *= 1.1f;
		}
	}

	int Closest(Ray mouse, float cap) {
		int c = -1;
		if (surface.position.Count != 0) {
			float d = Vector3.Cross (mouse.direction, surface.transform.TransformPoint(surface.position [0]) - mouse.origin).magnitude;
			c = 0;
			for (int i = 0; i < surface.position.Count; ++i) {
				float r = Vector3.Cross (mouse.direction, surface.transform.TransformPoint(surface.position [i]) - mouse.origin).magnitude;

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
		return cam.ScreenPointToRay (Input.mousePosition);
	}

	void OnDrawGizmos() {
		if (surface == null)
			return;
		if (surface.IsSamplePoint ()) {
			Gizmos.color = Color.green;
			Gizmos.DrawSphere (surface.transform.TransformPoint (surface.SamplePoint ()),0.02f);
		}
	}
}

