using System.Linq;

namespace WizardsCastle.Logic
{
    public static class Extensions
    {
        public static T[] Add<T>(this T[] current, params T[] toAdd)
        {
            return current.Union(toAdd).ToArray();
        }
    }
}