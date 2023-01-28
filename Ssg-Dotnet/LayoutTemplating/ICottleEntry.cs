using Cottle;

namespace Ssg_Dotnet.LayoutTemplating;

internal interface ICottleEntry
{
    (Value, Value) ToCottleContext();
}
