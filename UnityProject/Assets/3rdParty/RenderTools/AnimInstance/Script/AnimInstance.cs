// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-08-24 14:28 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RenderTools.AnimInstance
{
    [ExecuteAlways]
#if ODIN_INSPECTOR
    [HideMonoScript]
#endif
    public class AnimInstance : MonoBehaviour
    {
        public static readonly int S_FPS = 30;

        private static readonly int _animTimeInfoProp = Shader.PropertyToID("_AnimTimeInfo");
        private static readonly int _animInfoProp = Shader.PropertyToID("_AnimInfo");
        private static readonly int _timeProp = Shader.PropertyToID("_Time");

        private static readonly AnimInstanceFrameInfo _defaultInfo =
            new AnimInstanceFrameInfo("default", 0, 0, 1, true, -1);

        private static float s_shaderTimeY
        {
            get
            {
// #if UNITY_EDITOR
// 				return Shader.GetGlobalVector(_timeProp).y;
// #else
                return Time.timeSinceLevelLoad;
// #endif
            }
        }

        [Range(0f, 10f)] private float _animSpeed;

        public float AnimSpeed
        {
            get { return _animSpeed; }
            set
            {
                if (_animSpeed != value)
                {
                    _animSpeed = value;
                    OnCurrentAnimSpeedChanged(_animSpeed);
                }
            }
        }

        private string _curAnimName;

        public string CurAnimName
        {
            get { return _curAnimName; }
        }

        private int _curAnimIndex = -1;

        public int CurAnimIndex
        {
            get { return _curAnimIndex; }
        }

        private int _curFrame;
        public int CurFrame => _curFrame;

        public bool IsPlaying { get; private set; }

        [SerializeField] bool _autoPlay = false;
        [SerializeField] int _defaultAnimIndex = 0;
        [SerializeField] string _defaultAnimName = string.Empty;
        [SerializeField] int _defaultAnimPoseFrame = -1;

        [SerializeField] AnimInstance _attachAnimInstance;

        [SerializeField] Bounds _bounds;

        [SerializeField]
#if ODIN_INSPECTOR
        [ListDrawerSettings(IsReadOnly = false, ShowIndexLabels = true, DraggableItems = false,
            OnBeginListElementGUI = "BeginDrawAnimsListElement", OnEndListElementGUI = "EndDrawAnimsListElement")]
#endif
        string[] _animNames;

        [SerializeField] MeshRenderer[] _renderers;
        [SerializeField] AnimInstanceFrameInfo[] _frameInformations;

        [SerializeField] int _pixelCountPerFrame = 1;

        [SerializeField] List<Transform> boneTransList = new List<Transform>();

        public bool isDebug;

        private bool _inited;

        private float _startTime;
        private AnimInstanceFrameInfo _curAnimFrameInfo;

        private List<MeshFilter> _filters = new List<MeshFilter>();

        private Dictionary<MeshRenderer, MaterialPropertyBlock> _renderMatBlockDic =
            new Dictionary<MeshRenderer, MaterialPropertyBlock>();

        public Dictionary<MeshRenderer, MaterialPropertyBlock> RenderMatBlockDic => _renderMatBlockDic;

        private Dictionary<string, int> _animNameIndex = new Dictionary<string, int>();

        // shader property block data
        private Vector4 _timeBlockData = Vector4.zero;
        private Vector4 _frameBlockData = Vector4.zero;
        private Hash128[] _meshHashs;
        private int _animationCount;
        private float _offsetSeconds;

        private void OnEnable()
        {
            Init();
        }

        private void Update()
        {
            UpdateAnim();
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            if (Application.isPlaying && _meshHashs != null)
            {
                for (int i = 0; i < _meshHashs.Length; i++)
                {
                    MeshCache.Release(_meshHashs[i]);
                }
            }

            EditorApplication.update -= PlayFrameByFrame;
#endif
        }

        public void Init()
        {
            if (_animNames == null)
                return;

            InitAnimIns();

            if (_autoPlay && Application.isPlaying)
            {
                if (!string.IsNullOrEmpty(_defaultAnimName))
                    _defaultAnimIndex = GetAnimIndex(_defaultAnimName);
                if (_defaultAnimPoseFrame > 0)
                    PlayPose(_defaultAnimIndex, _defaultAnimPoseFrame);
                else
                    Play(_defaultAnimIndex);
            }
#if UNITY_EDITOR
            else
            {
                Reset();
            }
#endif
        }

        private void Reset()
        {
            _curFrame = 0;
            _animSpeed = 0;
            _curAnimIndex = -1;
            _curAnimName = string.Empty;
            _curAnimFrameInfo = _defaultInfo;

            OnCurrentAnimationChanged(_curAnimFrameInfo, 0, 0);
        }

        private void InitAnimIns()
        {
            if (_inited)
                return;
            _inited = true;
            _animationCount = _animNames.Length;
            _renderMatBlockDic = new Dictionary<MeshRenderer, MaterialPropertyBlock>(_renderers.Length);
            _animNameIndex = new Dictionary<string, int>(_animNames.Length);

            GetComponentsInChildren<MeshFilter>(true, _filters);
            _meshHashs = new Hash128[_filters.Count];
            for (int i = 0; i < _filters.Count; i++)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    _meshHashs[i] = new Hash128((uint)_filters[i].sharedMesh.GetInstanceID(), 0, 0, 0);
                    // _filters[i].mesh.bounds = _bounds;
                    _filters[i].sharedMesh = MeshCache.Get(_meshHashs[i], _filters[i].sharedMesh, _bounds);
                }
#else
				_filters[i].sharedMesh.bounds = _bounds;
#endif
            }
        }

        private void UpdateAnim()
        {
            if (!_inited)
                return;

            if (_animationCount == 0)
                return;

            if (_curAnimIndex < 0 || _curAnimIndex > _animationCount)
                return;

            if (!IsPlaying || AnimSpeed == 0)
                return;

            float time = s_shaderTimeY;
            int offsetFrame = (int)((time - _startTime + _offsetSeconds) * 30 * AnimSpeed);

            if (offsetFrame > _curAnimFrameInfo.FrameCount)
            {
                if (_curAnimFrameInfo.Loop)
                {
                    offsetFrame %= _curAnimFrameInfo.FrameCount;
                }
                else
                {
                    offsetFrame = _curAnimFrameInfo.FrameCount - 1;
                    // on finish
                    OnAnimFinished(_curAnimFrameInfo);
                    return;
                }
            }

            _curFrame = offsetFrame % _curAnimFrameInfo.FrameCount;

            // if (!Application.isPlaying)
            // 	return;

            if (boneTransList.Count == 0)
                return;
            for (int index = 0; index < boneTransList.Count; index++)
            {
                var boneTrans = boneTransList[index];
                var effectBoneData = _curAnimFrameInfo.effectBoneDatas[index];
                if (_curFrame < 0 || _curFrame >= effectBoneData.AnimBoneDataList.Count)
                    continue;
                var animBoneData = effectBoneData.AnimBoneDataList[_curFrame];
                boneTrans.localPosition = animBoneData.pos;
                boneTrans.localEulerAngles = animBoneData.rot;
            }
        }

        public float GetCurrentTime()
        {
            float time = s_shaderTimeY;
            int offsetFrame = (int)((time - _startTime + _offsetSeconds) * 30 * AnimSpeed);
            float curFrame = (float)(offsetFrame % _curAnimFrameInfo.FrameCount);
            return curFrame * 1000 / 30;
        }

        public AnimInstanceFrameInfo[] GetAnimInstanceFrameInfos()
        {
            return _frameInformations;
        }

        public void PlayDefaultAnim()
        {
            Play(_defaultAnimIndex);
        }

        public int GetAnimFrameCount(string playName)
        {
            if (string.IsNullOrEmpty(playName))
                return 1;
            int index = -1;
            if (!_animNameIndex.TryGetValue(playName, out index))
            {
                index = GetAnimIndex(playName);
                _animNameIndex.Add(playName, index);
            }

            if (index != -1)
                return _frameInformations[index].FrameCount;

            return 1;
        }

        public float GetAnimTime(string playName)
        {
            int frameCount = GetAnimFrameCount(playName);
            return 1f * frameCount / S_FPS;
        }

        public int GetAnimIndex(string animName)
        {
            if (animName.Equals(CurAnimName))
                return _curAnimIndex;

            for (int i = 0; i < _animNames.Length; i++)
            {
                if (_animNames[i].Equals(animName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return i;
                }
            }

            return _defaultAnimIndex;
        }

        public void Play(string animName, float speed = 1, bool inActiveIfNone = false)
        {
            Play(GetAnimIndex(animName), speed, inActiveIfNone);
        }

        public void PlayStartTime(string animName, float speed = 1, float offsetTime = 0, bool inActiveIfNone = false)
        {
            Play(GetAnimIndex(animName), speed, inActiveIfNone, offsetTime);
        }

        public void Play(int index, float speed = 1, bool inActiveIfNone = false, float offsetTime = 0)
        {
            if (index < 0 || _animNames.Length <= index)
            {
                if (inActiveIfNone)
                {
                    _curAnimIndex = -1;
                    _curAnimName = string.Empty;
                    //gameObject.SetActiveEx(false);
                    gameObject.SetActive(false);
                }

                return;
            }

            _offsetSeconds = offsetTime;
            _curFrame = 0;
            _animSpeed = speed;

            if (index != _curAnimIndex)
            {
                gameObject.SetActive(true);
                _curAnimIndex = index;
                _curAnimFrameInfo = new AnimInstanceFrameInfo(_frameInformations[index]);
                _curAnimName = _curAnimFrameInfo.Name;
            }

            OnCurrentAnimationChanged(_curAnimFrameInfo);

            if (_attachAnimInstance != null)
                _attachAnimInstance.Play(CurAnimName, speed, true);
        }

        public void PlayPose(string animName, int frame, bool inActiveIfNone = false)
        {
            PlayPose(GetAnimIndex(animName), frame, inActiveIfNone);
        }

        public void PlayPose(int index, int frame, bool inActiveIfNone = false)
        {
            if (index < 0 || _animNames.Length <= index)
            {
                if (inActiveIfNone)
                {
                    _curAnimIndex = -1;
                    _curAnimName = string.Empty;
                    gameObject.SetActive(false);
                }

                return;
            }

            _curFrame = frame;
            _animSpeed = 0;
            if (index != _curAnimIndex)
            {
                gameObject.SetActive(true);
                _curAnimIndex = index;
                _curAnimFrameInfo = new AnimInstanceFrameInfo(_frameInformations[index]);
                _curAnimName = _curAnimFrameInfo.Name;
            }

            if (frame >= _curAnimFrameInfo.FrameCount)
                frame = 0;

            OnCurrentAnimationChanged(_curAnimFrameInfo, frame);

            if (_attachAnimInstance != null)
                _attachAnimInstance.PlayPose(CurAnimName, frame, true);
        }

        public void Stop()
        {
            AnimSpeed = 0;
        }

        private void OnCurrentAnimationChanged(AnimInstanceFrameInfo frameInformation, int offsetFrame = 0,
            float startTime = -1)
        {
            if (_renderers == null)
                return;
            if (startTime < 0)
                _startTime = s_shaderTimeY;
            for (int i = 0; i < _renderers.Length; i++)
            {
                MeshRenderer mr = _renderers[i];
                if (!_renderMatBlockDic.TryGetValue(mr, out MaterialPropertyBlock block))
                {
                    block = new MaterialPropertyBlock();
                    _renderMatBlockDic.Add(mr, block);
                }

                mr.GetPropertyBlock(block);

                _timeBlockData.x = _startTime;
                _timeBlockData.y = _offsetSeconds;

                _frameBlockData.x = frameInformation.Loop ? 1 : 0;
                _frameBlockData.y = AnimSpeed;
                _frameBlockData.z = frameInformation.StartFrame + offsetFrame;
                _frameBlockData.w = frameInformation.FrameCount;

#if UNITY_EDITOR
                if (isDebug)
                {
                    mr.sharedMaterial.SetVector(_animTimeInfoProp, _timeBlockData);
                    mr.sharedMaterial.SetVector(_animInfoProp, _frameBlockData);
                }
                else
                {
#endif
                    block.SetVector(_animTimeInfoProp, _timeBlockData);
                    block.SetVector(_animInfoProp, _frameBlockData);
                    mr.SetPropertyBlock(block);
#if UNITY_EDITOR
                }
#endif
            }

            IsPlaying = true;
        }

        private void OnCurrentAnimSpeedChanged(float speed)
        {
            for (int i = 0; i < _renderers.Length; i++)
            {
                MeshRenderer mr = _renderers[i];
                if (!_renderMatBlockDic.TryGetValue(mr, out MaterialPropertyBlock block))
                {
                    block = new MaterialPropertyBlock();
                    _renderMatBlockDic.Add(mr, block);
                }

                mr.GetPropertyBlock(block);
                _frameBlockData.y = speed;
                block.SetVector(_animInfoProp, _frameBlockData);
                mr.SetPropertyBlock(block);
            }
        }

        private void OnAnimFinished(AnimInstanceFrameInfo frameInformation)
        {
            if (frameInformation.Loop)
                return;
            if (frameInformation.AutoTransition < 0)
                return;
            Play(frameInformation.AutoTransition, _animSpeed);
        }

        #region 编辑器用 但是不进Editor宏
        /// <summary>
        /// 编辑器setup 数据
        /// </summary>
        /// <param name="selectClipNames"></param>
        /// <param name="animMeshInstanceFrameInfos"></param>
        /// <param name="meshRenderers"></param>
        /// <param name="bounds"></param>
        /// <param name="effectTransList"></param>
        /// <param name="defaultState"></param>
        public void SetUp(List<string> selectClipNames, List<AnimInstanceFrameInfo> animMeshInstanceFrameInfos,
            MeshRenderer[] meshRenderers, Bounds bounds, List<Transform> effectTransList, string defaultState = null)
        {
            _animNames = selectClipNames.ToArray();
            _frameInformations = animMeshInstanceFrameInfos.ToArray();
            _renderers = meshRenderers;
            _bounds = bounds;
            _defaultAnimName = defaultState;
            boneTransList = effectTransList;
            if (!string.IsNullOrEmpty(_defaultAnimName))
            {
                _autoPlay = true;
            }
        }
        #endregion


#if UNITY_EDITOR
        private void BeginDrawAnimsListElement(int index)
        {
            EditorGUILayout.BeginHorizontal();
        }

        private void EndDrawAnimsListElement(int index)
        {
            if (GUILayout.Button("Play"))
            {
                if (!Application.isPlaying)
                {
                    PlayPose(_animNames[index], 0);
                    EditorApplication.update -= PlayFrameByFrame;
                    EditorApplication.update += PlayFrameByFrame;
                }
                else
                {
                    Play(_animNames[index]);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        void PlayFrameByFrame()
        {
            if (!_curAnimFrameInfo.Loop && (_curFrame + 1) >= _curAnimFrameInfo.FrameCount)
            {
                EditorApplication.update -= PlayFrameByFrame;
                return;
            }

            _curFrame = (_curFrame + 1) % _curAnimFrameInfo.FrameCount;
            PlayPose(_curAnimName, _curFrame);
        }

        // private void OnDrawGizmos()
        // {
        // 	var mfs = GetComponentsInChildren<MeshFilter>();
        // 	Bounds bounds = new Bounds();
        // 	bounds.center = transform.position;
        // 	for (int i = 0; i < mfs.Length; i++)
        // 	{
        // 		bounds.Encapsulate(mfs[i].sharedMesh.bounds);
        // 	}
        //
        // 	Gizmos.DrawWireCube(bounds.center, bounds.size);
        //
        // 	var mrs = GetComponentsInChildren<Renderer>();
        // 	Bounds mrbounds = new Bounds();
        // 	mrbounds.center = transform.position;
        // 	for (int i = 0; i < mrs.Length; i++)
        // 	{
        // 		mrbounds.Encapsulate(mrs[i].bounds);
        // 	}
        // 	Gizmos.color = Color.green;
        // 	Gizmos.DrawWireCube(mrbounds.center, mrbounds.size);
        // }
#endif
    }
}