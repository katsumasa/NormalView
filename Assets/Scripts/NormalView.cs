using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 法線・接線・従法線を描画するクラス
/// Katsumasa Kimura
/// 2017/06/27 --
/// </summary>
/// 
[ExecuteInEditMode()]
public class NormalView : MonoBehaviour {

	/// <summary>
	/// ラインを描画する際のスケール
	/// </summary>
	public float scale = 0.1f;
	/// <summary>
	/// 法線を描画するか否か
	/// </summary>
	public bool isRenderNormal = true;
	/// <summary>
	/// 接線を描画するか否か
	/// </summary>
	public bool isRenderTangent = true;
	/// <summary>
	/// 従法線を描画するか否か
	/// </summary>
	public bool isRenderBinormal = true;

	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		RenderLines();
		#endif
	}


	#if UNITY_EDITOR
	/// <summary>
	/// Renders the lines.
	/// </summary>
	void RenderLines(){
		Color[] colors = new Color[3];
		colors [0] = Color.blue;
		colors [1] = Color.red;
		colors [2] = Color.green;

		var meshFilters = gameObject.GetComponentsInChildren<MeshFilter> (false);
		if (meshFilters == null) {
			return;
		}
		var idx = 0;
		for (var i = 0; i < meshFilters.Length; i++) {
			var mesh = meshFilters[i].sharedMesh;
			for (var j = 0; j < mesh.normals.Length; j++) {
				var n = mesh.normals [j];
				var t = mesh.tangents [j];
				var v = mesh.vertices [j];
				var t3 = new Vector3 (t.x, t.y, t.z);
				var b = Vector3.Cross (n, t3) * t.w;

				var l1 = meshFilters[i].transform.TransformPoint (v);
				if (isRenderNormal) {
					var l2 = meshFilters [i].transform.TransformPoint (n) * scale + l1;
					Debug.DrawLine (l1, l2, Color.blue);
				}
				if (isRenderTangent) {
					var l2 = meshFilters [i].transform.TransformPoint (t3) * scale + l1;
					Debug.DrawLine (l1, l2, Color.red);
				}
				if (isRenderBinormal) {
					var l2 = meshFilters [i].transform.TransformPoint (b) * scale + l1;
					Debug.DrawLine (l1, l2, Color.green);
				}
				idx++;
			}	 
		}
	}
	#endif
}
