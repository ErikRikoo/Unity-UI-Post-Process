# UI Post Process - Unity
_A utility tool to apply postprocessing to world ui_

## Motivation
This is a utility tool needed for a [game](https://www.deadsigns.fr/) I work on with a team.
We needed some cool old tv effect on a world space canvas.
You can find the shader [there](https://github.com/ErikRikoo/Dead-Signs-Shaders).

## Installation
To install this package you can do one of this:
- Using Package Manager Window
    - Opening the Package Manager Window: Window > Package Manager
    - Wait for it to load
    - Click on the top left button `+` > Add package from git URL
    - Copy paste the [repository link](https://github.com/ErikRikoo/Unity-UI-Post-Process.git)
    - Press enter

- Modifying manifest.json file
Add the following to your `manifest.json` file (which is under your project location in `Packages` folder)
```json
{
  "dependancies": {
    ...
    "com.rikoo.ui-post-process": "https://github.com/ErikRikoo/Unity-UI-Post-Process.git",
    ...
  }
}
```

You can now use it and don't want to kill your designer/artist anymore. 

## Updating
Sometimes Unity has some hard time updating git dependencies so when you want to update the package, 
follow this steps:
- Go into `package-lock.json` file (same place that `manifest.json` one)
- It should look like this:
```json
{
  "dependencies": {
    ...
    "com.rikoo.ui-post-process": {
      "version": "https://github.com/ErikRikoo/Unity-UI-Post-Process.git",
      "depth": 0,
      "source": "git",
      "dependencies": {},
      "hash": "hash-number-there"
    },
    ...
}
```
- Remove the _"com.rikoo.ui-post-process"_ and save the file
- Get back to Unity
- Let him refresh
- Package should be updated

## How does it work
- Just drop the prefab in the Prefabs folder into you scene.
- Give it a post process material
- And then tweak the parameters

### Parameters
- **Settings**:
    - **Background Color**
    - **Dimension**
    - **Pixel Density**
    - **Scale**
    - **Post Process Material**
- **Components**: Just don't touch it if it comes from the prefab

### Creating canvas elements using *Editor Tools*
To add canvas elements to the canvas in the prefab, you should not do it directly.
Unity will try to keep the UI element at the same place in the scene so it will 
change position, rotation, scale and everything.
You should go under _Editor Tools_ and then drop an object into the object field.
It needs to be an object from the scene and not a child of the UI Post Process.

There is something else to note is that you should use relative positions for your UI elements
(anchors and so on). If it is not the case, just tick the check box and it will compute
directly the anchors for you.

Finally to move the object just press the button.

## TODO
- [ ] Applying multiple post process shaders
- [ ] Remove the dependency to a material instance

 
## Known issue
- When you do an "Export Package" action, the render texture is kind of discarded and you
will just have the post process applied on a completely white texture.

## Suggestions
Feel free to suggest features by creating an issue, any idea is welcome !
