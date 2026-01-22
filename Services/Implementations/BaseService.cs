using AutoMapper;
using Artify.Api.Repositories.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public abstract class BaseService
    {
        protected readonly IMapper _mapper;
        protected readonly IBuyerRepository _buyerRepository;
        protected readonly IOrderRepository _orderRepository;
        protected readonly IReviewRepository _reviewRepository;

        protected BaseService(
            IMapper mapper,
            IBuyerRepository buyerRepository,
            IOrderRepository orderRepository = null,
            IReviewRepository reviewRepository = null)
        {
            _mapper = mapper;
            _buyerRepository = buyerRepository;
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
        }

        protected async Task<bool> UserExistsAsync(Guid userId)
        {
            var user = await _buyerRepository.GetBuyerByIdAsync(userId);
            return user != null;
        }
    }
}