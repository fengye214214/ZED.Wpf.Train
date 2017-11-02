using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ZED.IVMS7200
{
    public static class IVMCommObjectExtension
    {
        private struct DisposableCommunicationObjectToken : IDisposable
        {
            private readonly ICommunicationObject client;

            internal DisposableCommunicationObjectToken(ICommunicationObject obj)
            {
                // The obj is not null.
                Debug.Assert(obj != null);

                // Store the obj.
                this.client = obj;
            }

            public void Dispose()
            {
                // If the obj is null, throw an exception.
                if (client == null)
                {
                    // Throw the exception.
                    throw new InvalidOperationException(
                        "The DisposableCommunicationObjectToken structure " +
                        "was created with the default constructor.");
                }

                // Try to close.
                try
                {
                    // Close.
                    client.Close();
                }
                catch (CommunicationException)
                {
                    // Abort if there is a communication exception.
                    client.Abort();
                }
                catch (TimeoutException)
                {
                    // Abort if there is a timeout exception.
                    client.Abort();
                }
                catch (Exception)
                {
                    // Abort in the face of any other exception.
                    client.Abort();

                    // Rethrow.
                    throw;
                }
            }
        }

        public static IDisposable AsDisposable(this ICommunicationObject obj)
        {
            // Return a new instance of the DisposableCommunicationObjectToken.
            return new DisposableCommunicationObjectToken(obj);
        }
    }
}
