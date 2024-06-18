# MetaMorphosMulti
Local multiplayer version of MetaMorphos VR using Mirror Networking plugin.


# Network Scene and main gameobjects
. The GameObject 'VRNetworkDiscovery' has got differents components :
  - Kcp Transport
  - Network Discovery to manage the kcp transport component and VR Canvas HUD gameobject
  - VR Network Manager to manage IP address of the host (the host vr headset), the player prefab ('NetworkRig' prefab) and all the registered spawnable prefabs (all the animals)

. The GameLogic gameobject contains scripts to manage buttons of the controllers and a GameLogicSpawn script to manage spawning of each animal regading button of the first menu clicked by the host

. There's 3 panels :
 -  One HUD for the choice of the ip address, client or host mode
 -  One for the choice of the animal
 -  One for the choice of the 'state' of the animal choosen (and all the systems)

# Current state of the application
Actualy, Clients can set the IP address to join the host. They'll seee the animal choosen by the host but they can't see the change of the state (for example, if the host has selected the intoto view with digestive system, it won't be updated on the server). The clients can see the name changed regarding what's pointed by the host but they can't see the highlighting (I couldn't succeed to update it on the server because it's a different library so I can't add mirror library on it)

# The idea of finalized app
We would like the host to be the only one who can choose an animal and change its state. The result should be visible to all clients on the server. The host must be the only one capable of pointing out the different elements of the animal and ensuring that the highlighting is visible to all customers.
