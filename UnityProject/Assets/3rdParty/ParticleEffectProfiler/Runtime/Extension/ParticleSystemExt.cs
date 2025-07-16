// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-01-30       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    |_ | _|			                *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using UnityEngine;

namespace UWParticleSystemProfiler
{
	public static class ParticleSystemExt
	{
		public static float GetMaxValue(this ParticleSystem.MinMaxCurve minMaxCurve)
		{
			switch (minMaxCurve.mode)
			{
				case ParticleSystemCurveMode.Constant:
					return minMaxCurve.constant;
				case ParticleSystemCurveMode.Curve:
					return minMaxCurve.curve.GetMaxValue();
				case ParticleSystemCurveMode.TwoConstants:
					return minMaxCurve.constantMax;
				case ParticleSystemCurveMode.TwoCurves:
					var ret1 = minMaxCurve.curveMin.GetMaxValue();
					var ret2 = minMaxCurve.curveMax.GetMaxValue();
					return ret1 > ret2 ? ret1 : ret2;
			}
			return -1f;
		}

		public static float GetMinValue(this ParticleSystem.MinMaxCurve minMaxCurve)
		{
			switch (minMaxCurve.mode)
			{
				case ParticleSystemCurveMode.Constant:
					return minMaxCurve.constant;
				case ParticleSystemCurveMode.Curve:
					return minMaxCurve.curve.GetMinValue();
				case ParticleSystemCurveMode.TwoConstants:
					return minMaxCurve.constantMin;
				case ParticleSystemCurveMode.TwoCurves:
					var ret1 = minMaxCurve.curveMin.GetMinValue();
					var ret2 = minMaxCurve.curveMax.GetMinValue();
					return ret1 < ret2 ? ret1 : ret2;
			}
			return -1f;
		}
		
		public static float GetMinValue(this AnimationCurve curve)
		{
			var ret = float.MaxValue;
			var frames = curve.keys;
			for (var i = 0; i < frames.Length; i++)
			{
				var frame = frames[i];
				var value = frame.value;
				if (value < ret)
				{
					ret = value;
				}
			}

			return ret;
		}

		public static float GetMaxValue(this AnimationCurve curve)
		{
			var ret = float.MinValue;
			var frames = curve.keys;
			for (var i = 0; i < frames.Length; i++)
			{
				var frame = frames[i];
				var value = frame.value;
				if (value > ret)
				{
					ret = value;
				}
			}

			return ret;
		}
		
		public static float GetDuration(this ParticleSystem particle, bool allowLoop = false)
		{
			if (!particle.emission.enabled) return 0f;
			if (particle.main.loop && !allowLoop)
			{
				return -1f;
			}
			if (particle.emission.rateOverTime.GetMinValue() <= 0)
			{
				return particle.main.startDelay.GetMaxValue() + particle.main.startLifetime.GetMaxValue();
			}
			else
			{
				return particle.main.startDelay.GetMaxValue() + Mathf.Max(particle.main.duration, particle.main.startLifetime.GetMaxValue());
			}
		}

		public static float GetParticleDuration(this GameObject gameObject, bool includeChildren = true, bool includeInactive = false, bool allowLoop = false)
		{
			if (includeChildren)
			{
				var particles = gameObject.GetComponentsInChildren<ParticleSystem>(includeInactive);
				var duration = -1f;
				for (var i = 0; i < particles.Length; i++)
				{
					var ps = particles[i];
					var time = ps.GetDuration(allowLoop);
					if (time > duration)
					{
						duration = time;
					}
				}

				return duration;
			}
			else
			{
				var ps = gameObject.GetComponent<ParticleSystem>();
				if (ps != null)
				{
					return ps.GetDuration(allowLoop);
				}
				else
				{
					return -1f;
				}
			}
		}

	}
}