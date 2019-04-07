public static T If<T>(this T obj, bool condition, Action<T> action)
    where T : Cake.Core.Tooling.ToolSettings
{
    if(condition)
    {
        action(obj);
    }
    return obj;
}