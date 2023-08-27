using Blish_HUD.Controls;

namespace HappyReset.Utils;

public static class ContainerExtensions
{
    public static Container AddControl(this Container container, Control control, out Control generatedControl)
    {
        control.Parent = container;

        container.AddChild(control);
        generatedControl = control;
        return container;
    }

    public static Container AddControl(this Container container, Control control)
    {
        control.Parent = container;

        container.AddChild(control);
        return container;
    }
}