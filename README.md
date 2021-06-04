# UnityAnalyticsTest
 
 
 ## How to Use
 
 To try the Tool without having to create a videogame, we recommend you open the ExampleScene.
 
 - EventHandler
  
First, add an EventHandler Component to the scene. When selecting it in the inspector, we have the option to add events to the list. This list will be shared across scenes. In order for our heatmap to work, we can add an event and change its type to position. Then we will need to drag a gameobject to the target field. Once this is done, we can hit play and move the character around, in order to generate events. The events are stored in Unity's persistent datapath + /events folder.

- HeatMap

To view the heatmap, we will need to first add a HeatmapRenderer component to the scene. Then we can open the Heatmap window, located on Window->Tool->DataViz->HeatMap. Here, we will see a list with all the events we have created in the EventHandler. If we press load, the button will disappear and a View checkbox will appear. Before generating the heatmap, we need to select a material. The tool comes with an already created material, so the user can drag it to the field. Then, we will choose the gradient we want, and we press Generate Heatmap. Some cubes should appear in the scene, where events have been stored. The user can change many things, like the heatmap color, the size, if the amount of events should affect the size, or the shape of each heatcube. Some of this options arent shown instantly, and will need the user to regenerate the heatmap, ord move the camera/change the size of the scene window (it is a known bug).


