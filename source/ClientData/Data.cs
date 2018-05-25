using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace Client.Sync
{
    public class Data : BaseScript
    {
        public static bool Debug = false;
        private static readonly Dictionary<int, Dictionary<string, object>> _data = new Dictionary<int, Dictionary<string, object>>();
        
        private static dynamic _dataGet = null;
        private static dynamic _dataGetAll = null;
        private static bool _dataHas = false;
        private static bool _isServerGetCallBack = false;
        private static bool _isServerGetAllCallBack = false;
        private static bool _isServerHasCallBack = false;
        
        public Data()
        {
            EventHandlers.Add("Sync:Client:Data:Get", new Action<object>(GetServer));
            EventHandlers.Add("Sync:Client:Data:GetAll", new Action<dynamic>(GetAllServer));
            EventHandlers.Add("Sync:Client:Data:Has", new Action<bool>(HasServer));
            
            Exports.Add("client_sync_data_enable_debug", new Action<bool, CallbackDelegate>((enableDebug, callback) =>
            {
                Debug = enableDebug;
            }));
            
            Exports.Add("client_sync_data_set", new Action<int, string, object>(Set));
            Exports.Add("client_sync_data_reset", new Action<int, string>(Reset));
            Exports.Add("client_sync_data_get", new Func<int, string, Task<object>>(async (id, key) => await Get(id, key)));
            Exports.Add("client_sync_data_getall", new Func<int, Task<object>>(async id => await GetAll(id)));
            Exports.Add("client_sync_data_has", new Func<int, string, Task<bool>>(async (id, key) => await Has(id, key)));
            
            Exports.Add("client_sync_data_set_locally", new Action<int, string, object>(SetLocally));
            Exports.Add("client_sync_data_reset_locally", new Action<int, string>(ResetLocally));
            Exports.Add("client_sync_data_get_locally", new Func<int, string, object>(GetLocally));
            Exports.Add("client_sync_data_getall_locally", new Func<int, object>(GetAllKeyLocally));
            Exports.Add("client_sync_data_has_locally", new Func<int, string, bool>(HasLocally));
            
            TriggerEvent("OnClientSyncDataLoaded", CitizenFX.Core.Native.API.GetCurrentResourceName());
        }
        
        public static void SetLocally(int id, string key, object value)
        {
            lock (_data)
            {
                if (_data.ContainsKey(id))
                {
                    _data[id].Set(key, value);
                }
                else
                {
                    _data.Add(id, new Dictionary<string, object>());
                    _data[id].Set(key, value);
                }
                
                if (Debug)
                    CitizenFX.Core.Debug.WriteLine($"[SET-LOCALLY] ID: {id}, KEY: {key}, OBJECT: {value}");
            }
        }
        
        public static void ResetLocally(int id, string key)
        {
            lock (_data)
            {
                if (!_data.ContainsKey(id) || !_data[id].ContainsKey(key)) return;
                
                _data[id].Remove(key);
                
                if (Debug)
                    CitizenFX.Core.Debug.WriteLine($"[RESET-LOCALLY] ID: {id}, KEY: {key}");
            }
        }
        
        public static dynamic GetLocally(int id, string key)
        {
            lock (_data)
            {
                if (Debug)
                    CitizenFX.Core.Debug.WriteLine($"[GET-LOCALLY] ID: {id}, KEY: {key}");
                
                return _data.ContainsKey(id) ? _data[id].Get(key) : null;
            }
        }
        
        public static bool HasLocally(int id, string key)
        {
            lock (_data)
            {
                if (Debug)
                    CitizenFX.Core.Debug.WriteLine($"[HAS-LOCALLY] ID: {id}, KEY: {key}");
                
                return _data.ContainsKey(id) && _data[id].ContainsKey(key);
            }
        }
        
        public static string[] GetAllKeyLocally(int id)
        {
            lock (_data)
            {
                return _data.ContainsKey(id) ? _data[id].Select(pair => pair.Key).ToArray() : new string[0];
            }
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
        
        public static async Task<dynamic> GetAll(int id, int waitMs = 500)
        {
            try
            {
                TriggerServerEvent("Sync:Server:Data:GetAll", id);
                
                while (!_isServerGetAllCallBack && waitMs > 0)
                {
                    waitMs--;
                    await Delay(1);
                }
                    
                if (Debug)
                    CitizenFX.Core.Debug.WriteLine($"[GETALL] ID: {id}", "");
    
                dynamic returnData = _dataGetAll;
                ResetGetAllCallback();
                return returnData;
            }
            catch (Exception e)
            {
                CitizenFX.Core.Debug.WriteLine($"GETALL {e}");
                throw;
            }
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
        
        private static async void GetAllServer(dynamic callback)
        {
            while (_isServerGetCallBack)
            {
                await Delay(1);
            }
            _dataGetAll = callback;
            _isServerGetAllCallBack = true;
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
        
        private static void ResetGetAllCallback()
        {
            _dataGetAll = null;
            _isServerGetAllCallBack = false;
        }
    }
}