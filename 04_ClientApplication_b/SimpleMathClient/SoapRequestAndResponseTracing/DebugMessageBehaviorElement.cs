using System;
using System.ServiceModel.Configuration;

namespace SoapRequestAndResponseTracing
{
    public class DebugMessageBehaviorElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new DebugMessageBehavior();
        }

        public override Type BehaviorType
        {
            get
            {
                return typeof(DebugMessageBehavior);
            }
        }
    }

}
