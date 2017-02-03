Example Scenes:

* BoxSpawner: Begin/Finish scanning the room with the voice commands "Scan" and "Stop" respectively.
Once scanning is completed, a box will be spawned according to the PlacementType variable on the BoxSpawner gameObject. 
The default setting will put the box on the floor somewhere.
Look at the box (from a distance of 2 meters or less) to make a timer start; once it is completed, the box will disappear and a new one will spawn.

* EnvironmentalRequirements: When the program starts, requirements to the playspace are listed on a GUI element in front of you.
These requirements are set on the Requirements gameObject.
Say "Scan" or look at the corresponding button and perform an air-tap gesture to start scanning.
The GUI menu will follow you, updating every few seconds.
Say "Refresh" to manually update the data. The commands "Close menu" or "Open menu" will do just that.
When all requirements are fulfilled (marked in green), a start button will appear. 
Currently, pressing it (again via gaze and air-tap) only finishes the scan and hides the menu.
Say "Quit" at any time or click on the button to close the application.

* InformationFilter: A target is located some distance in front of you (not immediately visible).
Note that this does not take into account room data and the target may be inaccessible; 
the coordinate system is created at the start of the application, so look at an open space to ensure it works as intended.
As you approach the target, events occur according to your distance from it. 
These are set in the InformationFilter_Distance script on the Target gameObject.
Although some events (mainly Text to Speech) may not work correctly, this scene is functional without a HoloLens - simply translate the camera.

* TestScene: A simple scene containing a Point of Interest and Gaze Point of Interest, as well as a regular cube for testing the cursor.
Look at or away from the GazeTarget gameObject or move to or away from the MovementTarget gameObject to trigger actions.
Although some events (mainly Text to Speech) may not work correctly, this scene is functional without a HoloLens - simply translate or rotate the camera.