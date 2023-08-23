const ParentConfiguration = require("./ParentConfiguration");

module.exports = {
  AddProduct: ParentConfiguration.Product + "api/Product/AddProduct",
  GetAllProduct: ParentConfiguration.Product + "api/Product/GetAllProduct",
  GetProductByID: ParentConfiguration.Product + "api/Product/GetProductByID",
  GetProductByName: ParentConfiguration.Product + "api/Product/GetProductByName",
  UpdateProduct: ParentConfiguration.Product + "api/Product/UpdateProduct",
  ProductMoveToArchive:
    ParentConfiguration.Product + "api/Product/ProductMoveToArchive",
  GetArchiveProduct:
    ParentConfiguration.Product + "api/Product/GetArchiveProduct",
  ProductMoveToTrash:
    ParentConfiguration.Product + "api/Product/ProductMoveToTrash",
  GetTrashProduct: ParentConfiguration.Product + "api/Product/GetTrashProduct",
  ProductDeletePermenently:
    ParentConfiguration.Product + "api/Product/ProductDeletePermenently",
  ProductRestore: ParentConfiguration.Product + "api/Product/ProductRestore",
  TrashProductRestore:
    ParentConfiguration.Product + "api/Product/ProductRestore",
  SearchProduct: ParentConfiguration.Product + "api/Product/SearchProduct?SearchKeyword=",
};
