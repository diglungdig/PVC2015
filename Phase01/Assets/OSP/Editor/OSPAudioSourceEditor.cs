/************************************************************************************

Filename    :   OSPAudioSourceEditor.cs
Content     :   OSP audio source interface. 
				This script adds editor functionality to OSPAudioSource script.
Created     :   November 19, 2014
Authors     :   Peter Giokaris

Copyright   :   Copyright 2014 Oculus VR, Inc. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.1 (the "License"); 
you may not use the Oculus VR Rift SDK except in compliance with the License, 
which is provided at the time of installation or download, or which 
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculusvr.com/licenses/LICENSE-3.1 

Unless required by applicable law or agreed to in writing, the Oculus VR SDK 
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
************************************************************************************/
#define CUSTOM_LAYOUT

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(OSPAudioSource))]

public class OSPAudioSourceEditor : Editor
{
	// target component
	private OSPAudioSource m_Component;
	
	// highlight color
	// Color HColor = Color.green;
	
	// Pop-up box for frequency hints
	string[] freqHints = new[] {"None", "Wide", "Narrow"};
	
	// OnEnable
	void OnEnable()
	{
		m_Component = (OSPAudioSource)target;
	}
	
	// OnDestroy
	void OnDestroy()
	{
	}
	
	// OnInspectorGUI
	public override void OnInspectorGUI()
	{
		GUI.color = Color.white;
		Undo.RecordObject(m_Component, "OSPAudioSource");
		
		{
			#if CUSTOM_LAYOUT
			Separator();
            m_Component.Bypass      = EditorGUILayout.Toggle("Bypass", m_Component.Bypass);
			m_Component.PlayOnAwake = EditorGUILayout.Toggle("Play On Awake", m_Component.PlayOnAwake);
			m_Component.UseSimple   = EditorGUILayout.Toggle("Use Simple Override", m_Component.UseSimple);
			
			Separator();
			m_Component.Priority = EditorGUILayout.IntField("Priority (0 - 10)", m_Component.Priority); 
			m_Component.FrequencyHint = EditorGUILayout.Popup("Frequency Hint",   m_Component.FrequencyHint, freqHints); 
			
			Separator();
			
			/*
			// Reference GUI Layout fields
			m_Component.VerticalFOV         = EditorGUILayout.FloatField("Vertical FOV", m_Component.VerticalFOV);
			m_Component.NeckPosition 		= EditorGUILayout.Vector3Field("Neck Position", m_Component.NeckPosition);
			m_Component.UsePlayerEyeHeight  = EditorGUILayout.Toggle ("Use Player Eye Height", m_Component.UsePlayerEeHeight);
			m_Component.FollowOrientation   = EditorGUILayout.ObjectField("Follow Orientation", 
																		m_Component.FollowOrientation,
																		typeof(Transform), true) as Transform;
			m_Component.BackgroundColor 	= EditorGUILayout.ColorField("Background Color", m_Component.BackgroundColor);
			OVREditorGUIUtility.Separator();
*/
			
			#else			 
			DrawDefaultInspector ();
			#endif
		}
		
		if (GUI.changed)
		{
			EditorUtility.SetDirty(m_Component);
		}
	}	
	
	// Utilities, move out of here (or copy over to other editor script)
	
	// Separator
	void Separator()
	{
		GUI.color = new Color(1, 1, 1, 0.25f);
		GUILayout.Box("", "HorizontalSlider", GUILayout.Height(16));
		GUI.color = Color.white;
	}
	
	// Label
	void Label(string label)
	{
		EditorGUILayout.LabelField (label);
	}
	
}

