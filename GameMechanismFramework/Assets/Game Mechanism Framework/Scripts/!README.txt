The framework consists of 13 core scripts and a few helper classes 
(which are applied in the example scenes or can be used for testing but are not essential or are heavily based on existing scripts). 

Interactable, InteractableCheck_Gaze, and InteractableCheck_Position together form the basis for the mechanisms POINT OF INTEREST and GAZE POINT OF INTEREST. 
The scripts inheriting from InteractableCheck provide different ways of detecting Interactable objects and call specific methods therein (Enter(), Stay(), and Exit()). 
What type of interaction an object supports is based on the layer it is placed on inside of Unity.

The EnvironmentRequirements class implements the pattern of the same name. 
Requirements consist of an environmental feature, an amount, and whether the amount represents an upper or lower limit. 
This data is compared to that gathered by the HoloLens. 
EnvironmentRequirementGUI represents one way of visualizing the available information.

The spatial understanding features of the device are further utilized in SpatialUnderstandingSpawner, 
an implementation of ENVIRONMENT-ADAPTION which uses predefined sets of rules and constraints from SpawnInformation to find suitable spots for instantiating objects, 
e.g. on a wall or on a floor, far from the player.

EnvironmentRequirementsFromUnderstandingSpawner provides a bridge between the two previous mechanisms 
by generating simple requirements from the parameters of a SpatialUnderstandingSpawner.

Finally, InformationFilter is, as the name suggests, an implementation of INFORMATION FILTERING, 
executing tasks according to rules based on the data from InformationFilterMetric-derived classes, 
such as IFMetric_Distance, which takes into account the distance between the object it is placed on and e.g. the player.