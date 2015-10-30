There are 4 scripts that are useful for highlighting: Highlighting Effect, Camera Targeting, Highlighting Controller & Highlightable Object.

Highlighting Effect, Camera Targeting, these 2 scripts are attached with camera.


Highlighting Controller is attacted with the object that will be highlighted when the mouse hovers on it.
Highlighting Controller will add Highlightable Object script to this game object as a component at runtime.


Highlightable Object script is a little bit bizarre. The good thing is it has some really helpful comments by the author to help understand this script.
Try to digest it slowly.

Reading the doc in HighlightingSystemDemo folder will definitely help.

There are some unnecessary features in the 4 scripts.
For example, if you click the object then the object is gonna start to flash.

What we want to have for our game is just the plain highlighting system, no click-flashing.
Thus first, try to get rid of/commment out those unnecessary features and only leave the ones we need, which will also help you understand the scripts.
Then next, try to make it somehow look better and fit in our game style.

This task is given to Sammy and Ryan.

Wei