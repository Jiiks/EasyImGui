using EasyImGui;
using Example.Properties;
using Hexa.NET.ImGui;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Vector2 = System.Numerics.Vector2;


var nativeSettings = NativeWindowSettings.Default;
nativeSettings.TransparentFramebuffer = true;
var settings = GameWindowSettings.Default;

var mw = new MainWindow(settings, nativeSettings) {
    ClearColor = new Color4(.5f, .0f, .0f, .5f)
};


mw.Run();
mw.Dispose();

internal unsafe class MainWindow(GameWindowSettings s, NativeWindowSettings n) : EasyWindow(s,n) {

    protected override void OnLoad() {
        base.OnLoad();
        EasyResourceMan.LoadFontFromResx(Resources.consolas, "consolas", 16, 32, 48);
        EasyResourceMan.CreateTexture2D(Resources.testimg, 123, 128, "test");
    }

    protected override void ImGuiRender(FrameEventArgs args) {
        ImGui.Begin("Hello, World!");
        ImGui.PushFont(EasyResourceMan.GetFont("consolas", 16));
        ImGui.Text("Consolas 16");
        ImGui.PopFont();
        ImGui.PushFont(EasyResourceMan.GetFont("consolas", 32));
        ImGui.Text("Consolas 32");
        ImGui.PopFont();
        ImGui.PushFont(EasyResourceMan.GetFont("consolas", 48));
        ImGui.Text("Consolas 48");
        ImGui.PopFont();
        ImGui.Image(EasyResourceMan.GetTexture("test").Texture, new Vector2(64));
        ImGui.End();
        
    }
}
