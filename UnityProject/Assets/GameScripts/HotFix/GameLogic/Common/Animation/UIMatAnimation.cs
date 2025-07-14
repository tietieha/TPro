using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class UIMatAnimation : MonoBehaviour
{
    [Header("IndividualMaterial Settings")]
    public bool BoolIndividualMaterial = false;

    public string FloatPropName_1;
    public float FloatPropValue_1;
    [Space]
    public string FloatPropName_2;
    public float FloatPropValue_2;
    [Space]
    public string FloatPropName_3;
    public float FloatPropValue_3;
    [Space]
    public string FloatPropName_4;
    public float FloatPropValue_4;
    [Space]
    public string FloatPropName_5;
    public float FloatPropValue_5;
    [Space]
    public string ColorPropName_1;
    public Color ColorPropValue_1;
    [Space]
    public string ColorPropName_2;
    public Color ColorPropValue_2;
    [Space]
    public string ColorPropName_3;
    public Color ColorPropValue_3;
    [Space]
    public string ColorPropName_4;
    public Color ColorPropValue_4;
    [Space]
    public string ColorPropName_5;
    public Color ColorPropValue_5;

    private MaskableGraphic _image;
    private Material _material;
    private void OnEnable()
    {
        _image = GetComponent<MaskableGraphic>();
        if (BoolIndividualMaterial && _material == null)
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                _material = Instantiate(_image.material);
                _image.material = _material;
            }
#else
                _material = Instantiate(_image.material);//new一个 后面好清理
                _image.material = _material;
#endif

        }

    }

    void LateUpdate()
    {
        if (_image == null) return;

        SetUIMatFloat(FloatPropName_1, FloatPropValue_1);
        SetUIMatFloat(FloatPropName_2, FloatPropValue_2);
        SetUIMatFloat(FloatPropName_3, FloatPropValue_3);
        SetUIMatFloat(FloatPropName_4, FloatPropValue_4);
        SetUIMatFloat(FloatPropName_5, FloatPropValue_5);

        SetUIMatColor(ColorPropName_1, ColorPropValue_1);
        SetUIMatColor(ColorPropName_2, ColorPropValue_2);
        SetUIMatColor(ColorPropName_3, ColorPropValue_3);
        SetUIMatColor(ColorPropName_4, ColorPropValue_4);
        SetUIMatColor(ColorPropName_5, ColorPropValue_5);
    }
    public void OnDestroy()
    {
        if (_material)
        {
            Destroy(_material);
        }
    }

    void SetUIMatFloat(string name, float value)
    {
        if (string.IsNullOrEmpty(name) ||
            _image == null ||
            _image.material == null ||
            _image.material.HasProperty(name) == false)
            return;

        _image.material.SetFloat(name, value);
    }

    void SetUIMatColor(string name, Color value)
    {
        if (string.IsNullOrEmpty(name) ||
            _image == null ||
            _image.material == null ||
            _image.material.HasProperty(name) == false)
            return;

        _image.material.SetColor(name, value);
    }

}

