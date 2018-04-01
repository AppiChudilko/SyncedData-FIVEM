using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;

namespace Server.Sync
{
    public class Data : BaseScript
    {
        public static bool Debug = false;
        private static readonly Dictionary<int, Dictionary<string, object>> _data = new Dictionary<int, Dictionary<string, object>>();
        
        public Data()
        {
            EventHandlers.Add("Sync:Server:Data:Set", new Action<int, string, object>(Set));
            EventHandlers.Add("Sync:Server:Data:Reset", new Action<int, string>(Reset));
            EventHandlers.Add("Sync:Server:Data:Get", new Action<Player, int, string>(GetClient));
            EventHandlers.Add("Sync:Server:Data:Has", new Action<Player, int, string>(HasClient));
        }
        
        public static void Set(int id, string key, object value)
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
                    CitizenFX.Core.Debug.WriteLine($"[SET] ID: {id}, KEY: {key}, OBJECT: {value}");
            }
        }
        
        public static void Reset(int id, string key)
        {
            lock (_data)
            {
                if (!_data.ContainsKey(id) || !_data[id].ContainsKey(key)) return;
                
                _data[id].Remove(key);
                
                if (Debug)
                    CitizenFX.Core.Debug.WriteLine($"[RESET] ID: {id}, KEY: {key}");
            }
        }
        
        public static dynamic Get(int id, string key)
        {
            lock (_data)
            {
                if (Debug)
                    CitizenFX.Core.Debug.WriteLine($"[GET] ID: {id}, KEY: {key}");
                
                return _data.ContainsKey(id) ? _data[id].Get(key) : null;
            }
        }
        
        public static bool Has(int id, string key)
        {
            lock (_data)
            {
                if (Debug)
                    CitizenFX.Core.Debug.WriteLine($"[HAS] ID: {id}, KEY: {key}");
                
                return _data.ContainsKey(id) && _data[id].ContainsKey(key);
            }
        }
        
        public static string[] GetAll(int id)
        {
            lock (_data)
            {
                return _data.ContainsKey(id) ? _data[id].Select(pair => pair.Key).ToArray() : new string[0];
            }
        }
        
        private static void GetClient([FromSource] Player player, int id, string key)
        {
            if (Debug)
                CitizenFX.Core.Debug.WriteLine($"[GETCLIENT] ID: {id}, KEY: {key}");
            
            TriggerClientEvent(player, "Sync:Client:Data:Get", Get(id, key));
        }
        
        private static void HasClient([FromSource] Player player, int id, string key)
        {
            if (Debug)
                CitizenFX.Core.Debug.WriteLine($"[HASCLIENT] ID: {id}, KEY: {key}");
            
            TriggerClientEvent(player, "Sync:Client:Data:Has", Has(id, key));
        }
    }
}