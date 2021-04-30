﻿using CitizenFX.Core;
using Newtonsoft.Json;
using OpenRP.Framework.Common.Enumeration;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace OpenRP.Framework.Client.Classes
{
    public class VehicleToggleComponent
    {
        internal static ClientMain Client { get; set; }
        internal static Vehicle Vehicle { get; set; }
        internal static int Seat { get; set; }
        internal static bool[] Taken { get; set; }
        internal static int Seats { get; set; }

        internal VehicleToggleComponent()
        {
            Client.RegisterTickHandler(PlayerStateMonitor);
        }

        internal static void SendPanelData()
        {
            string eventName = "VEHICLE_PANEL_DATA";
            var data = new
            {
                eventName,
                Seat,
                Taken,
                Seats
            };
            SendNuiMessage(JsonConvert.SerializeObject(data));
        }

        internal static void SendPanelState(string type, int index, bool status, bool broken)
        {
            string eventName = "VEHICLE_PANEL_STATE";
            var data = new
            {
                eventName,
                type,
                index,
                status,
                broken
            };
            SendNuiMessage(JsonConvert.SerializeObject(data));
        }

        private async Task PlayerStateMonitor()
        {
            if (Game.PlayerPed.VehicleTryingToEnter != null)
            {
                Vehicle = Game.PlayerPed.VehicleTryingToEnter;
                Client.Event.TriggerServerEvent(ServerEvent.RECEIVE_VEHICLE_STATE, Vehicle.Handle, Vehicle.IsEngineRunning);
                SetVehicleEngineOn(Vehicle.Handle, Vehicle.IsEngineRunning, true, true);
                await SeatTaken();
                await BaseScript.Delay(3000);
            }

            await BaseScript.Delay(0);
        }

        internal static async Task SeatTaken()
        {
            Seat = -2;
            Seats = 0;

            while (Seat < -1)
            {
                Seat = (int)Game.PlayerPed.SeatIndex;
                await BaseScript.Delay(50);
            }

            for (var i = -1; i < 3; i++)
                Taken[i + 1] = !IsVehicleSeatFree(Vehicle.Handle, i);

            Seats = GetVehicleModelNumberOfSeats((uint)Vehicle.Model.Hash);

            Debug.WriteLine($"Seats: {Seats}");
            SendPanelData();
        }
    }
}