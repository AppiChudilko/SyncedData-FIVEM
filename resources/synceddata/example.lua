AddEventHandler('onServerResourceStart', function (resource)
    if resource == "synceddata" then
        Server.Sync.Data.enableDebug(true)
		Server.Sync.Data.set(1, "appi", 666)
		local test = Server.Sync.Data.get(1, "appi")
    end
end)
