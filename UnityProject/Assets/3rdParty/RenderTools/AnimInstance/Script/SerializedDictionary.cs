﻿// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-08-08 18:51 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RenderTools.AnimInstance
{
	[Serializable]
	public class SerializedDictionary<TKey, TValue> : ISerializationCallbackReceiver
	{
		[SerializeField] List<TKey> m_Keys = new List<TKey>();
		[SerializeField] List<TValue> m_Values = new List<TValue>();

		readonly Dictionary<TKey, TValue> m_Dictionary = new Dictionary<TKey, TValue>();

		public Dictionary<TKey, TValue> dictionary
		{
			get { return m_Dictionary; }
		}

		public void OnBeforeSerialize()
		{
			m_Keys.Clear();
			m_Values.Clear();

			foreach (var keyPair in m_Dictionary)
			{
				m_Keys.Add(keyPair.Key);
				m_Values.Add(keyPair.Value);
			}
		}

		public void OnAfterDeserialize()
		{
			m_Dictionary.Clear();

			for (int i = 0; i < m_Keys.Count; ++i)
				m_Dictionary.Add(m_Keys[i], m_Values[i]);
		}
	}
}