using System;

namespace Pr.Core.Interfaces
{
    public interface ISubscription
    {
        Uri Uri { get; }
    }

    public class Subscription : ISubscription, IEquatable<ISubscription>
    {
        
        public Subscription(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; }

        public bool Equals(ISubscription other)
        {
            return other != null && Uri == other.Uri;
        }

        public override bool Equals(object other)
        {
            return Equals(other as ISubscription);
        }

        public override int GetHashCode()
        {
            return Uri?.GetHashCode() ?? 0;
        }
    }
}
