using MediatR;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace DiamondShopSystem.BLL.Handlers.Vip.Queries
{
    public class GetAllVipsQueryHandler : IRequestHandler<GetAllVipsQuery, IEnumerable<VipDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllVipsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VipDto>> Handle(GetAllVipsQuery request, CancellationToken cancellationToken)
        {
            var vips = _unitOfWork.Repository<DAL.Entities.Vip>();
            return _mapper.Map<IEnumerable<VipDto>>(vips);
        }
    }
}
