Client = {
    Sync = {
    	Data = {}
    }
}

function Client.Sync.Data.enableDebug(isEnable)
	assert(type(id) == "bool", "The ID must be a string")
    exports['synceddata']:client_sync_data_enable_debug(isEnable)
end

function Client.Sync.Data.set(id, key, value)
	assert(type(id) == "number", "The ID must be a string")
	assert(type(key) == "string", "The KEY must be a string")
    exports['synceddata']:client_sync_data_set(id, key, value)
end

function Client.Sync.Data.reset(id, key)
	assert(type(id) == "number", "The ID must be a string")
	assert(type(key) == "string", "The KEY must be a string")
    exports['synceddata']:client_sync_data_set(id, key)
end

function Client.Sync.Data.get(id, key)
	assert(type(id) == "number", "The ID must be a string")
	assert(type(key) == "string", "The KEY must be a string")
    return exports['synceddata']:client_sync_data_get(id, key)
end

function Client.Sync.Data.has(id, key)
	assert(type(id) == "number", "The ID must be a string")
	assert(type(key) == "string", "The KEY must be a string")
    return exports['synceddata']:client_sync_data_get(id, key)
end