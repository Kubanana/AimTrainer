using Silk.NET.Windowing;

public class Program
{
    static void Main()
    {
        var options = WindowOptions.DefaultVulkan;
        options.Size = new Silk.NET.Maths.Vector2D<int>(1280, 720);
        options.Title = "AimTrainer";

        var window = Window.Create(options);

        window.Load += () =>
        {
            Console.WriteLine("Window Created");  
        };

        window.Render += delta =>
        {
            
        };

        window.Run();
    }
}