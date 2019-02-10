using System;
using Moq.Language.Flow;

namespace WizardsCastle.Logic.Tests.Helpers
{
    public class MoqSequence
    {
        private int expectedSteps_;
        private int actualSteps_;

        public ISetup<T> InSequence<T>(ISetup<T> iSetup) where T : class
        {
            var expectedStep = expectedSteps_++;

            iSetup.Callback(() => CheckStep(expectedStep));

            return iSetup;
        }

        public ISetup<T, U> InSequence<T, U>(ISetup<T, U> iSetup) where T : class
        {
            var expectedStep = expectedSteps_++;

            iSetup.Callback(() => CheckStep(expectedStep));

            return iSetup;
        }

        public void Verify()
        {
            if (expectedSteps_ != actualSteps_)
                throw new SequenceException("Expected " + expectedSteps_ + " steps but only got " + actualSteps_ + " steps");
        }

        private void CheckStep(int expectedStep)
        {
            if (actualSteps_ != expectedStep)
            {
                int expectedStep1 = expectedStep + 1;
                int actualStep = actualSteps_ + 1;
                throw new SequenceException("Step " + expectedStep1 + " executed when step " + actualStep + " should have been.");
            }

            actualSteps_++;
        }

        private class SequenceException : Exception
        {
            public SequenceException(string message) : base(message) { }
        }
    }
}