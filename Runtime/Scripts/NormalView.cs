using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UTJ.NormalView
{

	/// <summary>
	/// 法線・接線・従法線を描画するクラス
	/// Katsumasa Kimura
	/// 2017/06/27 --
	/// </summary>
	/// 
	[ExecuteInEditMode()]
	public class NormalView : MonoBehaviour
	{
#if UNITY_EDITOR				
		/// <summary>
		/// ラインを描画する際のスケール
		/// </summary>
		[Tooltip("ラインを描画する際のスケール")]
		public float scale = 0.1f;
		
		/// <summary>
		/// 法線を描画するか否か
		/// </summary>
		[Tooltip("法線を描画するか否か")]
		public bool isRenderNormal = true;
		
		/// <summary>
		/// 接線を描画するか否か
		/// </summary>
		[Tooltip("接線を描画するか否か")]
		public bool isRenderTangent = true;
		
		/// <summary>
		/// 従法線を描画するか否か
		/// </summary>
		[Tooltip("従法線を描画するか否か")]
		public bool isRenderBinormal = true;
		
		
		/// <summary>
		/// The is normal check first.
		/// </summary>
		bool isNormalCheckFirst = true;
		
		/// <summary>
		/// The is tangent check first.
		/// </summary>
		bool isTangentCheckFirst = true;
		
		/// <summary>
		/// 法線・接線・従法線の色
		/// </summary>
		Color[] colors;


		void Awake()
        {
			Color[] colors = new Color[3];
			colors[0] = Color.blue;
			colors[1] = Color.red;
			colors[2] = Color.green;
		}


		// Update is called once per frame
		void Update()
		{
			RenderLines();
		}


		/// <summary>
		/// Renders the lines.
		/// </summary>
		void RenderLines()
		{			
			var meshFilters = gameObject.GetComponentsInChildren<MeshFilter>(false);
			if (meshFilters == null)
			{
				return;
			}
			var idx = 0;
			for (var i = 0; i < meshFilters.Length; i++)
			{
				var mesh = meshFilters[i].sharedMesh;
				for (var j = 0; j < mesh.normals.Length; j++)
				{
					var v = mesh.vertices[j];
					if (mesh.normals.Length <= j)
					{
						if (isNormalCheckFirst)
						{
							Debug.Log("Normals Length Check!");
							isNormalCheckFirst = false;
						}
						break;
					}
					var n = mesh.normals[j];
					var l1 = meshFilters[i].transform.TransformPoint(v);
					if (isRenderNormal)
					{
						var l2 = meshFilters[i].transform.TransformPoint(n) * scale + l1;
						Debug.DrawLine(l1, l2, Color.blue);
					}

					if (mesh.tangents.Length <= j)
					{
						if (isTangentCheckFirst)
						{
							Debug.Log("Tangents Length Check!");
							isTangentCheckFirst = false;
						}
						continue;
					}
					var t = mesh.tangents[j];
					var t3 = new Vector3(t.x, t.y, t.z);
					var b = Vector3.Cross(n, t3) * t.w;

					if (isRenderTangent)
					{
						var l2 = meshFilters[i].transform.TransformPoint(t3) * scale + l1;
						Debug.DrawLine(l1, l2, Color.red);
					}
					if (isRenderBinormal)
					{
						var l2 = meshFilters[i].transform.TransformPoint(b) * scale + l1;
						Debug.DrawLine(l1, l2, Color.green);
					}
					idx++;
				}
			}
		}
#endif
	}
}