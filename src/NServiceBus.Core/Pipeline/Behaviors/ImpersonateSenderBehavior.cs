﻿namespace NServiceBus.Pipeline.Behaviors
{
    using System;
    using System.Threading;
    using Impersonation;

    class ImpersonateSenderBehavior : IBehavior<PhysicalMessageContext>
    {
        public ExtractIncomingPrincipal ExtractIncomingPrincipal { get; set; }

        public void Invoke(PhysicalMessageContext context, Action next)
        {
            var principal = ExtractIncomingPrincipal.GetPrincipal(context.PhysicalMessage);

            if (principal == null)
            {
                next();
                return;
            }

            var previousPrincipal = Thread.CurrentPrincipal;
            try
            {
                Thread.CurrentPrincipal = principal;
                next();
            }
            finally
            {
                Thread.CurrentPrincipal = previousPrincipal;
            }
        }
    }
}