﻿using CitizenFX.Core;
using Newtonsoft.Json;
using OpenRP.Framework.Client.Classes;
using OpenRP.Framework.Common.Enumeration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using static OpenRP.Framework.Client.Classes.VehicleToggleComponent;

namespace OpenRP.Framework.Client.Controllers
{
    public class VehicleController : ClientAccessor
    {
        bool _gate;
        const float _gateAngle = 25;

        internal VehicleController (ClientMain client) : base (client)
        {
            VehicleToggleComponent.Client = client;

            new VehicleEngineToggle();

            new VehicleToggleDoor(0);
            new VehicleToggleDoor(1);
            new VehicleToggleDoor(2);
            new VehicleToggleDoor(3);
            new VehicleToggleDoor(4);
            new VehicleToggleDoor(5);

            new VehicleToggleWindow(0);
            new VehicleToggleWindow(1);
            new VehicleToggleWindow(2);
            new VehicleToggleWindow(3);

            new VehicleSwitchSeats();

            VehicleToggleComponent.TrackedVehicle = new Vehicle(0);
            Seat = -1;
            Taken = new bool[] { false, false, false, false };
            _gate = false;

            Client.RegisterKeyBinding("ToggleVehiclePanel", "(HUD) Vehicle Panel", "grave", new Action(ToggleVehiclePanel));
            Client.RegisterKeyBinding("ToggleLeftSignal", "(Vehicle) Left Signal", "minus", new Action(VehicleTurnSignals.ToggleLeftSignal));
            Client.RegisterKeyBinding("ToggleRightSignal", "(Vehicle) Right Signal", "equals", new Action(VehicleTurnSignals.ToggleRightSignal));

            Client.RegisterTickHandler(WheelMonitor);
            Client.RegisterTickHandler(VehicleMonitor);
        }

        private async void ToggleVehiclePanel()
        {
            if (Game.PlayerPed.IsInVehicle())
            {
                TrackedVehicle = Game.PlayerPed.CurrentVehicle;
                var eventName = "TOGGLE_VEHICLE_PANEL_MODULE";
                UIElement.ToggleNuiModule(eventName, true, true);
                Client.Event.TriggerEvent(ClientEvent.SEND_VEHILCE_STATE);
                await SeatTaken();
            }
        }

        async Task WheelMonitor()
        {
            if (Game.PlayerPed.IsInVehicle())
            {
                var state = GetVehicleIndicatorLights(TrackedVehicle.Handle);

                if (state == 1 || state == 2)
                {
                    var angle = TrackedVehicle.SteeringAngle;

                    if ((angle >= _gateAngle || angle <= _gateAngle * -1) && !_gate)
                    {
                        await BaseScript.Delay(400);
                        angle = TrackedVehicle.SteeringAngle;
                        if ((angle >= _gateAngle || angle <= _gateAngle * -1) && !_gate)
                            _gate = true;
                    }

                    if ((angle < _gateAngle && angle > _gateAngle * -1) && _gate)
                    {
                        VehicleTurnSignals.TurnOffSignals();
                        VehicleTurnSignals.SendState(true);
                        _gate = false;
                    }
                }
            }

            await BaseScript.Delay(50);
        }

        async Task VehicleMonitor()
        {
            if (Game.PlayerPed.IsInVehicle())
            {
                var eventName = "VEHICLE_SPEED_MONITOR";
                var speed = TrackedVehicle.Speed * 2.23694;
                var max = GetVehicleHandlingFloat(TrackedVehicle.Handle, "CHandlingData", "fInitialDriveMaxFlatVel") * 0.82;
                var ratio = speed / max * 100;
                var rpm = TrackedVehicle.CurrentRPM * 100;
                var gearInt = TrackedVehicle.CurrentGear;
                var gear = "";

                if (gearInt == 0)
                    gear = "R";
                else
                    gear = gearInt.ToString();

                var data = new
                {
                    eventName,
                    speed,
                    ratio,
                    rpm,
                    gear
                };

                SendNuiMessage(JsonConvert.SerializeObject(data));
            }

            await BaseScript.Delay(0);
        }
    }
}
