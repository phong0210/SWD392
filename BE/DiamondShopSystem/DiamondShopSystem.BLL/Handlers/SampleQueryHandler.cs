using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers
{
    // Sample Query
    public class GetHelloQuery : IRequest<string> { }

    // Sample Handler
    public class GetHelloQueryHandler : IRequestHandler<GetHelloQuery, string>
    {
        public Task<string> Handle(GetHelloQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult("Hello from MediatR!");
        }
    }
} 