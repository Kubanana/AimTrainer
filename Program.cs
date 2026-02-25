using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using Silk.NET.Windowing;

public class Program
{
    private static Vk? _vk;
    private static Instance _instance;

    static unsafe void Main()
    {
        var options = WindowOptions.DefaultVulkan;
        options.Size = new Silk.NET.Maths.Vector2D<int>(1280, 720);
        options.Title = "AimTrainer";

        var window = Window.Create(options);

        window.Load += () =>
        {
            _vk = Vk.GetApi();

            var appInfo = new ApplicationInfo();
            appInfo.SType = StructureType.ApplicationInfo;
            appInfo.PApplicationName = (byte*)SilkMarshal.StringToPtr("AimTrainer");
            appInfo.ApplicationVersion = new Version32(1, 0, 0);
            appInfo.PEngineName = (byte*)SilkMarshal.StringToPtr("");
            appInfo.EngineVersion = new Version32(1, 0, 0);
            appInfo.ApiVersion = Vk.Version12;

            var createInfo = new InstanceCreateInfo();
            createInfo.SType = StructureType.InstanceCreateInfo;
            createInfo.PApplicationInfo = &appInfo;

            if (_vk.CreateInstance(&createInfo, null, out _instance) != Result.Success)
                throw new Exception("Failed to create Vulkan instance");

            Console.WriteLine("Created Vulkan instance");
        };

        window.Render += delta =>
        {
            
        };

        window.Run();
    }
}