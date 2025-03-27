using EasyImGui;
using Hexa.NET.ImGui;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

var nativeSettings = NativeWindowSettings.Default;
nativeSettings.TransparentFramebuffer = true;
var settings = GameWindowSettings.Default;

var mw = new MainWindow(settings, nativeSettings) {
    ClearColor = new Color4(.5f, .0f, .0f, .5f)
};
mw.Run();
mw.Dispose();

internal unsafe class MainWindow(GameWindowSettings s, NativeWindowSettings n) : EasyWindow(s,n) {
    protected override void ImGuiRender(FrameEventArgs args) {
        ImGui.Begin("Hello");
        ImGui.Text("World!");
        ImGui.End();
    }
}
