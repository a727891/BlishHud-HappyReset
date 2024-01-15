using System.ComponentModel;

namespace HappyReset.Enum;

public enum ChestPosition
{
    [Description("Above the minimap - Top Left")]
    MINIMAP_TOP_LEFT,

    [Description("Above the minimap - Top Right")]
    MINIMAP_TOP_RIGHT,

    [Description("Inside the minimap - Top Left")]
    MINIMAP_INSIDE_TOP_LEFT,

    [Description("Inside the minimap - Top Right")]
    MINIMAP_INSIDE_TOP_RIGHT,

    [Description("Inside the minimap - Bottom Left")]
    MINIMAP_INSIDE_BOTTOM_LEFT,

    [Description("Inside the minimap - Bottom Right")]
    MINIMAP_INSIDE_BOTTOM_RIGHT,

    [Description("Bottom Right of the screen")]
    SCREEN_BOTTOM_RIGHT

}
