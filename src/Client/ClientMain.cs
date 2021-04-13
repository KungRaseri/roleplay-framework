﻿using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using OpenRP.Framework.Client.Controllers;
using static CitizenFX.Core.Native.API;

namespace OpenRP.Framework.Client
{
    /// <summary>
    /// Main client-side entry point for the OpenRP Framework
    /// </summary>
    public class ClientMain : BaseScript
    {
        public PlayerList players;

        public readonly VoiceController voiceController;

        public ClientMain()
        {
            players = Players;

            voiceController = new VoiceController(this);

            Initialize();

            Debug.WriteLine($"[{nameof(ClientMain)}] resources loaded");
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (!GetCurrentResourceName().Equals(resourceName))
                return;
            Debug.WriteLine($"[{resourceName}] start");
        }

        private void OnClientResourceStop(string resourceName)
        {
            if (!GetCurrentResourceName().Equals(resourceName))
                return;
            Debug.WriteLine($"[{resourceName}] stop");
        }

        private void Initialize()
        {
        }

        public void RegisterKeyBinding(string commandString, string description, string defaultMapper, string defaultParameter)
        {
            RegisterKeyMapping(commandString, description, defaultMapper, defaultParameter);
        }

        public void RegisterClientEvent(IEvent e)
        {
            try
            {
                //Events.RegisterEvent(e);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void UnregisterClientEvent(IEvent e)
        {
            try
            {
                //Events.UnregisterEvent(e);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void RegisterTickHandler(Func<Task> action)
        {
            try
            {
                Tick += action;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void UnregisterTickHandler(Func<Task> action)
        {
            try
            {
                Tick -= action;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void RegisterExport(string exportName, Delegate callback)
        {
            Exports.Add(exportName, callback);
        }

        public dynamic GetExport(string resourceName)
        {
            return Exports[resourceName];
        }
    }
}