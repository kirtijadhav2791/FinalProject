using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayers.Interface
{
    public interface ICardRL
    {
        //AddToCard, GetAllCardDetails, RemoveCartProduct,  OrderProduct
        public Task<AddToCardResponse> AddToCard(AddToCardRequest request);

        public Task<GetAllCardDetailsResponse> GetAllCardDetails(GetAllCardDetailsRequest request);

        public Task<RemoveCardResponse> RemoveCartProduct(RemoveCardRequest request);

        public Task<OrderProductResponse> OrderProduct(OrderProductRequest request);

        public Task<OrderProductResponse> CancleOrder(OrderProductRequest request);

        public Task<GetOrderProductResponse> GetOrderProduct(GetOrderProductRequest request);

        public Task<AddRatingResponse> AddRating(AddRatingRequest request);

        public Task<PaymentGetwayResponse> PaymentGetway(PaymentGetwayRequest request);
    }
}
