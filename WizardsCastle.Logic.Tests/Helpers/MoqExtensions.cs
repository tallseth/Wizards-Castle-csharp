using Moq.Language.Flow;

namespace WizardsCastle.Logic.Tests.Helpers
{
    public static class MoqExtensions
    {
        public static IReturnsResult<TMock> ReturnsNull<TMock, TResult>(this ISetup<TMock, TResult> setup) 
            where TMock : class 
            where TResult : class
        {
            return setup.Returns((TResult) null);
        }
    }
}