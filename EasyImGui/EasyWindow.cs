using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Backends.GLFW;
using Hexa.NET.ImGui.Backends.OpenGL3;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace EasyImGui;
/// <inheritdoc/>
public unsafe class EasyWindow(GameWindowSettings? gameWindowSettings = null, NativeWindowSettings? nativeWindowSettings = null) : 
    GameWindow(gameWindowSettings??GameWindowSettings.Default, nativeWindowSettings??NativeWindowSettings.Default) {

    /// <summary>
    /// GLFW pointer of Window
    /// </summary>
    public GLFWwindow* GLFWPtr => (GLFWwindow*)WindowPtr;

    public Color4 ClearColor = new Color4(.0f, .0f, .0f, .0f);
    public ClearBufferMask ClearBufferMask = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit;
    public string GlslVersion = "#version 150";

    protected override void OnLoad() {
        ImGui.SetCurrentContext(ImGui.CreateContext());
        ImGuiImplGLFW.SetCurrentContext(ImGui.GetCurrentContext());
        ImGuiImplGLFW.InitForOpenGL(GLFWPtr, true);
        ImGuiImplOpenGL3.SetCurrentContext(ImGui.GetCurrentContext());
        ImGuiImplOpenGL3.Init(GlslVersion);
    }

    protected override void OnRenderFrame(FrameEventArgs args) {
        if (IsExiting) return;
        GL.ClearColor(ClearColor);
        GL.Clear(ClearBufferMask);
        Render(args);

        ImGuiImplOpenGL3.NewFrame();
        ImGuiImplGLFW.NewFrame();
        ImGui.NewFrame();

        ImGuiRender(args);
        ImGui.Render();
        ImGui.EndFrame();
        ImGuiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());
        ImGui.UpdatePlatformWindows();
        ImGui.RenderPlatformWindowsDefault();
        SwapBuffers();
        PostRender(args);
    }

    /// <summary>
    /// Called after clearing
    /// </summary>
    protected virtual void Render(FrameEventArgs args) { }
    /// <summary>
    /// Called after setting up ImGui frame
    /// </summary>
    protected virtual void ImGuiRender(FrameEventArgs args) { }
    /// <summary>
    /// Called post frame render and <see cref="GameWindow.SwapBuffers"/>
    /// </summary>
    protected virtual void PostRender(FrameEventArgs args) { }

}
