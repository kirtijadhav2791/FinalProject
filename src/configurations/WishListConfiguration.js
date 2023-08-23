const ParentConfiguration = require("./ParentConfiguration");

module.exports = {
  AddToWishList: ParentConfiguration.Wishlist + "api/WishList/AddToWishList",
  GetAllWishListDetails: ParentConfiguration.Wishlist + "api/WishList/GetAllWishListDetails",
  RemoveWishListProduct: ParentConfiguration.Wishlist + "api/WishList/RemoveWishListProduct",
  MoveToCard: ParentConfiguration.Wishlist + "api/WishList/MoveToCard",
};
