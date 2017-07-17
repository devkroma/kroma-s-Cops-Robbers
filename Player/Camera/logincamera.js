var loginCameraPos = new Vector3(350.0, 200.0, 150.0);
var loginCameraRot = new Vector3(-25.0, 0.0, 180.0);

/*API.onServerEventTrigger.connect(function (eventName, args)
{
	switch(eventName)
	{
		case 'setPlayerLoginCamera':
			API.setActiveCamera(loginCamera);
			break;
		case 'resetPlayerLoginCamera':
			API.setActiveCamera(null);
			break;
	}
});*/

API.onServerEventTrigger.connect(function (eventName, args) {
  switch (eventName) {

    case 'setPlayerLoginCamera':
	  let loginCamera = API.createCamera(loginCameraPos, loginCameraRot);
      API.setActiveCamera(loginCamera);
      break;
    case 'resetPlayerLoginCamera':
      API.setActiveCamera(null);
      break;
  }
});