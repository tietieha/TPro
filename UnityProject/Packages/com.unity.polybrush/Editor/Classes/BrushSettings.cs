using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Polybrush;

namespace UnityEditor.Polybrush
{
    public enum TEX_BLEND_MODE
    {
        REPLACE,
        MIXED,
    }

    /// <summary>
    /// Collection of settings for a sculpting brush.
    /// </summary>
    [CreateAssetMenuAttribute(menuName = "Polybrush/Brush Settings Preset", fileName = "Brush Settings", order = 800)]
    [System.Serializable]
    internal class BrushSettings : PolyAsset, IHasDefault, ICustomSettings
    {

        [SerializeField] internal float brushRadiusMin = 1f;
        [SerializeField] internal float brushRadiusMax = 100f;

        /// The total affected radius of this brush.
        [SerializeField] private float _radius = 10f;

        /// At what point the strength of the brush begins to taper off.
        [SerializeField] float _falloff = .5f;

        /// How may times per-second a mouse click will apply a brush stroke.
        [SerializeField] float _strength = 1f;

        [SerializeField] AnimationCurve _curve = new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(1f, 0f, -3f, -3f)
            );

        public string assetsFolder { get { return "Brush Settings/"; } }

        internal AnimationCurve falloffCurve
        {
            get
            {
                return _curve;
            }
            set
            {
                _curve = allowNonNormalizedFalloff ? value : Util.ClampAnimationKeys(value, 0f, 1f, 1f, 0f);
            }
        }

        /// If true, the falloff curve won't be clamped to keyframes at 0,0 and 1,1.
        public bool allowNonNormalizedFalloff = false;

        /// The total affected radius of this brush.
        internal float radius
        {
            get
            {
                return _radius;
            }

            set
            {
                _radius = Mathf.Clamp(value, brushRadiusMin, brushRadiusMax);
                _radius = Mathf.Round(_radius * 1000f) / 1000f;
            }
        }

        /// <summary>
        /// At what point the strength of the brush begins to taper off.
        /// 0 means the strength tapers from the center of the brush to the edge.
        /// 1 means the strength is 100% all the way through the brush.
        /// .5 means the strength is 100% through 1/2 the radius then tapers to the edge.
        /// </summary>
        internal float falloff
        {
            get
            {
                return _falloff;
            }

            set
            {
                _falloff = Mathf.Clamp(value, 0f, 1f);
                _falloff = Mathf.Round(_falloff * 1000f) / 1000f;
            }
        }

        internal float strength
        {
            get
            {
                return _strength;
            }

            set
            {
                _strength = Mathf.Clamp(value, 0f, 1f);
                _strength = Mathf.Round(_strength * 1000f) / 1000f;
            }
        }

        /// <summary>
        /// Radius value scaled to 0-1.
        /// </summary>
        internal float normalizedRadius
        {
            get
            {
                return (_radius - brushRadiusMin) / (brushRadiusMax - brushRadiusMin);
            }
        }

        ///whether the user is holding control or not
        internal bool isUserHoldingControl { get; set; }
        ///whether the user is holding shift or not
        internal bool isUserHoldingShift { get; set; }

        /// <summary>
        /// Set the object's default values.
        /// </summary>
        public void SetDefaultValues()
        {
            brushRadiusMin = 0.001f;
            brushRadiusMax = 5f;

            radius = 1f;
            falloff = .5f;
            strength = 1f;
        }

        /// <summary>
        /// Deep copy this
        /// </summary>
        /// <returns>A new object with the same values as this</returns>
		internal BrushSettings DeepCopy()
		{
			BrushSettings copy = ScriptableObject.CreateInstance<BrushSettings>();
			this.CopyTo(copy);
			return copy;
		}

        /// <summary>
        /// Copy all properties to target
        /// </summary>
        /// <param name="target"></param>
        internal void CopyTo(BrushSettings target)
		{
			target.name 							= this.name;
			target.brushRadiusMin					= this.brushRadiusMin;
			target.brushRadiusMax					= this.brushRadiusMax;
			target._radius							= this._radius;
			target._falloff							= this._falloff;
			target._strength						= this._strength;
			target._curve							= new AnimationCurve(this._curve.keys);
			target.allowNonNormalizedFalloff		= this.allowNonNormalizedFalloff;
		}

