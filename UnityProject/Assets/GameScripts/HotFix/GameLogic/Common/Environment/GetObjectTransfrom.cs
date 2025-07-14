using UnityEngine;
using UnityEngine.UI;
//[ExecuteInEditMode]
public class ShipParameter
{
    float bigenPosX = 0.0f;
    float endPosX = 14.8f;
    float bigenHightY = 0.0f;
    float endHightY = 4.7f;
}
//[ExecuteInEditMode]
public class GetObjectTransfrom : MonoBehaviour
{
    [Header("灯相关属性")]
    [Header("灯位置")]
    public GameObject RoleLight;
    //设置全局属性，不会导致每个房间的灯光有差异
    //[Header("灯光颜色")]
    //public Color LightColor = new Color(1.0f, 1.0f, 1.0f, 1.0f).linear;
    [Range(0.0f,1.0f)]
    [Header("灯光半径")]
    public float LightRadius = 0.12f;
    [Header("灯光柔和度")]
    [Range(0.0f, 2.0f)]
    public float LightEdgeSoft = 1.0f;
    [Header("灯光圆形/圆柱")]
    public bool CircleOrCylinder = false;
    // Start is called before the first frame update
    [Header("材质变化--解锁")]
    public bool resetZero = false;
    public float ChangeMatSpeed = 1.0f;
    public GameObject[] ChangeMatObj_B1;
    public GameObject[] ChangeMatObj_B2;

    float tmieADD = 0;
    bool B1 = false;
    bool B2 = false;
    private void OnEnable()
    {
        //Shader.SetGlobalColor("_PointLight01Color", LightColor);
        Shader.SetGlobalFloat("_PointLight01Area", LightRadius);
        Shader.SetGlobalFloat("_PointLight01Soft", LightEdgeSoft);
        Shader.SetGlobalFloat("_CircleOrCylinder", CircleOrCylinder?1:0);

        if (GameObject.Find("Button01") && GameObject.Find("Button02"))
        {
            GameObject.Find("Button01").GetComponent<Button>().onClick.AddListener(() => {
                tmieADD = 0;
                B1 = true;
            });
            GameObject.Find("Button02").GetComponent<Button>().onClick.AddListener(() => {
                tmieADD = 0;
                B2 = true;
            });
        }

    }
    public void ChangeMatObjs(GameObject[] ChangeMatObj)
    {
        for (int i = 0; i < ChangeMatObj.Length; i++)
        {
            Material thisMat = ChangeMatObj[i].GetComponent<Renderer>().material;
            if (thisMat.HasProperty("_GostFogMainSwitch"))
            {
                if (resetZero)
                {
                    thisMat.SetFloat("_GostFogMainSwitch", 0);
                    resetZero = false;
                }
                else
                {
                    tmieADD += Time.deltaTime* ChangeMatSpeed;
                    tmieADD = Mathf.Min(1, tmieADD);
                    thisMat.SetFloat("_GostFogMainSwitch", tmieADD);
                }

            }
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (RoleLight!=null)
        {
#if UNITY_EDITOR
            Shader.SetGlobalFloat("_PointLight01Area", LightRadius);
            Shader.SetGlobalFloat("_PointLight01Soft", LightEdgeSoft);
            Shader.SetGlobalFloat("_CircleOrCylinder", CircleOrCylinder ? 1 : 0);
#endif
            Vector3 lightPosWS = RoleLight.GetComponent<Transform>().position;
            Vector4 lightPosWSV4 = new Vector4(lightPosWS.x, lightPosWS.y, lightPosWS.z,1);
            Shader.SetGlobalVector("_PointLightPos",lightPosWSV4);

            if (B1)
            {
                if (resetZero)
                {
                    ChangeMatObjs(ChangeMatObj_B1);
                    B1 = false;
                }
                else
                {
                    ChangeMatObjs(ChangeMatObj_B1);
                    if (tmieADD == 1)
                    { B1 = false; }
                }
            }
            if (B2)
            {
                if (resetZero)
                {
                    ChangeMatObjs(ChangeMatObj_B2);
                    B2 = false;
                }
                else
                {
                    ChangeMatObjs(ChangeMatObj_B2);
                    if (tmieADD == 1)
                    { B2 = false; }
                }
            }
        }
    }

}
