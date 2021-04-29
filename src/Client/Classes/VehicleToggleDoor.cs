﻿using CitizenFX.Core;
using OpenRP.Framework.Common.Enumeration;
using System;
using static CitizenFX.Core.Native.API;

namespace OpenRP.Framework.Client.Classes
{
    public class VehicleToggleDoor : VehicleToggleComponent
    {
        int _index;
        bool _status;

        internal VehicleToggleDoor(int index)
        {
            _index = index;
            _status = false;

            Client.Event.RegisterNuiEvent(NuiEvent.TOGGLE_COMPONENT, new Action<dynamic>(ToggleComponent));
        }

        void ToggleComponent(dynamic args)
        {
            if (args.type == "door" && args.index == _index)
            {
                _status = args.status;
                if (_status)
                    SetVehicleDoorOpen(Vehicle.Handle, _index, false, false);
                else
                    SetVehicleDoorShut(Vehicle.Handle, _index, false);
            }
        }
    }
}
