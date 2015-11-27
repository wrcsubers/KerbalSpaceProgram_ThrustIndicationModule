//=====================================================================================
// The MIT License (MIT)
// 
// T.I.M. - Thrust Indication Module - Copyright (c) 2015 WRCsubeRS
// 
// T.I.M. - Thrust Indication Module - A Mod for Kerbal Space Program by Squad
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
//=====================================================================================
//
//Version 1.0 - Initial Release 11.26.15
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TIM
{
	[KSPAddon (KSPAddon.Startup.Flight, false)]
	public class ThrustIndicationModule : MonoBehaviour
	{
		//============================================================================================================================================
		//Define Variables
		//============================================================================================================================================
		private List <Part> ActiveEngines = new List<Part> ();
		private int PartCount = 0;
		private Gauge TIMGauge;
		private float TotalCapableThrust;
		private float TotalCurrentThrust;
		private float ThrustPercentage;

		//============================================================================================================================================
		//Start Running Processes
		//============================================================================================================================================
		//This function gets called only once, during the KSP loading screen.
		private void Awake ()
		{

		}

		//Called when the flight starts or in the editor. OnStart will be called before OnUpdate or OnFixedUpdate are ever called.
		private void Start ()
		{
			//Thanks to KSP Forum Member xEvilReeperx for helping with this...
			var SourceObject = FlightUIController.fetch.thr.gameObject;
			var ClonedObject = UnityEngine.Object.Instantiate (SourceObject, SourceObject.transform.position, SourceObject.transform.rotation) as GameObject;
			TIMGauge = ClonedObject.GetComponent<Gauge> ();
			//Set the TIMGauge to a different color... Color.gray is default
			TIMGauge.transform.GetChild (0).renderer.material.color = Color.gray;

			ClonedObject.transform.parent = SourceObject.transform.parent;

			TIMGauge.minRot = FlightUIController.fetch.thr.minRot;
			TIMGauge.maxRot = FlightUIController.fetch.thr.maxRot;
			TIMGauge.minValue = FlightUIController.fetch.thr.minValue;
			TIMGauge.maxValue = FlightUIController.fetch.thr.maxValue;
			TIMGauge.responsiveness = FlightUIController.fetch.thr.responsiveness;
			TIMGauge.setValue (0f);
			TIMGauge.enabled = true;
		}

		//This method runs every physics frame
		//============================================================================================================================================
		private void FixedUpdate ()
		{
			//We can reduce compute times by testing only engines from a list rather than every part on the ship
			if (PartCount != FlightGlobals.ActiveVessel.parts.Count ()) {
				//Make a new list of Engines if the Part Count has changed since last frame
				ActiveEngines.Clear ();
				foreach (Part Engine in FlightGlobals.ActiveVessel.parts) {
					if (Engine.Modules.Contains ("ModuleEngines") | Engine.Modules.Contains ("ModuleEnginesFX")) {
						ActiveEngines.Add (Engine);
					}
				}
				//Set Part count to current count
				PartCount = FlightGlobals.ActiveVessel.parts.Count ();
			}
			//Perform calculations for each engine
			foreach (Part SingleEngine in ActiveEngines) {
				//ModuleEngines
				//------------------------------------------------------------------------------------------------------------------------------
				if (SingleEngine.Modules.Contains ("ModuleEngines")) {
					ModuleEngines ME1 = new ModuleEngines ();
					ModuleEngines ME2 = new ModuleEngines ();
					ME1 = SingleEngine.FindModulesImplementing<ModuleEngines> ().First ();
					ME2 = SingleEngine.FindModulesImplementing<ModuleEngines> ().Last ();
					//Do Calculations for Module 1 if it is operational
					if (ME1.isOperational == true) {
						TotalCapableThrust = TotalCapableThrust + (ME1.GetMaxThrust () * (ME1.thrustPercentage / 100));
						TotalCurrentThrust = TotalCurrentThrust + ME1.GetCurrentThrust ();
					}
					//If Module 2 is different from 1 (by engineID) do calculations for Module 2 if it is operational
					if (ME1.engineID != ME2.engineID) {
						if (ME2.isOperational == true) {
							TotalCapableThrust = TotalCapableThrust + (ME2.GetMaxThrust () * (ME2.thrustPercentage / 100));
							TotalCurrentThrust = TotalCurrentThrust + ME2.GetCurrentThrust ();
						}
					}
				}
				//ModuleEnginesFX
				//------------------------------------------------------------------------------------------------------------------------------
				if (SingleEngine.Modules.Contains ("ModuleEnginesFX")) {
					ModuleEnginesFX MEFX1 = new ModuleEnginesFX ();
					ModuleEnginesFX MEFX2 = new ModuleEnginesFX ();
					MEFX1 = SingleEngine.FindModulesImplementing<ModuleEnginesFX> ().First ();
					MEFX2 = SingleEngine.FindModulesImplementing<ModuleEnginesFX> ().Last ();
					//Do Calculations for Module 1 if it is operational
					if (MEFX1.isOperational == true) {
						TotalCapableThrust = TotalCapableThrust + (MEFX1.GetMaxThrust () * (MEFX1.thrustPercentage / 100));
						TotalCurrentThrust = TotalCurrentThrust + MEFX1.GetCurrentThrust ();
					}
					//If Module 2 is different from 1 (by engineID) do calculations for Module 2 if it is operational
					if (MEFX1.engineID != MEFX2.engineID) {
						if (MEFX2.isOperational == true) {
							TotalCapableThrust = TotalCapableThrust + (MEFX2.GetMaxThrust () * (MEFX2.thrustPercentage / 100));
							TotalCurrentThrust = TotalCurrentThrust + MEFX2.GetCurrentThrust ();
						}
					}
				}
			}
			//Prevent Divide by 0 Errors
			if (TotalCapableThrust == 0) {
				TotalCapableThrust = 0.0001f;
			}
			//Not truly a percentage, gauge works between 0 and 1, NOT 0 and 100
			ThrustPercentage = ((TotalCurrentThrust / TotalCapableThrust));
			//Set Gauge to Calculated Value
			TIMGauge.setValue (ThrustPercentage);
			//Reset Thrust Values Each Compute Cycle
			TotalCapableThrust = 0;
			TotalCurrentThrust = 0;
		}
	}
}