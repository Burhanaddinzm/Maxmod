using Braintree;

namespace Maxmod.Services.Interfaces;

public interface IBraintreeService
{
    IBraintreeGateway CreateGateway();
    IBraintreeGateway GetGateway();
}
