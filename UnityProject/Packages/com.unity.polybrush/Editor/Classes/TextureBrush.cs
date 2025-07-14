#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace UnityEditor.Polybrush
{
	public class PaintTrack
	{
		class TrackInfo
		{
			public Color[] texInfo;
			public int idx = -1;
		}

		private int mMaxCount = 100;
		private int mCurCount = 0;
		private int mIdx = 0;
		private TrackInfo[] mTracks = new TrackInfo[100];

		public void AddTrack(Texture2D texInfo)
		{
			Color[] color = texInfo.GetPixels(0, 0, texInfo.width, texInfo.height, 0);
			if (mCurCount < mMaxCount)
			{
				TrackInfo track = new TrackInfo();

				track.texInfo = color;
				track.idx = mIdx++;

				mTracks[mCurCount] = track;
				mCurCount++;
			}
			else
			{
				TrackInfo oldTrack = mTracks[0];
				foreach (TrackInfo track in mTracks)
				{
					if (oldTrack.idx == -1)
						break;

					if (oldTrack.idx > track.idx)
					{
						oldTrack = track;
					}
				}

				oldTrack.texInfo = color;
				oldTrack.idx = mIdx++;
			}
		}

		public Color[] ReturnBack()
		{
			if (mCurCount == 0)
				return null;
			int idx = mTracks[0].idx;
			foreach (TrackInfo track in mTracks)
			{
				if (track == null)
					break;
				if (idx < track.idx)
				{
					idx = track.idx;
				}
			}

			if (idx > -1)
			{
				for (int i = 0; i < mTracks.Length; ++i)
				{
					if (mTracks[i] != null && idx == mTracks[i].idx)
					{
						mTracks[i].idx = -1;
						return mTracks[i].texInfo;
					}
				}
			}

			return null;
		}
	}

	public class TextureBrush
	{
		// Fields
		internal const int kMinBrushSize = 3;
		private Texture2D m_Brush;
		private int m_Size;
		private float[] m_Strength;
		private bool m_Hole = false;
		private PaintTrack m_Track = new PaintTrack();
		private PaintTrack m_Track6Tex = new PaintTrack();

		public float GetStrengthInt(int ix, int iy)
		{
			ix = Mathf.Clamp(ix, 0, this.m_Size - 1);
			iy = Mathf.Clamp(iy, 0, this.m_Size - 1);
			return this.m_Strength[(iy * this.m_Size) + ix];
		}

		public Color GetColor(int i)
		{
			if (m_Hole)
				return new Color(0, 0, 0, 0);

			i = Mathf.Clamp(i, 0, 7);
			if (i == 0)
				return (new Color(1, 0, 0, 0));
			if (i == 1)
				return (new Color(0, 1, 0, 0));
			if (i == 2)
				return (new Color(0, 0, 1, 0));
			if (i == 3)
				return (new Color(0, 0, 0, 1));
			if (i == 4)
				return (new Color(1, 0, 0, 0));
			if (i == 5)
				return (new Color(0, 1, 0, 0));
			if (i == 6)
				return (new Color(0, 0, 1, 0));
			if (i == 7)
				return (new Color(0, 0, 0, 1));

			return Color.black;
		}

		public Color[] Get2Color(int i)
		{
			i = Mathf.Clamp(i, 0, 7);
			if (i == 0)
				return new Color[] {new Color(1, 0, 0, 0), new Color(0, 0, 0, 0)};
			if (i == 1)
				return new Color[] {new Color(0, 1, 0, 0), new Color(0, 0, 0, 0)};
			if (i == 2)
				return new Color[] {new Color(0, 0, 1, 0), new Color(0, 0, 0, 0)};
			if (i == 3)
				return new Color[] {new Color(0, 0, 0, 1), new Color(0, 0, 0, 0)};
			if (i == 4)
				return new Color[] {new Color(0, 0, 0, 0), new Color(1, 0, 0, 0)};
			if (i == 5)
				return new Color[] {new Color(0, 0, 0, 0), new Color(0, 1, 0, 0)};
			if (i == 6)
				return new Color[] {new Color(0, 0, 0, 0), new Color(0, 0, 1, 0)};
			if (i == 7)
				return new Color[] {new Color(0, 0, 0, 0), new Color(0, 0, 0, 1)};

			return new Color[] {new Color(0, 0, 0, 0), new Color(0, 0, 0, 0)};
		}

		public Color GetColorDec(int i)
		{
			i = Mathf.Clamp(i, 0, 7);
			if (i == 0)
				return (new Color(0, 1, 1, 1));
			if (i == 1)
				return (new Color(1, 0, 1, 1));
			if (i == 2)
				return (new Color(1, 1, 0, 1));
			if (i == 3)
				return (new Color(1, 1, 1, 0));
			if (i == 4)
				return (new Color(0, 1, 1, 1));
			if (i == 5)
				return (new Color(1, 0, 1, 1));
			if (i == 6)
				return (new Color(1, 1, 0, 1));
			if (i == 7)
				return (new Color(1, 1, 1, 0));

			return Color.black;
		}

		public bool Load(Texture2D brushTex, int size)
		{
			if (((this.m_Brush == brushTex) && (size == this.m_Size)) && (this.m_Strength != null))
			{
				return true;
			}

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
				float num = size;
				this.m_Size = size;
				this.m_Strength = new float[this.m_Size * this.m_Size];
				if (this.m_Size > 3)
				{
					for (int j = 0; j < this.m_Size; j++)
					{
						for (int k = 0; k < this.m_Size; k++)
						{
							this.m_Strength[(j * this.m_Size) + k] =
								brushTex.GetPixelBilinear((k + 0.5f) / num, ((float) j) / num).a;
						}
					}
				}
				else
				{
					for (int m = 0; m < this.m_Strength.Length; m++)
					{
						this.m_Strength[m] = 1f;
					}
				}

				this.m_Brush = brushTex;
				m_Brush.wrapMode = TextureWrapMode.Clamp;

				return true;
			}

			this.m_Strength = new float[] {1f};
			this.m_Size = 1;
			return false;
		}

		public bool Hole
		{
			set { m_Hole = value; }
			get { return m_Hole; }
		}

		public void AddTrack(Texture2D tex)
		{
			m_Track.AddTrack(tex);
		}

		public Color[] ReturnBack()
		{
			return m_Track.ReturnBack();
		}

		public void AddTrack6Tex(Texture2D tex)
		{
			m_Track6Tex.AddTrack(tex);
		}

		public Color[] ReturnBack6Tex()
		{
			return m_Track6Tex.ReturnBack();
		}

	}
}
#endif
