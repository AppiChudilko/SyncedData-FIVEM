Server = {
    Sync = {
    	Data = {}
    }
}

function Server.Sync.Data.enableDebug(isEnable)
    exports['synceddata']:server_sync_data_enable_debug(isEnable)
end

function Server.Sync.Data.set(id, key, value)
    exports['synceddata']:server_sync_data_set(id, key, value)
end

function Server.Sync.Data.reset(id, key)
    exports['synceddata']:server_sync_data_set(id, key)
end

function Server.Sync.Data.get(id, key)
    return exports['synceddata']:server_sync_data_get(id, key)
end

function Server.Sync.Data.has(id, key)
    return exports['synceddata']:server_sync_data_get(id, key)
end