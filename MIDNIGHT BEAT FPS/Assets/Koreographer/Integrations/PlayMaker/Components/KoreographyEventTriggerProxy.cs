﻿//----------------------------------------------
//                 Koreographer                 
//    Copyright © 2014-2024 Sonic Bloom, LLC    
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using Tooltip=UnityEngine.TooltipAttribute;

namespace SonicBloom.Koreo.PlayMaker
{
	[AddComponentMenu("Koreographer/PlayMaker/Koreography Event Trigger Proxy")]
	public class KoreographyEventTriggerProxy : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		[Tooltip("Each mapping represents a connection between a Koreography Event ID and target PlayMaker FSMs.")]
		List<CallbackProxyRouter> eventMappings = null;

		#endregion
		#region Methods

		void Start()
		{
			for (int i = 0; i < eventMappings.Count; ++i)
			{
				eventMappings[i].RegisterCallbacks();
			}
		}

		void OnDestroy()
		{
			for (int i = 0; i < eventMappings.Count; ++i)
			{
				eventMappings[i].UnregisterCallbacks();
			}

			eventMappings.Clear();
		}

		#endregion
	}

	[System.Serializable]
	class CallbackProxyRouter
	{
		#region Fields

		[SerializeField]
		[Tooltip("Koreography Event ID")]
		[EventID]
		string eventID = string.Empty;

		[SerializeField]
		[Tooltip("FSM Event to send when a Koreography Event from a Koreography Track with the given Event ID is triggered.")]
		string sendEvent = string.Empty;

		[SerializeField]
		[Tooltip("If checked, the Target FSMs list is ignored and this event is sent to ALL running PlayMakerFSM components.")]
		bool globalEvent = false;

		[SerializeField]
		[Tooltip("Specific PlayMakerFSM components to which to send the event.  Only used if Global Event is unchecked.")]
		List<PlayMakerFSM> targetFSMs = null;

		[Header("Optional")]
		
		[SerializeField]
		[Tooltip("A specific Koreographer component with which to register for events.  If this is null, it will use the singleton default.")]
		Koreographer targetKoreographer = null;

		KoreographyEventInfo info = new KoreographyEventInfo();

		#endregion
		#region Methods

		public void RegisterCallbacks()
		{
			Koreographer koreoCom = targetKoreographer;
			if (koreoCom == null)
			{
				koreoCom = Koreographer.Instance;
			}

			if (koreoCom != null)
			{
				koreoCom.RegisterForEventsWithTime(eventID, HandleKoreographyEvent);
			}
		}

		public void UnregisterCallbacks()
		{
			Koreographer koreoCom = targetKoreographer;
			if (koreoCom == null)
			{
				koreoCom = Koreographer.Instance;
			}
			
			if (koreoCom != null)
			{
				koreoCom.UnregisterForEvents(eventID, HandleKoreographyEvent);
			}
		}

		void HandleKoreographyEvent(KoreographyEvent evt, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
		{
			info.koreoEvent = evt;
			info.sampleTime = sampleTime;
			info.sampleDelta = sampleDelta;
			info.deltaSlice = deltaSlice;

			// Store the current info into a global object.
			KoreographyEventInfo.SetCallbackInfo(info);

			// Set up the correct list to traverse.
			List<PlayMakerFSM> fsmsToTrigger = globalEvent ? PlayMakerFSM.FsmList : targetFSMs;

			// Trigger the event.  Listeners will access the global KoreographyEventInfo object.
			for (int i = 0; i < fsmsToTrigger.Count; ++i)
			{
				fsmsToTrigger[i].SendEvent(sendEvent);
			}

			// Remove our event info from the global object.
			KoreographyEventInfo.ClearCallbackInfo();

			// Reset our local info, freeing any memory references.
			info.Reset();
		}

		#endregion
	}
}
