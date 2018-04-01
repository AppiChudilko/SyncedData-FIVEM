Resource for LUA
If u wanna using C#, just include ClientData.net.dll (On client side) and ServerData.net.dll (On server side) in ur project 

And include in __resource.lua

client_scripts{
  "ClientData.net.dll"
}
server_script {
  "ServerData.net.dll"
}