using UnityEngine;

using XLua;

[CSharpCallLua]
public delegate void OneObjDelegate(object userdata);

[CSharpCallLua]
public delegate void TwoFloatDelegate(float elapseSeconds, float realElapseSeconds);

[CSharpCallLua]
public delegate void raycastHitCall(RaycastHit dictData);

[CSharpCallLua]
public delegate void CustomCall_String(string _stringMessage);
[CSharpCallLua]
public delegate void CustomCall_Float(float _IntMessage);
[CSharpCallLua]
public delegate void CustomCall_Bool(bool _BoolMessage);
[CSharpCallLua]
public delegate void CustomCall_ProgressAndState(string _state,float _progress);
