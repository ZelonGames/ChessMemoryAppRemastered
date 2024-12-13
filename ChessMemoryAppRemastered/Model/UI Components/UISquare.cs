using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.UI_Components;

public class UISquare
{
    public enum HighlightType
    {
        Red,
        Green,
        None,
    }

    public static readonly Color white = Color.FromArgb("#f0d9b5");
    public static readonly Color black = Color.FromArgb("#b58863");
    
    public HighlightType _HighlightType {get; private set;}
    public Color _Color { get; private set; }
    public readonly ContentView contentView;
    public readonly Color initialColor;

    public UISquare(ContentView contentView, Color color)
    {
        this.contentView = contentView;
        initialColor = _Color = color;
        this.contentView.BackgroundColor = color;
        _HighlightType = HighlightType.None;
    }

    public void SetHighlight(HighlightType highlightType)
    {
        _HighlightType = highlightType;
        switch (_HighlightType)
        {
            case HighlightType.Red:
                contentView.BackgroundColor = _Color = GetRedVariant();
                break;
            case HighlightType.Green:
                contentView.BackgroundColor = _Color = GetGreenVariant();
                break;
            case HighlightType.None:
                contentView.BackgroundColor = _Color = initialColor;
                break;
            default:
                break;
        }
    }

    public Color GetRedVariant()
    {
        return new Color(
            Math.Min(1, initialColor.Red + 0.5f),
            Math.Max(0, initialColor.Green - 0.3f),
            Math.Max(0, initialColor.Blue - 0.3f),
            initialColor.Alpha);
    }

    public Color GetGreenVariant()
    {
        return new Color(
            Math.Max(0, initialColor.Red - 0.3f),
            Math.Min(1, initialColor.Green + 0.5f),
            Math.Max(0, initialColor.Blue - 0.3f),
            initialColor.Alpha);
    }
}
