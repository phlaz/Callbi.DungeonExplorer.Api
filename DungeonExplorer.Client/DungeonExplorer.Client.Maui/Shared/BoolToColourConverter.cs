using System.Globalization;

namespace DungeonExplorer.Client.Maui.Shared;

public class BoolToColourConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isWall = (bool)value;
        return isWall ? Colors.Black : Colors.White;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}