Client = {
    Sync = {
    	Data = {}
    }
}

function Client.Sync.Data.enableDebug(isEnable)
    exports['synceddata']:client_sync_data_enable_debug(isEnable)
end

function Client.Sync.Data.set(id, key, value)
    exports['synceddata']:client_sync_data_set(id, key, value)
end

function Client.Sync.Data.reset(id, key)
    exports['synceddata']:client_sync_data_set(id, key)
end

function Client.Sync.Data.get(id, key)
    return exports['synceddata']:client_sync_data_get(id, key)
end

function Client.Sync.Data.has(id, key)
    return exports['synceddata']:client_sync_data_has(id, key)
end

function Client.Sync.Data.setLocally(id, key, value)
    exports['synceddata']:client_sync_data_set_locally(id, key, value)
end

function Client.Sync.Data.resetLocally(id, key)
    exports['synceddata']:client_sync_data_set_locally(id, key)
end

function Client.Sync.Data.getLocally(id, key)
    return exports['synceddata']:client_sync_data_get_locally(id, key)
end

function Client.Sync.Data.hasLocally(id, key)
    return exports['synceddata']:client_sync_data_has_locally(id, key)
end