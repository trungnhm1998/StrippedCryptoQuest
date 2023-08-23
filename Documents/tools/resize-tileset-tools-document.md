# Resize tileset in Unit to optimize

Right click on the root folder of the tileset asset you want to resize and select this option

![How to resize](img/resize.png)

After that please check all the collider in scene
There's might be a case that the polygon collider being shifted down a little bit 
If you found 1 polygon collider has this issue, select this option to fix it

![How to fix collider](img/fixCollider.png)

If there's 1 polygon correct then all the polygon is correct and you dont have to fix it
If you accidently fix the collider, you can reset by select this

![How to reset fix collider](img/resetfixCollider.png)

If there're many scene using the same tilemap, you should open those scene to check whether the collider is correct and fix it

![Multiple tilemap](img/multipleTimeMap.png)

This is the result, please smoke test after resize

![Result](img/result.png)

Further optimize:

You can change the atlas size to fit with new size
For example the size is 704x992 now so you only need atlas with 1024x1024 size to contains all of it 

![Change atlas size](img/atlasSize.png)


Problems left:

Tilemap PPU not match, but I don't see any issue cause by this

![Tilemap PPU not match](img/problem.png)
