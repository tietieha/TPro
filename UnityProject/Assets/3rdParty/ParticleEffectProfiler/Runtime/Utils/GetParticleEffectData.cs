#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// 处理特效整体相关的数据
/// </summary>
namespace UWParticleSystemProfiler
{
	public class GetParticleEffectData
	{
		public static int ParticleSystemGameObjectDC = 0;

		static int m_MaxDrawCall = 0;

		public static int GetStorageMemorySize(Texture texture)
		{
			return (int) InvokeInternalAPI("UnityEditor.TextureUtil", "GetStorageMemorySize", texture);
		}

		private static object InvokeInternalAPI(string type, string method, params object[] parameters)
		{
			var assembly = typeof(AssetDatabase).Assembly;
			var custom = assembly.GetType(type);
			var methodInfo = custom.GetMethod(method, BindingFlags.Public | BindingFlags.Static);
			return methodInfo != null ? methodInfo.Invoke(null, parameters) : 0;
		}

		public static string GetCullingSupportedString(GameObject go)
		{
			var particleSystems = go.GetComponentsInChildren<ParticleSystem>(true);
			string text = "";
			foreach (ParticleSystem item in particleSystems)
			{
				string str = CheckCulling(item);
				if (!string.IsNullOrEmpty(str))
				{
					text += item.gameObject.name + ":" + str + "\n\n";
				}
			}

			return text;
		}

		static string CheckCulling(ParticleSystem particleSystem)
		{
			string text = "";
			if (particleSystem.collision.enabled)
			{
				text += "\n勾选了 Collision";
			}

			if (particleSystem.emission.enabled)
			{
				if (particleSystem.emission.rateOverDistance.curveMultiplier != 0)
				{
					text += "\nEmission使用了Current(非线性运算)";
				}
			}

			if (particleSystem.externalForces.enabled)
			{
				text += "\n勾选了 External Forces";
			}

			if (particleSystem.forceOverLifetime.enabled)
			{
				if (GetIsRandomized(particleSystem.forceOverLifetime.x)
				    || GetIsRandomized(particleSystem.forceOverLifetime.y)
				    || GetIsRandomized(particleSystem.forceOverLifetime.z)
				    || particleSystem.forceOverLifetime.randomized)
				{
					text += "\nForce Over Lifetime使用了Current(非线性运算)";
				}
			}

			if (particleSystem.inheritVelocity.enabled)
			{
				if (GetIsRandomized(particleSystem.inheritVelocity.curve))
				{
					text += "\nInherit Velocity使用了Current(非线性运算)";
				}
			}

			if (particleSystem.noise.enabled)
			{
				text += "\n勾选了 Noise";
			}

			if (particleSystem.rotationBySpeed.enabled)
			{
				text += "\n勾选了 Rotation By Speed";
			}

			if (particleSystem.rotationOverLifetime.enabled)
			{
				if (GetIsRandomized(particleSystem.rotationOverLifetime.x)
				    || GetIsRandomized(particleSystem.rotationOverLifetime.y)
				    || GetIsRandomized(particleSystem.rotationOverLifetime.z))
				{
					text += "\nRotation Over Lifetime使用了Current(非线性运算)";
				}
			}

			if (particleSystem.shape.enabled)
			{
				ParticleSystemShapeType shapeType = (ParticleSystemShapeType) particleSystem.shape.shapeType;
				switch (shapeType)
				{
					case ParticleSystemShapeType.Cone:
					case ParticleSystemShapeType.ConeVolume:
#if UNITY_2017_1_OR_NEWER
					case ParticleSystemShapeType.Donut:
#endif
					case ParticleSystemShapeType.Circle:
						if (particleSystem.shape.arcMode != ParticleSystemShapeMultiModeValue.Random)
						{
							text += "\nShape的Circle-Arc使用了Random模式";
						}

						break;
					case ParticleSystemShapeType.SingleSidedEdge:
						if (particleSystem.shape.radiusMode != ParticleSystemShapeMultiModeValue.Random)
						{
							text += "\nShape的Edge-Radius使用了Random模式";
						}

						break;
					default:
						break;
				}
			}

			if (particleSystem.subEmitters.enabled)
			{
				text += "\n勾选了 SubEmitters";
			}

			if (particleSystem.trails.enabled)
			{
				text += "\n勾选了 Trails";
			}

			if (particleSystem.trigger.enabled)
			{
				text += "\n勾选了 Trigger";
			}

			if (particleSystem.velocityOverLifetime.enabled)
			{
				if (GetIsRandomized(particleSystem.velocityOverLifetime.x)
				    || GetIsRandomized(particleSystem.velocityOverLifetime.y)
				    || GetIsRandomized(particleSystem.velocityOverLifetime.z))
				{
					text += "\nVelocity Over Lifetime使用了Current(非线性运算)";
				}
			}

			if (particleSystem.limitVelocityOverLifetime.enabled)
			{
				text += "\n勾选了 Limit Velocity Over Lifetime";
			}

			if (particleSystem.main.simulationSpace != ParticleSystemSimulationSpace.Local)
			{
				text += "\nSimulationSpace 不等于 Local";
			}

			if (particleSystem.main.gravityModifierMultiplier != 0)
			{
				text += "\nGravityModifier 不等于0";
			}

			return text;
		}

		static bool GetIsRandomized(ParticleSystem.MinMaxCurve minMaxCurve)
		{
			bool flag = AnimationCurveSupportsProcedural(minMaxCurve.curveMax);

			bool result;
			if (minMaxCurve.mode != ParticleSystemCurveMode.TwoCurves && minMaxCurve.mode != ParticleSystemCurveMode.TwoConstants)
			{
				result = flag;
			}
			else
			{
				bool flag2 = AnimationCurveSupportsProcedural(minMaxCurve.curveMin);
				result = (flag && flag2);
			}

			return result;
		}

		static bool AnimationCurveSupportsProcedural(AnimationCurve curve)
		{
			//switch (AnimationUtility.IsValidPolynomialCurve(curve)) //保护级别，无法访问，靠
			//{
			//    case AnimationUtility.PolynomialValid.Valid:
			//        return true;
			//    case AnimationUtility.PolynomialValid.InvalidPreWrapMode:
			//        break;
			//    case AnimationUtility.PolynomialValid.InvalidPostWrapMode:
			//        break;
			//    case AnimationUtility.PolynomialValid.TooManySegments:
			//        break;
			//}
			return false; //只能默认返回false了
		}

		static string FormatColorValue(int value)
		{
			return string.Format("<color=green>{0}</color>", value);
		}

		static string FormatColorMax(int value, int max)
		{
			if (max > value)
				return string.Format("<color=green>{0}</color>", value);
			else
				return string.Format("<color=red>{0}</color>", value);
		}
	}
}
#endif