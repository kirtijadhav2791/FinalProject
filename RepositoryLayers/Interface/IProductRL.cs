using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayers.Interface
{
    public interface IProductRL
    {
        //AddProduct, GetAllProduct, GetProductByID, GetProductByName, UpdateProduct
        //ProductMoveToArchive, GetArchiveProduct, ProductMoveToTrash, GetTrashProduct,
        //ProductDeletePermenently, ProductRestore
        public Task<AddProductResponse> AddProduct(AddProductRequest request);

        public Task<GetAllProductResponse> GetAllProduct(GetAllProductRequest request);

        public Task<GetProductByIDResponse> GetProductByID(GetProductByIDRequest request);

        public Task<GetProductByNameResponse> GetProductByName(GetProductByNameRequest request);

        public Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request);

        public Task<ProductMoveToArchiveResponse> ProductMoveToArchive(ProductMoveToArchiveRequest request);

        public Task<GetArchiveProductResponse> GetArchiveProduct(GetAllProductRequest request);

        public Task<ProductMoveToArchiveResponse> ProductMoveToTrash(ProductMoveToArchiveRequest request);

        public Task<GetArchiveProductResponse> GetTrashProduct(GetAllProductRequest request);

        public Task<ProductMoveToArchiveResponse> ProductDeletePermenently(ProductMoveToArchiveRequest request);

        public Task<ProductMoveToArchiveResponse> ProductRestore(ProductMoveToArchiveRequest request);
  
    }
}
