import AxiosServices from "./AxiosServices";
import CartConfiguration from "../configurations/CartConfiguration";

const axiosServices = new AxiosServices();

export default class CartServices {
  AddToCard(data) {
    return axiosServices.post(CartConfiguration.AddToCard, data, false);
  }

  GetAllCardDetails(data) {
    return axiosServices.post(CartConfiguration.GetAllCardDetails, data, false);
  }

  RemoveCartProduct(data) {
    return axiosServices.DeleteCart(
      CartConfiguration.RemoveCartProduct,
      data,
      false
    );
  }

  OrderProduct(data) {
    return axiosServices.post(CartConfiguration.OrderProduct, data, false);
  }

  GetOrderProduct(data) {
    return axiosServices.post(CartConfiguration.GetOrderProduct, data, false);
  }

  GetAllOrderProduct(data) {
    return axiosServices.post(CartConfiguration.GetAllOrderProduct, data, false);
  }

  CancleOrder(data) {
    return axiosServices.Patch(CartConfiguration.CancleOrder, data, false);
  }

  PaymentGetway(data) {
    return axiosServices.Patch(CartConfiguration.PaymentGetway, data, false);
  }

  Rating(data) {
    return axiosServices.Patch(CartConfiguration.Rating, data, false);
  }
}
