# SolutionIcon

![](SolutionIcon/Screenshot.png)

SolutionIcon is an extension that adds solution-related overlay icon to VS taskbar button.

### Use case

I often have multiple Visual Studio instances open.  
I never combine taskbar buttons as it makes switching inefficient — however this means that I do not get previews either.  

This extension allows to quickly distinguish VS buttons with a glance.

### Icon logic

The solution icon is chosen as following:

1. First choice: .editoricon.png (or .ico, .gif, etc) under solution root.  
This is inspired by [.editorconfig](http://editorconfig.org/), however I am not aware of anyone using .editoricon before.

2. Second choice: certain images within the projects.  
This is very limited at the moment, but e.g. favicon.ico might be recognized.

3. Third choice: if no applicable icon is found in solution, a new icon is generated.  
The algorithm uses one or two letters from the solution name, and a color based on solution path.

### Requirements
Supports Visual Studio 2010 - 2015.

### See also

[Visual Studio Gallery](https://visualstudiogallery.msdn.microsoft.com/cebfde58-4395-4372-a94f-c400dd0aa125)
