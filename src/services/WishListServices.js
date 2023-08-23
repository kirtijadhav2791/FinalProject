import AxiosServices from "./AxiosServices";
import WishListConfiguration from "../configurations/WishListConfiguration";

const axiosServices = new AxiosServices();

export default class WishListServices{
  AddToWishList(data) {
    return axiosServices.post(WishListConfiguration.AddToWishList, data, false);
  }

  GetAllWishListDetails(data) {
    return axiosServices.post(
      WishListConfiguration.GetAllWishListDetails,
      data,
      false
    );
  }
  RemoveWishListProduct(data) {
    return axiosServices.DeleteWishList(
      WishListConfiguration.RemoveWishListProduct,
      data,
      false
    );
  }
  MoveToCard(data) {
    return axiosServices.post(WishListConfiguration.MoveToCard, data, false);
  }
}
