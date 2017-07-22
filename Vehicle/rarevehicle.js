var garageMarker;
var garageBlip;
var garageLocation = new Vector3(1000.0, 1000.0, 1000.0);
var isDeliveringVehicle = false;
var localPlayer = API.getLocalPlayer();
var rareVehicle;

API.onServerEventTrigger.connect(function (eventName, args) {
	switch (eventName) {
		case 'createGarageMarker':
			garageMarker = API.createMarker(1, args[0], args[0], new Vector3(0.0, 0.0, 0.0), new Vector3(1.0, 1.0, 1.0), 255, 0, 0, 255);
			garageLocation = args[0];
			garageBlip = API.createBlip(garageLocation);
			API.setBlipSprite(garageBlip, 357);
			API.setBlipColor(garageBlip, 11);
			break;
		case 'setRareVehicle':
			rareVehicle = args[0];
			isDeliveringVehicle = true;
			break;
		case 'createGarageWaypoint':
			API.setBlipRouteVisible(garageBlip, true);
			API.setBlipRouteColor(garageBlip, 11);
			break;
		case 'onRareVehicleDelivered':
			garageLocation = new Vector3(1000.0, 1000.0, 1000.0);
			isDeliveringVehicle = false;
			rareVehicle = null;
			API.setBlipRouteVisible(garageBlip, false);
			API.deleteEntity(garageMarker);
			API.deleteEntity(garageBlip.handle);
	}
});

/*API.onUpdate.connect(function() {
	if(isDeliveringVehicle)
	{
		if(API.isPlayerInAnyVehicle(localPlayer))
		{
			if(API.getPlayerVehicle(localPlayer) == rareVehicle)
			{
				var playerPos = API.getEntityPosition(localPlayer.handle);
				if(API.isInRangeOf(playerPos, garageLocation, 50))
				{
					garageLocation = new Vector3(1000.0, 1000.0, 1000.0);
					isDeliveringVehicle = false;
					rareVehicle = null;
					API.triggerServerEvent("onRareVehicleDelivered");
					API.setBlipRouteVisible(garageBlip, false);
					API.deleteEntity(garageMarker.handle);
					API.deleteEntity(garageBlip.handle);
				}
			}
		}
	}
});*/