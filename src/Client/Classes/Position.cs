﻿using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace OpenRP.Framework.Client.Classes
{
    public static class Position
    {
        public static async Task Teleport(Vector3 pos)
        {
            var ground = 950f;

            for (var zz = 950f; zz >= 0f; zz -= 25f)
            {
                var z = zz;

                if (zz % 2 != 0)
                {
                    z = 950f - zz;
                }

                RequestCollisionAtCoord(pos.X, pos.Y, z);
                NewLoadSceneStart(pos.X, pos.Y, z, pos.X, pos.Y, z, 50f, 0);

                int timer = GetGameTimer();

                while (IsNetworkLoadingScene())
                {
                    if (GetGameTimer() - timer > 1000)
                        break;
                    await BaseScript.Delay(0);
                }

                SetEntityCoords(Game.PlayerPed.Handle, pos.X, pos.Y, z, false, false, false, true);
                timer = GetGameTimer();

                while (!HasCollisionLoadedAroundEntity(Game.PlayerPed.Handle))
                {
                    if (GetGameTimer() - timer > 1000)
                        break;
                    await BaseScript.Delay(0);
                }

                var found = GetGroundZFor_3dCoord(pos.X, pos.Y, z, ref ground, false);

                if (found)
                {
                    SetEntityCoords(Game.PlayerPed.Handle, pos.X, pos.Y, ground, false, false, false, true);
                    break;
                }

                await BaseScript.Delay(0);
            }
        }
    }
}
