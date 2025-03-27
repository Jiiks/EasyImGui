# EasyImGui
Super easy way to setup ImGui with OpenTK/OpenGL and Hexa.NET.ImGui

as simple as:

```cs
using EasyImGui;
using Hexa.NET.ImGui;
using OpenTK.Windowing.Common;

var mw = new MainWindow();
mw.Run();
mw.Dispose();

internal unsafe class MainWindow : EasyWindow {
    protected override void ImGuiRender(FrameEventArgs args) {
        ImGui.Begin("Hello");
        ImGui.Text("World!");
        ImGui.End();
    }
}
```

# Loading fonts:
```cs
// Load consolas from resources with sizes 16, 32 and 48.
EasyResourceMan.LoadFontFromResx(Resources.consolas, "consolas", 16, 32, 48); 
ImGui.PushFont(EasyResourceMan.GetFont("consolas", 16));
ImGui.Text("Consolas 16");
ImGui.PopFont();
````
# Loading images:
```cs
// Load image bytes from resources and create texture.
EasyResourceMan.CreateTexture2D(Resources.testimg, 123, 128, "test");
ImGui.Image(EasyResourceMan.GetTexture("test").Texture, new Vector2(64));
```


[Example](https://github.com/Jiiks/EasyImGui/blob/master/Example/Program.cs)


[OpenTK](https://github.com/opentk/opentk) | [Hexa.NET.ImGui](https://github.com/HexaEngine/Hexa.NET.ImGui)
