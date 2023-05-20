﻿namespace MARDEK.CharacterSystem
{
    internal interface IPortrait
    {
        public PortraitType PortraitType { get; }
        public void SetPortrait(CharacterPortrait portrait);
        public void SetExpression(PortraitExpressionEnum expression);
    }
}
