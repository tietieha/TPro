using BigWorldRender;
using GEgineRunTime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TEngine;
using UnityEngine;

public class ResRenderData : MonoBehaviour
{
	public List<WorldRegionDetailScriptableObject> worldRegionDetailScriptableObjectList;

	public WorldTerrainUVScaleScriptableObject UVScaleData;
	public WorldRegionTotalScriptableObject worldRegionTotalScriptableObject;

	public List<Mesh> meshList;
	public List<Material> materialList;
	public List<GameObject> effectTypeList;

	public Dictionary<string, Mesh> resMeshDic;
	public Dictionary<string, Material> resMatDic;

	private Mesh _mesh;

	Mesh mesh
	{
		get { return _mesh ? _mesh : _mesh = new Mesh() {hideFlags = HideFlags.HideAndDontSave}; }
	}

	public void InitData()
	{
		if (BigWorldRenderManager.DebugConf.IsLog)
			Log.Info("[BigWorldRender] 初始化 预制体资源");
		resMeshDic = new Dictionary<string, Mesh>(1024);
		resMatDic = new Dictionary<string, Material>(1024);
		if (meshList == null)
		{
			Debug.LogError("meshList == null");
		}

		if (materialList == null)
		{
			Debug.LogError("materialList == null");
		}

		foreach (var mesh in meshList)
		{
			if (mesh == null)
			{
#if GREATEWORLD_DEBUG
				Debug.LogError(" mesh == null ");
#endif
				continue;
			}

			if (resMeshDic.ContainsKey(mesh.name))
			{
				Log.Warning($"[BigWorldRender] {mesh.name} already exist");
				continue;
			}

			resMeshDic.Add(mesh.name, mesh);
		}

		foreach (var mat in materialList)
		{
			if (mat == null)
			{
#if GREATEWORLD_DEBUG
				Debug.LogError(" mat == null ");
#endif
				continue;
			}

			if (resMatDic.ContainsKey(mat.name))
			{
				Log.Warning($"[BigWorldRender] {mat.name} already exist");
				continue;
			}

#if UNITY_EDITOR
			var material = new Material(mat);
			resMatDic.Add(mat.name, material);
#else
			resMatDic.Add(mat.name, mat);
#endif
		}
	}

	public void Exit()
	{
#if UNITY_EDITOR
		var matlist = resMatDic.Keys.ToList();
		foreach (var matname in matlist)
		{
			if (resMatDic.ContainsKey(matname))
			{
				Destroy(resMatDic[matname]);
			}
		}
#endif
	}

	public Mesh GetAssetMesh(string path)
	{
		if (!resMeshDic.ContainsKey(path))
		{
#if GREATEWORLD_DEBUG
			Debug.LogError(path + "不存在Mesh");
#endif
			return mesh;
		}

		// Mesh mesh = resMeshDic[path]; //Resources.Load(path) as Mesh;
		return resMeshDic[path];
	}

	public Material GetAssetMaterial(string path)
	{
		string matPath = Path.GetFileNameWithoutExtension(path);

		if (!resMatDic.ContainsKey(matPath))
		{
#if GREATEWORLD_DEBUG
			Debug.LogError("resMatDic 中不存在    " + path);
#endif
			return null;
		}

		Material material = resMatDic[matPath]; //Resources.Load(path) as Material;
		return material;
	}

	public void ConfigMaterialPassEnable(List<string> filterShader, string pass, bool enable)
	{
		if (string.IsNullOrEmpty(pass))
			return;
		if (filterShader == null || filterShader.Count == 0)
			return;
		foreach (var mat in resMatDic.Values)
		{
			if (mat.shader != null && filterShader.Contains(mat.shader.name))
			{
				mat.SetShaderPassEnabled(pass, enable);
			}
		}
	}
}

public class ResEffectData
{
	public int resEffectId;
	public GameObject effectObj;
}