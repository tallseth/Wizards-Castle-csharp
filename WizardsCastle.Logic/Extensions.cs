using System.Linq;

namespace WizardsCastle.Logic
{
    public static class Extensions
    {
        public static T[] Add<T>(this T[] current, params T[] toAdd)
        {
            return current.Union(toAdd).ToArray();
        }

        public static T[] Without<T>(this T[] current, params T[] toRemove)
        {
            return current.Except(toRemove).ToArray();
        }
    }
}