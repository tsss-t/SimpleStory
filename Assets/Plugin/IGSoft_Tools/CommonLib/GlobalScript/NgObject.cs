// ----------------------------------------------------------------------------------
//
// FXMaker
// Created by ismoon - 2012 - ismoonto@gmail.com
//
// ----------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public	class NgObject
{
	public static void SetActive(GameObject target, bool bActive)
	{
#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
		target.SetActive(bActive);
#else
		target.SetActive( bActive);
#endif
	}

	public static void SetActiveRecursively(GameObject target, bool bActive)
	{
#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
		for (int n = target.transform.childCount-1; 0 <= n; n--)
			if (n < target.transform.childCount)
				SetActiveRecursively(target.transform.GetChild(n).gameObject, bActive);
		target.SetActive(bActive);
#else
		target.SetActive(bActive);
#endif
	}

	public static bool IsActive(GameObject target)
	{
#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
		return (target.activeInHierarchy && target.activeSelf);
#else
		return target.activeSelf;
#endif
	}

	public static GameObject CreateGameObject(GameObject prefabObj)
	{
		GameObject newChild = (GameObject)NcSafeTool.SafeInstantiate(prefabObj);
		return newChild;
	}

	// °ÔÀÓ¿ÀºE§Æ®¸¦ »ý¼ºÇÑ´Ù.
	public static GameObject CreateGameObject(GameObject parent, string name)
	{
		return CreateGameObject(parent.transform, name);
	}
	public static GameObject CreateGameObject(MonoBehaviour parent, string name)
	{
		return CreateGameObject(parent.transform, name);
	}
	public static GameObject CreateGameObject(Transform parent, string name)
	{
		GameObject newChild = new GameObject(name);
		if (parent != null)
		{	// ¿øº» transformÀ» À¯ÁE½ÃÄÑÁÖÀÚ
 			NcTransformTool	trans	= new NcTransformTool(newChild.transform);
			newChild.transform.parent = parent;
 			trans.CopyToLocalTransform(newChild.transform);
		}
		return newChild;
	}

	// Å¬·Ð °ÔÀÓ¿ÀºE§Æ®¸¦ »ý¼ºÇÑ´Ù.
	public static GameObject CreateGameObject(GameObject parent, GameObject prefabObj)
	{
		return CreateGameObject(parent.transform, prefabObj);
	}
	public static GameObject CreateGameObject(MonoBehaviour parent, GameObject prefabObj)
	{
		return CreateGameObject(parent.transform, prefabObj);
	}
	public static GameObject CreateGameObject(Transform parent, GameObject prefabObj)
	{
		GameObject newChild = (GameObject)NcSafeTool.SafeInstantiate(prefabObj);
		if (parent != null)
		{	// ¿øº» transformÀ» À¯ÁE½ÃÄÑÁÖÀÚ
 			NcTransformTool	trans	= new NcTransformTool(newChild.transform);
			newChild.transform.parent = parent;
 			trans.CopyToLocalTransform(newChild.transform);
		}
		return newChild;
	}

	// Å¬·Ð °ÔÀÓ¿ÀºE§Æ®¸¦ »ý¼ºÇÑ´Ù.
	public static GameObject CreateGameObject(GameObject parent, GameObject prefabObj, Vector3 pos, Quaternion rot)
	{
		return CreateGameObject(parent.transform, prefabObj, pos, rot);
	}
	public static GameObject CreateGameObject(MonoBehaviour parent, GameObject prefabObj, Vector3 pos, Quaternion rot)
	{
		return CreateGameObject(parent.transform, prefabObj, pos, rot);
	}
	public static GameObject CreateGameObject(Transform parent, GameObject prefabObj, Vector3 pos, Quaternion rot)
	{
		GameObject newChild;

		if (NcSafeTool.IsSafe() == false)
			return null;
		newChild = (GameObject)NcSafeTool.SafeInstantiate(prefabObj, pos, rot);
		if (parent != null)
		{	// ¿øº» transformÀ» À¯ÁE½ÃÄÑÁÖÀÚ
 			NcTransformTool	trans	= new NcTransformTool(newChild.transform);
			newChild.transform.parent = parent;
 			trans.CopyToLocalTransform(newChild.transform);
		}
		return newChild;
	}

	public static void HideAllChildObject(GameObject parent)
	{
		for (int n = parent.transform.childCount-1; 0 <= n; n--)
		{
			if (n < parent.transform.childCount)
				NgObject.IsActive(parent.transform.GetChild(n).gameObject);
		}
	}

	public static void RemoveAllChildObject(GameObject parent, bool bImmediate)
	{
		for (int n = parent.transform.childCount-1; 0 <= n; n--)
		{
			if (n < parent.transform.childCount)
			{
				Transform	obj = parent.transform.GetChild(n);
				if (bImmediate)
					Object.DestroyImmediate(obj.gameObject);
				else Object.Destroy(obj.gameObject);
			}
// 			obj.parent = null;
// 			Object.Destroy(obj.gameObject);
		}
	}

	// ÄÄÆ÷³ÍÆ®¸¦ »ý¼ºÇÑ´Ù.(ÀÖÀ¸¸E½ºÅµ)
	public static Component CreateComponent(Transform parent, System.Type type)
	{
		return CreateComponent(parent.gameObject, type);
	}
	public static Component CreateComponent(MonoBehaviour parent, System.Type type)
	{
		return CreateComponent(parent.gameObject, type);
	}
	public static Component CreateComponent(GameObject parent, System.Type type)
	{
		Component com = parent.GetComponent(type);
		if (com != null)
			return com;
		else
		{
			com = parent.AddComponent(type);
			return com;
		}
	}

	// Â÷ÀÏµåÀÇ ¸ðµE¿ÀºE§Æ®¸¦ °Ë»öÇÑ´Ù
	public static Transform FindTransform(Transform rootTrans, string name)
	{
		Transform dt = rootTrans.Find(name);
		if (dt)
			return dt;
		else
		{
			foreach (Transform child in rootTrans)
			{
				dt = FindTransform(child, name);
				if (dt)
					return dt;
			}
		}
		return null;
	}

	public static bool FindTransform(Transform rootTrans, Transform findTrans)
	{
		if (rootTrans == findTrans)
			return true;
		else
		{
			foreach (Transform child in rootTrans)
				if (FindTransform(child, findTrans))
					return true;
		}
		return false;
	}

	// Â÷ÀÏµåÀÇ ¸ðµEMesh materialÀ» º¯°æÇÑ´Ù
	public static Material ChangeMeshMaterial(Transform t, Material newMat)
	{
		MeshRenderer[]	ren = t.GetComponentsInChildren<MeshRenderer>(true);
		Material		reMat = null;
		for (int n = 0; n < ren.Length; n++)
		{
			reMat = ren[n].material;
			ren[n].material = newMat;
		}
		return reMat;
	}

	// Â÷ÀÏµåÀÇ ¸ðµESkinnedMesh materialÀÇ color¸¦ º¯°æÇÑ´Ù
	public static void ChangeSkinnedMeshColor(Transform t, Color color)
	{
		SkinnedMeshRenderer[]	ren = t.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		for (int n = 0; n < ren.Length; n++)
			ren[n].material.color = color;
	}

	// Â÷ÀÏµåÀÇ ¸ðµEMesh materialÀÇ color¸¦ º¯°æÇÑ´Ù
	public static void ChangeMeshColor(Transform t, Color color)
	{
		MeshRenderer[]	ren = t.GetComponentsInChildren<MeshRenderer>(true);
		for (int n = 0; n < ren.Length; n++)
			ren[n].material.color = color;
	}

	// Â÷ÀÏµåÀÇ ¸ðµESkinnedMesh materialÀÇ alpha¸¦ º¯°æÇÑ´Ù
	public static void ChangeSkinnedMeshAlpha(Transform t, float alpha)
	{
		SkinnedMeshRenderer[]	ren = t.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		for (int n = 0; n < ren.Length; n++)
		{
			Color al = ren[n].material.color;
			al.a = alpha;
			ren[n].material.color = al;
		}
	}

	// Â÷ÀÏµåÀÇ ¸ðµEMesh materialÀÇ Alpha¸¦ º¯°æÇÑ´Ù
	public static void ChangeMeshAlpha(Transform t, float alpha)
	{
		MeshRenderer[]	ren = t.GetComponentsInChildren<MeshRenderer>(true);
		for (int n = 0; n < ren.Length; n++)
		{
			Color al = ren[n].material.color;
			al.a = alpha;
			ren[n].material.color = al;
		}
	}

	// ¼­ºE÷ÇÔ Â÷ÀÏµE¸®½ºÆ®¸¦ ±¸¼ºÇØ¼­ ¸®ÅÏÇÑ´Ù.
	public static Transform[] GetChilds(Transform parentObj)
	{
		Transform[] arr = parentObj.GetComponentsInChildren<Transform>(true);
		Transform[] arr2 = new Transform[arr.Length - 1];
		for (int i = 1; i < arr.Length; i++)
		{
			arr2[i - 1] = arr[i];
		}
		return arr2;
	}

	// ¼­ºE±ÅÃ Â÷ÀÏµE¸®½ºÆ®¸¦ nameÀ¸·Î ¼ÒÆÃ±¸¼ºÇØ¼­ ¸®ÅÏÇÑ´Ù.
	public static SortedList GetChildsSortList(Transform parentObj, bool bSub, bool bOnlyActive)
	{
		SortedList	sortList = new SortedList();

		if (bSub)
		{
			Transform[] arr	= parentObj.GetComponentsInChildren<Transform>(bOnlyActive);

			for (int i = 1; i < arr.Length; i++)
			{
// 				Debug.Log(arr[i]);
//	 			Debug.Log(arr[i].name);
				sortList.Add(arr[i].name, arr[i]);
			}
		} else {
			for (int i = 0; i < parentObj.childCount; i++)
			{
				Transform trans = parentObj.GetChild(i);
				sortList.Add(trans.name, trans);
			}
		}
		return sortList;
	}

	// ¼­ºE÷ÇÔ tag°¡ ÀÖ´Â Ã¹ obj ¸®ÅÏÇÑ´Ù.
	public static GameObject FindObjectWithTag(GameObject rootObj, string findTag)
	{
		if (rootObj == null)
			return null;
		if (rootObj.tag == findTag)
			return rootObj;

		for (int n = 0; n < rootObj.transform.childCount; n++)
		{
			GameObject	find = FindObjectWithTag(rootObj.transform.GetChild(n).gameObject, findTag);
			if (find != null)
				return find;
		}
		return null;
	}

	// ¼­ºE÷ÇÔ layer°¡ ÀÖ´Â Ã¹ obj ¸®ÅÏÇÑ´Ù.
	public static GameObject FindObjectWithLayer(GameObject rootObj, int nFindLayer)
	{
		if (rootObj == null)
			return null;
		if (rootObj.layer == nFindLayer)
			return rootObj;

		for (int n = 0; n < rootObj.transform.childCount; n++)
		{
			GameObject	find = FindObjectWithLayer(rootObj.transform.GetChild(n).gameObject, nFindLayer);
			if (find != null)
				return find;
		}
		return null;
	}

	// tag¸úÜ» ¸ðµÎ ¹Ù²Û´Ù.
	public static void ChangeLayerWithChild(GameObject rootObj, int nLayer)
	{
		if (rootObj == null)
			return;
		rootObj.layer = nLayer;
		for (int n = 0; n < rootObj.transform.childCount; n++)
			ChangeLayerWithChild(rootObj.transform.GetChild(n).gameObject, nLayer);
	}

	// mesh count ================================================================================
	public static void GetMeshInfo(GameObject selObj, bool bInChildren, out int nVertices, out int nTriangles, out int nMeshCount)
	{
		Component[] skinnedMeshes;
		Component[] meshFilters;

		nVertices	= 0;
		nTriangles	= 0;
		nMeshCount	= 0;

		if (selObj == null)
			return;

		if (bInChildren)
		{
			skinnedMeshes = selObj.GetComponentsInChildren(typeof(SkinnedMeshRenderer));
			meshFilters = selObj.GetComponentsInChildren(typeof(MeshFilter));
		} else
		{
			skinnedMeshes = selObj.GetComponents(typeof(SkinnedMeshRenderer));
			meshFilters = selObj.GetComponents(typeof(MeshFilter));
		}

		ArrayList totalMeshes = new ArrayList(meshFilters.Length + skinnedMeshes.Length);

		for (int meshFiltersIndex = 0; meshFiltersIndex < meshFilters.Length; meshFiltersIndex++)
		{
			MeshFilter meshFilter = (MeshFilter)meshFilters[meshFiltersIndex];
			totalMeshes.Add(meshFilter.sharedMesh);
		}

		for (int skinnedMeshIndex = 0; skinnedMeshIndex < skinnedMeshes.Length; skinnedMeshIndex++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)skinnedMeshes[skinnedMeshIndex];
			totalMeshes.Add(skinnedMeshRenderer.sharedMesh);
		}

		for (int i = 0; i < totalMeshes.Count; i++)
		{
			Mesh mesh = (Mesh)totalMeshes[i];
			if (mesh != null)
			{
				nVertices += mesh.vertexCount;
				nTriangles += mesh.triangles.Length / 3;
				nMeshCount++;
			}
		}
	}

	public static void GetMeshInfo(Mesh mesh, out int nVertices, out int nTriangles, out int nMeshCount)
	{
		nVertices	= 0;
		nTriangles	= 0;
		nMeshCount	= 0;

		if (mesh == null)
			return;

		if (mesh != null)
		{
			nVertices += mesh.vertexCount;
			nTriangles += mesh.triangles.Length / 3;
			nMeshCount++;
		}
	}
}
