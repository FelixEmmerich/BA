The framework consists of nine core scripts and a few helper classes 
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

Finally, InformationFilter_Distance is – as the name suggests – a variant on INFORMATION FILTERING, 
based on the distance between the user and an object (and thus an alternative approach to POINT OF INTEREST). 
Events are invoked when the user enters or leaves certain ranges.