
using System.ServiceModel;

namespace IBatisNet.DataMapper.SessionStore
{
    public class WcfSessionItemsInstanceExtension : IExtension<InstanceContext>
    {
        public static WcfSessionItemsInstanceExtension Current
        {
            get
            {
                if (OperationContext.Current==null)
                {
                    return null;
                }
                InstanceContext context = OperationContext.Current.InstanceContext;
                return GetCollectionFrom(context);
            }
        }

        private static WcfSessionItemsInstanceExtension GetCollectionFrom(InstanceContext context)
        {
            lock (context)
            {
                var extension = context.Extensions.Find<WcfSessionItemsInstanceExtension>();
                if (extension==null)
                {
                    extension = new WcfSessionItemsInstanceExtension();
                    extension.Items.Hook(context);
                }
                return extension;
            }
        }
        public InstanceItems Items = new InstanceItems();
        public void Attach(InstanceContext owner)
        {
            // intentionally do nothing
        }

        public void Detach(InstanceContext owner)
        {
            // intentionally do nothing
        }
    }
}
