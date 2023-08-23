import AxiosServices from "./AxiosServices";
import ProductConfiguration from "../configurations/ProductConfiguration";

const axiosServices = new AxiosServices();

export default class ProductServices {
  AddProduct(data) {
    return axiosServices.post(ProductConfiguration.AddProduct, data, false);
  }
  GetAllProduct(data) {
    return axiosServices.post(ProductConfiguration.GetAllProduct, data, false);
  }
  GetProductByID(data) {
    return axiosServices.post(ProductConfiguration.GetProductByID, data, false);
  }

  GetProductByName(data) {
    return axiosServices.post(
      ProductConfiguration.GetProductByName,
      data,
      false
    );
  }
  UpdateProduct(data) {
    return axiosServices.post(ProductConfiguration.UpdateProduct, data, false);
  }
  ProductMoveToArchive(data) {
    return axiosServices.Patch(
      ProductConfiguration.ProductMoveToArchive,
      data,
      false
    );
  }
  GetArchiveProduct(data) {
    return axiosServices.post(
      ProductConfiguration.GetArchiveProduct,
      data,
      false
    );
  }
  ProductMoveToTrash(data) {
    return axiosServices.Patch(
      ProductConfiguration.ProductMoveToTrash,
      data,
      false
    );
  }
  GetTrashProduct(data) {
    return axiosServices.post(
      ProductConfiguration.GetTrashProduct,
      data,
      false
    );
  }
  ProductDeletePermenently(data) {
    return axiosServices.Delete(
      ProductConfiguration.ProductDeletePermenently,
      data,
      false
    );
  }
  ProductRestore(data) {
    return axiosServices.Patch(
      ProductConfiguration.ProductRestore,
      data,
      false
    );
  }

  TrashProductRestore(data) {
    return axiosServices.Patch(
      ProductConfiguration.TrashProductRestore,
      data,
      false
    );
  }

  SearchProduct(data) {
    return axiosServices.Get(ProductConfiguration.SearchProduct + data, false);
  }
}
