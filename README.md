# UnityAnalyticsTest
 
 
 ## How to Use
 
 Open the Unity project where you want to use the tool, and drop the unitypackage from the lastest release.
 To try the Tool without having to create a videogame, we recommend you open the ExampleScene.
 
 ### SDV
 
 In order to use the tool, each GameObject that the user wants to track, will need to have an SDVEventTracker component
 
 
 ### EventHandler
  
The EventHandler is the component in charge of collection events. There is only one EventHandler for each scene, and this component will need to be attached to a GameObject in the scene in order to collect events. This is intrusion, although is minimal, as the Component does not need any additional GameObject. The GameObject will contain a list of Editor Events. These are created by the player, and are basically the different events that will be collected during the execution of the game. Right now, events can store five different types of data Figure2, plus not store data at all, for example, only use position. Events are also divided on types of events, where the number of targets is the difference, plus a position type for easy implementation

•	No target: This type is only added in case the user wants to visualize events outside of the tool. Due to not having target, there is no way to track any positions or GameObjects. Still, it allows the user to use our tool for capturing events such as level starts or completions, instead of having to use another tool.

•	Position: This simple type is implemented in order for the users to get familiar with the tool and be able to start tracking the position of a GameObject easily.

•	Single Target:  With this type, the user will be able to track a specific variable from a specific component of the GameObject. The user can call this event from code and pass the variable, or configure the event to track the variable periodically. In order to do so, the user will need to write the name of the script and the name of the variable, and if they exist and are serializable, the EventHandler will track them.

•	Multi Target: This type allows the user to store the same type of event but with different target GameObjects each time. Because the event does not know which GameObjects to track, this event can only be stored from code.

Aside from event types, the user has also event options that can configure in order to further customize their event.

•	Active: This variable is independent for each scene, and allows the user to activate or deactivate certain events in specific scenes, in case they use events only in certain scenes.

•	Frequency: Frequency can be used in case the event is of type targeted, and will be used if it’s of type position. If use frequency is checked, this variable will dictate how frequently the EventHandler will save the variable it is being tracked, or position in case of the event being position type.

•	Save Position: This checkbox can be ticked in order for the event to store the position of the target, in addition to the variable it is being tracked. The position can be tracked by frequency, if used by the event, or by passing the position as an optional variable through code.

•	Target: For targeted and position events, this field lets the user set the GameObject from the scene that it’s going to be tracked.

There are two ways to add events in SDV. As stated, the first way is to activate the frequency and leave the Event Handler in charge of storing the events. An alternative, and a necessary one if frequency is not being used, is to store the events from the code. In order to do so, the users will need to call StoreEventStatic, and pass the name of the event they want to store, and the data of the event. Additional parameters can be sent, specifically the position of the event, if the event is saving the position, or the target GameObject, in case the event is either tracking a specific target, or more importantly, if the event is of type multi target.


### HeatMap

To access the heatmap, the users will need to go to Window->SDV->Heatmap, and open the heatmap window. The window is divided in sections, separated with lines, and each section handles a different aspect of the Heatmap.

•	Colouring: Here the users can change both the gradient and the material used. The gradient is a fully functional Unity gradient, where the users can set as many colors and transparency values, in positions from 0 to 100, and then each heatmap cube color is obtained depending on the events contained divided by the maximum events. The material field changes the material used to draw the cubes of the heatmap. A default material is included in the SDV unity package, but with proper knowledge of shaders, users can create materials that blend with each other, in order to have a better final visualization.

•	Selection: This section allows the user to selecta specific area of the scene using two handles that delimit the selection square. If the user selects only visualize selection, only the heatmap cubes that are inside the square will be rendered.

•	Shape and Size: This section lets the user alter the shape and size of each heatmap cube. Currently, the user can switch between cube shapes and sphere shapes. Cube size dictates the size of the heatmap cubes side, and if the users check the Modify size checkmark, they will have access a new variable, Size modifier. It acts as a multiplier for the size of the heatmap cube, making the relationship between the number of events and the size exponential.

•	Information: Is a collection of simple labels that give the user technical information about the heatmap, like the selected cubes, the number of cubes in each cell and the maximum number of events in a unique cube.

•	Bar Chart: Once the heatmap is generated, a simple bar chart will appear in this section. The number of events as well as the name is also displayed. The user can change the colors of each bar in the section below.

•	Events: Here, the user will be able to see what events are available to be loaded into SDV’s heatmap. Once they are correctly loaded, both a toggle and a color field will appear. The color field is used to set the color of the event in the Bar Chart, while the toggle, or checkbox, lets the users choose if they want to visualize the events in both the heatmap and the bar chart. In other words, if the checkbox is unchecked, the events will still be loaded but will not be used for heatmap calculations.

In order to create the Heatmap grid, all the loaded events, that are also set to use, are classified in each cell, the HeatCubes, of the grid. Then a recount is done to find the cubes with the most events, and the color of each cube is determined following this maximum amount.


### GameObjects

To use SDV’s GameObjects, the user will need to open the GameObjects window, by navigating Unity’s header menu: Window->SVD->GameObjects, where the users will be able to customize how the events are viewed. 
 
•	Color: This color is only used when the events are not viewed separately, and instead, each GameObject has a cube drawn on top with all the events, where the color depends on the events divided by the maximum number of events.

•	Distribute events: Once the events have been loaded, the users will need to press this button in order to distribute all the loaded events into their corresponding GameObjects. When the users load more CSV, they will need to hit the button again.

•	View events separately: This toggle lets the user switch between viewing all the events as a single cube, or divide each group of events in a bar whose height depends on the number of events.

•	View events only for selected: This toggle changes the rendering mode of the Gizmos. If it is not activated, each event will render their events, but if it is activated, only the GameObjects currently selected in the hierarchy window will render their events.

•	Y offset: Because GameObjects that include meshes can have their middle point inside, Y offset allows the user to move where the bars are drawn, in order to keep them outside of the GameObject’s model.

•	Y multiplier: This field lets the user set how the number of events will scale the height of the bars when viewing events separately

•	Size multiplier: Similar to how Y multiplier works, Size multiplier lets the user adjust the size of the cubes when viewing the events together.
 
Aside from the options in the GameObjects window, each SDVEventTracker component, which each GameObject that renders events has, has the option to change their specific Y offset, on top of the variable set in the window. This is done to let the users adjust this offset for GameObjects that have different sizes.