        // 贴图笔刷
        internal bool isTexBrush;
        internal float[] texBrushStrength;
        internal Texture2D[] texBrushTextures;
        internal Texture2D currentTexBrushTexture;
        internal int currentSelectedTexBrush = 0;

        public int texBrushSize => (int) radius;
        public TEX_BLEND_MODE texBrushBlendMode = TEX_BLEND_MODE.REPLACE;
		public bool texBrushEraser = false;

        public float GetStrengthInt(int ix, int iy)
        {
            ix = Mathf.Clamp(ix, 0, this.texBrushSize - 1);
            iy = Mathf.Clamp(iy, 0, this.texBrushSize - 1);
            return this.texBrushStrength[(iy * this.texBrushSize) + ix] * strength;
        }

        public Color GetColor(int channel)
        {
            channel = Mathf.Clamp(channel, 0, 7);
            if (channel == 0)
                return (new Color(1, 0, 0, 0));
            if (channel == 1)
                return (new Color(0, 1, 0, 0));
            if (channel == 2)
                return (new Color(0, 0, 1, 0));
            if (channel == 3)
                return (new Color(0, 0, 0, 1));
            if (channel == 4)
                return (new Color(1, 0, 0, 0));
            if (channel == 5)
                return (new Color(0, 1, 0, 0));
            if (channel == 6)
                return (new Color(0, 0, 1, 0));
            if (channel == 7)
                return (new Color(0, 0, 0, 1));

            return Color.black;
        }

        internal void LoadBrush()
        {
            if(PrepareBrushIcons())
                Load(texBrushTextures[currentSelectedTexBrush]);
        }

        public bool Load(Texture2D brushTex)
        {
            if (brushTex != null)
            {
                bool succed = false;
                string p = AssetDatabase.GetAssetPath(brushTex);
                TextureImporter ti = TextureImporter.GetAtPath(p) as TextureImporter;
                if (ti.textureCompression != TextureImporterCompression.Uncompressed)
                {
                    ti.textureCompression = TextureImporterCompression.Uncompressed;
                    succed = true;
                }

                if (ti.wrapMode != TextureWrapMode.Clamp)
                {
                    ti.wrapMode = TextureWrapMode.Clamp;
                    succed = true;
                }

                if (!ti.isReadable)
                {
                    ti.isReadable = true;
                    succed = true;
                }

                if (succed)
                {
                    AssetDatabase.ImportAsset(p, ImportAssetOptions.ForceSynchronousImport);
                    AssetDatabase.Refresh();
                }
            }

            if (brushTex != null)
            {
                float num = texBrushSize;
                this.texBrushStrength = new float[this.texBrushSize * this.texBrushSize];
                if (this.texBrushSize > 3)
                {
                    for (int j = 0; j < this.texBrushSize; j++)
                    {
                        for (int k = 0; k < this.texBrushSize; k++)
                        {
                            this.texBrushStrength[(j * this.texBrushSize) + k] =
                                brushTex.GetPixelBilinear((k + 0.5f) / num, ((float) j) / num).a;
                        }
                    }
                }
                else
                {
                    for (int m = 0; m < this.texBrushStrength.Length; m++)
                    {
                        this.texBrushStrength[m] = 1f;
                    }
                }

                this.currentTexBrushTexture = brushTex;
                currentTexBrushTexture.wrapMode = TextureWrapMode.Clamp;

                return true;
            }

            this.texBrushStrength = new float[] {1f};
            return false;
        }

        internal bool PrepareBrushIcons()
        {
            if (texBrushTextures == null) LoadBrushIcons();

            return texBrushTextures != null;
        }

        internal void LoadBrushIcons()
        {
            List<Texture2D> list = new List<Texture2D>();
            int num = 0;
            Texture2D texture = null;
            do
            {
                texture = IconUtility.GetBrushIcon(string.Format("Brushes/brush_{0:00}", num));
                if (texture != null)
                {
                    list.Add(texture);
                }

                num++;
            } while (texture != null);

            texBrushTextures = list.ToArray();
        }

    }
}
