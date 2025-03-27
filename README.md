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

internal unsafe class MainWindow() : EasyWindow {
    protected override void ImGuiRender(FrameEventArgs args) {
        ImGui.Begin("Hello");
        ImGui.Text("World!");
        ImGui.End();
    }
}
```

[OpenTK](https://github.com/opentk/opentk) | [Hexa.NET.ImGui](https://github.com/HexaEngine/Hexa.NET.ImGui)
