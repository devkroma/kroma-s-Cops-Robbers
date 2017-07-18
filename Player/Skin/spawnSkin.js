var skinCameraPos = new Vector3(464.3336, -987.6918, 24.91485);
var skinCameraRot = new Vector3(0.0, 0.0, 175.5411);

var skins;
var currentSkin;

var isSelectingSkin = false;

API.onServerEventTrigger.connect(function (eventName, args) {
  switch (eventName) {

    case 'showPlayerSkinSelection':
	  let skinCamera = API.createCamera(skinCameraPos, skinCameraRot);
	  currentSkin = 0;
	  isSelectingSkin = true;
	  skins = args[0].split(",");
      API.setActiveCamera(skinCamera);
	  currentSkin = 0;
	  API.setPlayerSkin(API.pedNameToModel(skins[currentSkin]));
      break;
  }
});

API.onUpdate.connect(function() {
	if(isSelectingSkin == true)
	{
		if(API.isControlJustPressed(34))
		{
			currentSkin--;
			if(currentSkin < 0)
			{
				currentSkin = skins.length - 1;
			}
			API.setPlayerSkin(API.pedNameToModel(skins[currentSkin]));
		}
		else if(API.isControlJustPressed(35))
		{
			currentSkin++;
			if(currentSkin >= skins.length)
			{
				currentSkin = 0;
			}
			API.setPlayerSkin(API.pedNameToModel(skins[currentSkin]));
		}
		else if(API.isControlJustPressed(23) || API.isControlJustPressed(24))
		{
			API.setActiveCamera(null);
			isSelectingSkin = false;
			API.triggerServerEvent("setPlayerSpawnSkin", skins[currentSkin]);
		}
	}
});