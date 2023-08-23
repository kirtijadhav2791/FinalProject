using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayers.Interface
{
    public interface IWishListRL
    {
        //AddToWishList, GetAllWishListDetails, RemoveWishListProduct, MoveToCard
        public Task<AddToWishListResponse> AddToWishList(AddToWishListRequest request);

        public Task<GetAllWishListDetailsResponse> GetAllWishListDetails(GetAllWishListDetailsRequest request);

        public Task<RemoveWishListProductResponse> RemoveWishListProduct(RemoveWishListProductRequest request);

        public Task<MoveToCardResponse> MoveToCard(MoveToCardRequest request);

    }
}
