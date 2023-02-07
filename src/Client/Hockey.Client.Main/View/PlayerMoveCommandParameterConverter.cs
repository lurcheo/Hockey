using Hockey.Client.Main.Model.Data;
using Hockey.Client.Main.ViewModel;
using Hockey.Client.Shared.Converter;
using System;
using System.Globalization;

namespace Hockey.Client.Main.View;

internal class PlayerMoveCommandParameterConverter : TwoValuesConverterBase<PlayerMoveCommandParameterConverter, int, PlayerInfo>
{
    public override object Convert(int first, PlayerInfo second, Type targetType, object parameter, CultureInfo culture)
    {
        return new PlayerMoveCommandParameter(first, second);
    }
}
