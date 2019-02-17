using System;

namespace WizardsCastle.Logic.Data
{
    [Flags]
    internal enum Curses
    {
        None = 0,
        CurseOfLethargy = 1,
        CurseOfTheLeech = 2,
        CurseOfForgetfulness = 4
    }
}