/************************************************************************************
Filename    :   OSPAudioSource.cs
Content     :   Interface into the Oculus Spatializer Plugin (from audio source)
Created     :   Novemnber 11, 2014
Authors     :   Peter Giokaris
opyright   :   Copyright 2014 Oculus VR, Inc. All Rights reserved.

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
//#define DEBUG_AudioSource

using UnityEngine;

/// <summary>
/// OSP audio source.
/// This component should be added to a scene with an audio source
/// </summary>
public class OSPAudioSource : MonoBehaviour 
{
	// Public members
	[SerializeField]
	private bool bypass = false;
	public  bool Bypass
	{
		get{return bypass;}
		set{bypass = value;}
	}

	[SerializeField]
	private bool playOnAwake = false;
	public  bool PlayOnAwake
	{
		get{return playOnAwake;}
		set{playOnAwake = value;}
	}

	[SerializeField]
	private int priority = 0;
	public  int Priority
	{
		get{return priority;}
		set{priority = value;
			if(priority < 0)  priority = 0;
			if(priority > 10) priority = 10;}
	}

	[SerializeField]
	private int frequencyHint = 0;
	public  int FrequencyHint
	{
		get{return frequencyHint;}
		set{frequencyHint = value;
			if(frequencyHint < 0) frequencyHint = 0;
			if(frequencyHint > 2) frequencyHint = 2;}
	}

	[SerializeField]
	private bool useSimple = false;
	public  bool UseSimple
	{
		get{return useSimple; }
		set{useSimple = value;}
	}

	// Private members
	AudioListener audioListener = null;

	private int   context   = sNoContext;
	private bool  isPlaying = false;
	private float panLevel  = 0.0f;
	private float spread    = 0.0f;
	// We must account for the early reflection tail, don't but the 
	// context back until it's been rendered
	private bool  drain     = false;
	private float drainTime = 0.0f;
	// We will set the relative position in the Update function
	// Capture the previous so that we can interpolate into the
	// latest position in AudioFilterRead
	private Vector3 relPos      = new Vector3(0,0,0);
	private Vector3 relVel      = new Vector3(0,0,0);
	private float   relPosTime  = 0.0f;

	// Consts
	private const int   sNoContext   = -1;
	private const float sSetPanLevel = 1.0f;
	private const float sSetSpread   = 180.0f;

#if DEBUG_AudioSource
	// Debug
	private const bool  debugOn		 = false; // TURN THIS OFF FOR NO VERBOSE
	private float dTimeMainLoop      = 0.0f;
	private int   audioFrames		 = 100;
	private float PrevAudioTime 	 = 0.0f;
#endif

	//* * * * * * * * * * * * *
	// MonoBehaviour functions
	//* * * * * * * * * * * * *
	void Awake()
	{
		if(!AudioSourceExists())return;

		// Set this in Start; we need to ensure spatializer has been initialized
		// It's MUCH better to set playOnAwake in the audio source script, will avoid
		// having the sound play then stop and then play again)
		if (GetComponent<AudioSource>().playOnAwake == true || playOnAwake == true)
		{
			GetComponent<AudioSource>().Stop();
		}
	}

	// Use this for initialization
	void Start() 
	{		
		if(!AudioSourceExists())
		{
			Debug.LogWarning ("Start - Warning: No AudioSource on GameObject");
			return;
		}

		// Start a play on awake sound here, after spatializer has been initialized
		// Only do this IF it didn't happen in Awake
		if (((GetComponent<AudioSource>().playOnAwake == true) || playOnAwake == true) && 
		     (isPlaying == false))
			Play();
	}
	
	// Update is called once per frame
	void Update() 
	{
		if(!AudioSourceExists())return;

		// If sound is playing on it's own and dies off, we need to
		// Reset
		if (isPlaying == true) 
		{
			// We will go back and forth between spatial processing
			// and native 2D panning
			if((Bypass == true) || (OSPManager.GetBypass() == true))
			{
#if (!UNITY_5_0)
				GetComponent<AudioSource>().spatialBlend = panLevel;
#else
				GetComponent<AudioSource>().spatialBlend = panLevel;
#endif
				GetComponent<AudioSource>().spread   = spread;
			}
			else
			{
#if (!UNITY_5_0)
				GetComponent<AudioSource>().spatialBlend = sSetPanLevel;
#else
				GetComponent<AudioSource>().spatialBlend = sSetPanLevel;
#endif
				GetComponent<AudioSource>().spread   = sSetSpread;
			}

			// Update FrequencyHints
			// Optimize: Do not do this if context doesn't exist
			if(context != sNoContext)
			{
				OSPManager.SetFrequencyHint(context, frequencyHint);
			}

			// return the context when the sound has finished playing
			if((GetComponent<AudioSource>().time >= GetComponent<AudioSource>().clip.length) && (GetComponent<AudioSource>().loop == false))
			{
				// One last pass to allow for the tail portion of the sound
				// to finish
				drainTime = OSPManager.GetDrainTime(context);
				drain     = true;
			}
			else
			{	
				// We must set all positional properties here, not in  
				// OnAudioFilterRead. We might consider a better approach
				// to interpolate the current location for better localization,
				// should the resolution of setting it here sound jittery due
				// to thread frequency mismatch.
				SetRelativeSoundPos(false);

#if DEBUG_AudioSource
				// Time main thread and audio thread
				if(debugOn)
				{
					// Get audio frequency
					if(audioFrames == 0)
					{
						float ct = 1.0f / (GetTime(false) - dTimeMainLoop);
						Debug.LogWarning (System.String.Format ("U: {0:F0}", ct));
						ct = 100.0f / (GetTime(true) - PrevAudioTime);
						Debug.LogWarning (System.String.Format ("A: {0:F0}", ct));
						audioFrames = 100;
						PrevAudioTime = (float)GetTime(true);
					}

					dTimeMainLoop = (float)GetTime(false);
				}
#endif
			}

			if(drain == true)
			{
				// Keep playing until we safely drain out the early reflections
				drainTime -= Time.deltaTime;
				if(drainTime < 0.0f)
				{
					drain = false;

					lock (this) isPlaying = false;
					// Stop will both stop audio from playing (keeping OnAudioFilterRead from 
					// running with a held audio source) as well as release the spatialization
					// resources via Release()
					Stop(); 
				}
			}
		}
	}

	// OnDestroy
	void OnDestroy()
	{
		if(!AudioSourceExists())return;

		lock (this) isPlaying = false;
		Release();
	}
		
	//* * * * * * * * * * * * *
	// Private functions
	//* * * * * * * * * * * * *

	// Aquire a sound context and cache variables
	private void Aquire()
	{
		if(!AudioSourceExists())return;

		// Cache pan and spread
#if (!UNITY_5_0)
		panLevel = GetComponent<AudioSource>().spatialBlend;
#else
		panLevel = GetComponent<AudioSource>().spatialBlend;
#endif
		spread = GetComponent<AudioSource>().spread;
		
		// override to use simple spatializer
		bool prevUseSimple = OSPManager.GetUseSimple();

		if(useSimple == true) 
			OSPManager.SetUseSimple (useSimple); 
	
		// Reserve a context in OSP that will be used for spatializing sound
		// (Don't grab a new one if is already has a context attached to it)
		if(context == sNoContext)
			context = OSPManager.AquireContext ();

		OSPManager.SetUseSimple (prevUseSimple); // reset 

		// We don't have a context here, so bail
		if(context == sNoContext)
			return;

		// Set pan to full (spread at 180 will keep attenuation curve working, but all 2D
		// panning will be removed)
#if (!UNITY_5_0)
		GetComponent<AudioSource>().spatialBlend = sSetPanLevel;
#else
		GetComponent<AudioSource>().spatialBlend = sSetPanLevel;
#endif

		// set spread to 180
		GetComponent<AudioSource>().spread = sSetSpread;
	}

	// Reset cached variables and free the sound context back to OSPManger
	private void Release()
	{
		if(!AudioSourceExists())return;

		// Reset all audio variables that were changed during play
#if (!UNITY_5_0)
		GetComponent<AudioSource>().spatialBlend = panLevel;
#else
		GetComponent<AudioSource>().spatialBlend = panLevel;
#endif
		GetComponent<AudioSource>().spread   = spread;

		// Put context back into pool
		if(context != sNoContext)
		{
			OSPManager.ReleaseContext (context);
			context = sNoContext;
		}
	}

	// Set the position of the sound relative to the listener
	private void SetRelativeSoundPos(bool firstTime)
	{
		// Find the audio listener (first time used)
		if(audioListener == null)
		{
			audioListener = FindObjectOfType<AudioListener>();

			// If still null, then we have a problem;
			if(audioListener == null)
			{
				Debug.LogWarning ("SetRelativeSoundPos - Warning: No AudioListener present");
				return;
			}
		}

		// Get the location of this sound
		Vector3 sp    = transform.position;
		// Get the main camera in the scene
		Vector3    cp = audioListener.transform.position;
		Quaternion cq = audioListener.transform.rotation;
		// transform the vector relative to the camera
		Quaternion cqi = Quaternion.Inverse (cq);

		// Get the vector between the sound and camera
		lock (this) 
		{
			if(firstTime)
			{
				relPos = cqi * (sp - cp);
				relVel.x = relVel.y = relVel.z = 0.0f;
				relPosTime = GetTime(true);
			}
			else
			{
				Vector3 prevRelPos     = relPos;
				float   prevRelPosTime = relPosTime;
				relPos = cqi * (sp - cp);
				// Reverse z (Unity is left-handed co-ordinates)
				relPos.z = -relPos.z;
				relPosTime = GetTime(true);
				// Calculate velocity
				relVel = relPos - prevRelPos;
				float dTime = relPosTime - prevRelPosTime;
				relVel *= dTime;
			}
		}
	}


	//* * * * * * * * * * * * *
	// Public functions
	//* * * * * * * * * * * * *

	/// <summary>
	/// Play this sound.
	/// </summary>
	public void Play()
	{
		if(!AudioSourceExists())
		{
			Debug.LogWarning ("Play - Warning: No AudioSource on GameObject");
			return;
		}

		// Bail if manager has not even started
		if (OSPManager.IsInitialized () == false) 
			return;

		// We will grab a context at this point, and set the right values
		// to allow for spatialization to take effect
		Aquire();

		// We will set the relative position of this sound now before we start playing
		SetRelativeSoundPos(true);

		// We are ready to play the sound
		GetComponent<AudioSource>().Play();

		lock(this) isPlaying = true;
	}

	/// <summary>
	/// Plays the sound with a delay (in sec.)
	/// </summary>
	/// <param name="delay">Delay.</param>
	public void PlayDelayed(float delay)
	{
		if(!AudioSourceExists())
		{
			Debug.LogWarning ("PlayDelayed - Warning: No AudioSource on GameObject");
			return;
		}
		
		// Bail if manager has not even started
		if (OSPManager.IsInitialized () == false) 
			return;
		
		// We will grab a context at this point, and set the right values
		// to allow for spatialization to take effect
		Aquire();
		
		// We will set the relative position of this sound now before we start playing
		SetRelativeSoundPos(true);
		
		// We are ready to play the sound
		GetComponent<AudioSource>().PlayDelayed(delay);
		
		lock(this) isPlaying = true;
	}

	/// <summary>
	/// Plays the time scheduled relative to current AudioSettings.dspTime.
	/// </summary>
	/// <param name="time">Time.</param>
	public void PlayScheduled(double time)
	{
		if(!AudioSourceExists())
		{
			Debug.LogWarning ("PlayScheduled - Warning: No AudioSource on GameObject");
			return;
		}
		
		// Bail if manager has not even started
		if (OSPManager.IsInitialized () == false) 
			return;
		
		// We will grab a context at this point, and set the right values
		// to allow for spatialization to take effect
		Aquire();
		
		// We will set the relative position of this sound now before we start playing
		SetRelativeSoundPos(true);
		
		// We are ready to play the sound
		GetComponent<AudioSource>().PlayScheduled(time);
		
		lock(this) isPlaying = true;
	}

	/// <summary>
	/// Stop this instance.
	/// </summary>
	public void Stop()
	{
		if(!AudioSourceExists())
		{
			Debug.LogWarning ("Stop - Warning: No AudioSource on GameObject");
			return;
		}

		lock(this) isPlaying = false;

		// Stop audio from playing, and reset any cached values that we
		// have set from Play
		GetComponent<AudioSource>().Stop();

		// Return spatializer context
		Release();
	}

	/// <summary>
	/// Pause this instance.
	/// </summary>
	public void Pause()
	{
		if(!AudioSourceExists())
		{
			Debug.LogWarning ("Pause - Warning: No AudioSource on GameObject");
			return;
		}

		GetComponent<AudioSource>().Pause ();
	}


	//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
	// OnAudioFilterRead (separate thread)
	//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

	/// <summary>
	/// Raises the audio filter read event.
	/// We will spatialize the audio here
	/// NOTE: It's important that we get the audio attenuation curve for volume,
	/// that way we can feed it into our spatializer and have things sound match
	/// with bypass on/off
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="channels">Channels.</param>
	///
	void OnAudioFilterRead(float[] data, int channels)
	{

#if DEBUG_AudioSource
		if(debugOn)
			if(audioFrames > 0)
				audioFrames--;
#endif
		// Problem: We cannot read timer here.
		// However, we can read time-stamp via plugin
		// This is required to smooth out the position,
		// since the main loop is only allowed to update position
		// of sound, but is running at a different frequency then
		// the audio loop.
		// 
		// Therefore, we will need to dead reckon the position of the sound.

		// Do not spatialize if we are not playing
		if ((isPlaying == false) || 
		    (Bypass == true) ||
		    (OSPManager.GetBypass () == true) || 
		    OSPManager.IsInitialized() == false)
			return;

		float dTime = GetTime(true) - relPosTime;
		lock(this)
		{
			relPos += relVel * dTime;
			relPosTime = GetTime(true);
		}

		// TODO: Need to ensure that sound is not played before context is
		// legal
		if(context != sNoContext)
		{
			// Set the position for this context and sound
			OSPManager.SetPositionRel (context, relPos.x, relPos.y, relPos.z);
			//Spatialize
			OSPManager.Spatialize (context, data);
		}

	}

	// GetTime
	// We can bounce between Time and AudioSettings.dspTime
	float GetTime(bool dspTime)
	{
		if(dspTime == true)
			return (float)AudioSettings.dspTime;

		return Time.time;
	}

	// AudioSourceExists
	// We musst have an audio source component attached to this 
	// game object. 
	bool AudioSourceExists()
	{
		if(GetComponent<AudioSource>() == null) 
		{
			return false;
		}

		return true;
	}
}
