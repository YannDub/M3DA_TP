using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasisInteractive : MonoBehaviour
{
	Camera cam;
	LineRender l;
	Basis b;
	Color[] colorChoice;

	float firstClick;
	bool move=false;
	bool firstMove=false;
	int selected;
	bool directionMoveRight;

	// Use this for initialization
	void Start ()
	{
		l = transform.Find ("LineRender").GetComponent<LineRender> ();
		cam = transform.Find ("Camera").GetComponent<Camera> ();

		b = transform.parent.gameObject.GetComponent<Basis> ();

		colorChoice=new Color[3]{Color.red,Color.blue,Color.green};

	}

	void MoveSelectedKnot(Vector3 mouse) {
		bool ok = false;
		Vector3 mouseLocal = cam.ScreenToWorldPoint (mouse);
		mouseLocal = transform.parent.InverseTransformPoint (mouseLocal);
		double newPos = (double)mouseLocal.x;
		if (newPos > b.knot [selected + 1])
			newPos = b.knot [selected + 1];
		if (newPos<b.knot[selected-1])
			newPos = b.knot [selected - 1];
		b.knot [selected] = newPos;
	}

	int ClosestKnot(float first,float second) {
		if (Mathf.Abs (second - first) < 2)
			return -1;
		bool direction = second - first > 0;
		if (direction) {
			for (int k = b.knot.Count - 1; k >= 0; k--) {
				Vector3 knot = new Vector3 ((float)b.knot [k], 0, 0);
				knot = transform.parent.TransformPoint (knot);
				knot = cam.WorldToScreenPoint (knot);
				if (knot.x < second) {
					if (Mathf.Abs (knot.x - first) < 10.0f) {
						return k;
					} else
						return -1;
				}
			}
		} else {
			for (int k = 0; k <b.knot.Count; ++k) {
				Vector3 knot = new Vector3 ((float)b.knot [k], 0, 0);
				knot = transform.parent.TransformPoint (knot);
				knot = cam.WorldToScreenPoint (knot);
				if (knot.x > second) {
					if (Mathf.Abs (knot.x - first) < 10.0f) {
						return k;
					} else
						return -1;
				}
			}
		}
		return -1;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (cam.pixelRect.Contains (Input.mousePosition)) {
			if (Input.GetMouseButtonDown (0)) {
					firstClick = Input.mousePosition.x;
					move = true;
					firstMove=true;

			}
			if (Input.GetMouseButton (0)) {
				if (move) {
					if (firstMove) {
						selected = ClosestKnot (firstClick, Input.mousePosition.x);
						if (selected != -1 && selected!=b.knot.Count-1 && selected!=0) {
							firstMove = false;
							directionMoveRight = (firstClick - Input.mousePosition.x < 0);
						}
					} else {
						MoveSelectedKnot (Input.mousePosition);
					}
				}

			}
			if (Input.GetMouseButton(1)) {
				Vector3 mouse=cam.ScreenToWorldPoint(Input.mousePosition);
				mouse=b.transform.InverseTransformPoint(mouse);
				if (mouse.x < 0)
					b.currentT = 0.0;
				else if (mouse.x >= 1)
					b.currentT = 0.9999999;
				else
					b.currentT=mouse.x;
			
			}
			if (Input.GetMouseButtonUp (0)) {
				move = false;
			}
			if (Input.GetKeyDown(KeyCode.F1)) {
				if (b.degree>=1) b.degree--;
			}
			if (Input.GetKeyDown (KeyCode.F2)) {
				b.degree++;
			}
		}
			

		l.Clear ();
		BasisDrawCompute ();
	}


	void BasisDrawCompute() {
		int nbBasis = b.knot.Count - b.degree - 1;
		for (int k = 0; k < nbBasis; ++k) {
			l.currentColor = colorChoice [k % 3];
			l.AddLine(b.DrawBasis (k));
		}			
	}


	void OnDrawGizmos ()
	{
		if (b==null)
			return;
		Gizmos.color = Color.red;
		for (int i = 0; i < b.knot.Count; ++i) {
			Gizmos.DrawSphere (transform.TransformPoint (new Vector3 ((float)b.knot [i], 0, 0)), 0.02f);
		}

		Gizmos.color = Color.green;
		//Gizmos.DrawLine (transform.position, transform.position + Vector3.right);
		Gizmos.color = Color.blue;
		Vector3 posX = new Vector3 ((float)b.currentT, 0, 0);
		Gizmos.DrawLine (transform.position+posX, transform.position + Vector3.up+posX);
	}

}


