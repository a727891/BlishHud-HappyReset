using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace HappyReset.Utils;

public static class FlowPanelExtensions
{
    public static void VisiblityChanged(this FlowPanel panel, SettingEntry<bool> setting)
    {
        setting.SettingChanged += (_, e) =>
        {
            panel.Visible = e.NewValue;
            panel.Parent?.Invalidate();
        };
        
        panel.Visible = setting.Value;
        panel.Parent?.Invalidate();
    }
    
    public static void InvertedVisiblityChanged(this FlowPanel panel, SettingEntry<bool> setting)
    {
        setting.SettingChanged += (_, e) =>
        {
            panel.Visible = !e.NewValue;
            panel.Parent?.Invalidate();
        };
        
        panel.Visible = !setting.Value;
        panel.Parent?.Invalidate();
    }



    public static FlowPanel BeginFlow(this FlowPanel panel, Container parent, Point sizeOffset, Point locationOffset)
    {
        panel.FlowDirection = ControlFlowDirection.SingleTopToBottom;
        panel.OuterControlPadding = new Vector2(20, 25);
        panel.Parent = parent;
        panel.Size = parent.Size + sizeOffset;
        panel.ShowBorder = true;
        panel.Location += locationOffset;
        
        return panel;
    }
    
    public static FlowPanel BeginFlow(this FlowPanel panel, Container parent)
    {
        return BeginFlow(panel, parent, new Point(0), new Point(0));
    }

    public static FlowPanel AddSetting(this FlowPanel panel, SettingEntry setting)
    {
        var viewContainer = new ViewContainer
        {
            Parent = panel,
        };


        viewContainer.Show(SettingView.FromType(setting, panel.Width));
        
        
        return panel;
    }
    
    public static FlowPanel AddSetting(this FlowPanel panel, IEnumerable<SettingEntry>? settings)
    {
        if (settings is null) return panel;
        
        foreach (var setting in settings)
        {
            panel.AddSetting(setting);
        }
        return panel;
    }

    


    public static FlowPanel AddSpace(this FlowPanel panel)
    {
        var _ = new ViewContainer
        {
            Parent = panel,
        };
        return panel;
    }

    public static FlowPanel AddString(this FlowPanel panel, string text)
    {
        var _ = new Label
        {
            Parent = panel,
            AutoSizeWidth = true,
            AutoSizeHeight = true,
            Text = text,
            WrapText = false,
            Location = new Point(25, 0),
        };

        return panel;
    }
    public static FlowPanel AddChildPanel(this FlowPanel panel, Panel child)
    {
        child.Parent = panel;

        return panel;
    }
    public static FlowPanel Indent(this FlowPanel panel)
    {
        panel.Left = 30;
        return panel;
    }

    public static FlowPanel AddFlowControl(this FlowPanel panel, Control control, out Control generatedControl)
    {
        control.Parent = panel;

        panel.AddChild(control);
        generatedControl = control;
        return panel;
    }

    public static FlowPanel AddFlowControl(this FlowPanel panel, Control control)
    {
        control.Parent = panel;

        panel.AddChild(control);
        return panel;
    }
}
