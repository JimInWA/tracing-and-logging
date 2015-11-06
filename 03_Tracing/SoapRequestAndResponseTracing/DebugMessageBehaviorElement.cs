namespace SoapRequestAndResponseTracing
{
    using System;
    using System.ServiceModel.Configuration;

    /// <summary>
    /// DebugMessageBehaviorElement class - extends BehaviorExtensionElement abstract class
    /// </summary>
    public class DebugMessageBehaviorElement : BehaviorExtensionElement
    {
        /// <summary>
        /// CreateBehavior method, overrides base method
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new DebugMessageBehavior();
        }

        /// <summary>
        /// BehaviorType public property, overrides base property
        /// </summary>
        /// <returns></returns>
        public override Type BehaviorType
        {
            get
            {
                return typeof(DebugMessageBehavior);
            }
        }
    }

}
