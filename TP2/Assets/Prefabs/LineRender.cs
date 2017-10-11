using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour {


	Mesh mesh;

	List<Vector3> position;
	List<int> index;
	List<Color> color;

	public Color currentColor;

	public void Clear() {
		position.Clear ();
		index.Clear ();
		color.Clear ();
	}

	public void AddLine(List<Vector3> pos) {
		int afterIndex = position.Count;
		position.AddRange (pos);
		for (int i = 0; i < pos.Count; ++i) {
			color.Add (currentColor);
		}
		for (int i = 0; i < pos.Count-1; ++i) {
			index.Add (i + afterIndex);
			index.Add (i + 1 + afterIndex);
		}
	}

	public void AddGrid (List<Vector3> pos, int nbSlice,int nbStack) {
		int afterIndex = position.Count;
		position.AddRange (pos);
		for (int i = 0; i < pos.Count; ++i) {
			color.Add (currentColor);
		}
		for (int i = 0; i < nbStack; ++i) {
			for (int j = 0; j < nbSlice-1; ++j) {
				index.Add (j + i * nbSlice + afterIndex);
				index.Add (j+1 + i * nbSlice + afterIndex);
				if (i < nbStack - 1) {
					index.Add (j + i * nbSlice + afterIndex);
					index.Add (j + (i+1) * nbSlice + afterIndex);
					index.Add (j+1 + i * nbSlice + afterIndex);
					index.Add (j+1 + (i+1) * nbSlice + afterIndex);

				}
			}
		}

	}

	// Use this for initialization
	void Start () {
		print ("start LineRender");
		mesh = GetComponent<MeshFilter>().mesh=new Mesh();

		position = new List<Vector3> ();
		index = new List<int> ();
		color = new List<Color> ();
	}
	
	// Update is called once per frame
	void Update () {
		mesh.Clear ();
		mesh.SetVertices (position);
		mesh.SetIndices (index.ToArray(), MeshTopology.Lines,0);
		mesh.SetColors (color);
		
	}
}
