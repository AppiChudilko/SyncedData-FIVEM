using System;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace Client.Sync
{
    public class Data : BaseScript
    {
        public static bool Debug = false;
        
        private static dynamic _dataGet = null;
        private static bool _dataHas = false;
        private static bool _isServerGetCallBack = false;
        private static bool _isServerHasCallBack = false;
        
        public Data()
        {
            EventHandlers.Add("Sync:Client:Data:Get", new Action<object>(GetServer));
            EventHandlers.Add("Sync:Client:Data:Has", new Action<bool>(HasServer));
            
            Exports.Add("client_sync_data_set", new Action<int, string, object>(Set));
            Exports.Add("client_sync_data_reset", new Action<int, string>(Reset));
            
            Exports.Add("client_sync_data_enable_debug", new Action<bool, CallbackDelegate>((enableDebug, callback) =>
            {
                Debug = enableDebug;
            }));
            
            Exports.Add("client_sync_data_get", new Func<int, string, Task<object>>(async (id, key) => await Get(id, key)));
            Exports.Add("client_sync_data_has", new Func<int, string, Task<bool>>(async (id, key) => await Has(id, key)));
            
            TriggerEvent("OnClientSyncDataLoaded", CitizenFX.Core.Native.API.GetCurrentResourceName());
        }
        
        public static void Set(int id, string key, object value)
        {
            TriggerServerEvent("Sync:Server:Data:Set", id, key, value);
                
            if (Debug)
                CitizenFX.Core.Debug.WriteLine($"[SET] ID: {id}, KEY: {key}, OBJECT: {value}", "");
        }
        
        public static void Reset(int id, string key)
        {
            TriggerServerEvent("Sync:Server:Data:Reset", id, key);
                
            if (Debug)
                CitizenFX.Core.Debug.WriteLine($"[RESET] ID: {id}, KEY: {key}", "");
        }
        
        public static async Task<dynamic> Get(int id, string key, int waitMs = 500)
        {
            TriggerServerEvent("Sync:Server:Data:Get", id, key);
            
            while (!_isServerGetCallBack && waitMs > 0)
            {
                waitMs--;
                await Delay(1);
            }
                
            if (Debug)
                CitizenFX.Core.Debug.WriteLine($"[GET] ID: {id}, KEY: {key}", "");

            var returnData = _dataGet;
            ResetGetCallback();
            return returnData;
        }
        
        public static async Task<bool> Has(int id, string key, int waitMs = 500)
        {
            TriggerServerEvent("Sync:Server:Data:Has", id, key);
            
            while (!_isServerHasCallBack && waitMs > 0)
            {
                waitMs--;
                await Delay(1);
            }
                
            if (Debug)
                CitizenFX.Core.Debug.WriteLine($"[HAS] ID: {id}, KEY: {key}", "");

            bool returnData = _dataHas;
            ResetHasCallback();
            return returnData;
        }
        
        private static async void GetServer(dynamic callback)
        {
            while (_isServerGetCallBack)
            {
                await Delay(1);
            }
            _dataGet = callback;
            _isServerGetCallBack = true;
        }
        
        private static async void HasServer(bool callback)
        {
            while (_isServerHasCallBack)
            {
                await Delay(1);
            }
            _dataHas = callback;
            _isServerHasCallBack = true;
        }
        
        private static void ResetHasCallback()
        {
            _dataHas = false;
            _isServerHasCallBack = false;
        }
        
        private static void ResetGetCallback()
        {
            _dataGet = null;
            _isServerGetCallBack = false;
        }
    }
}