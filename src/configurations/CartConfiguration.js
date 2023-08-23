const ParentConfiguration = require("./ParentConfiguration");

module.exports = {
  AddToCard: ParentConfiguration.Cart + "api/Card/AddToCard",
  GetAllCardDetails: ParentConfiguration.Cart + "api/Card/GetAllCardDetails",
  RemoveCartProduct: ParentConfiguration.Cart + "api/Card/RemoveCartProduct",
  OrderProduct: ParentConfiguration.Cart + "api/Card/OrderProduct",
  GetOrderProduct: ParentConfiguration.Cart + "api/Card/GetOrderProduct",
  GetAllOrderProduct:
    ParentConfiguration.Cart + "api/Card/GetAllOrderProduct",
  CancleOrder: ParentConfiguration.Cart + "api/Card/CancleOrder",
  PaymentGetway: ParentConfiguration.Cart + "api/Card/PaymentGetway",
  Rating: ParentConfiguration.Cart + "api/Card/Rating",
};
