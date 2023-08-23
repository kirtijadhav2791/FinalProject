import React, { useState, useEffect } from "react";
import "./GetProduct.css";
import ProductServices from "../../services/ProductServices";
import CartServices from "../../services/CartServices";
import WishListServices from "../../services/WishListServices";

import Card from "@material-ui/core/Card";
import CardActionArea from "@material-ui/core/CardActionArea";
import CardActions from "@material-ui/core/CardActions";
import CardContent from "@material-ui/core/CardContent";
import CardMedia from "@material-ui/core/CardMedia";
import Button from "@material-ui/core/Button";
import Typography from "@material-ui/core/Typography";
import Pagination from "@material-ui/lab/Pagination";
import DeleteIcon from "@material-ui/icons/Delete";
import ArchiveIcon from "@material-ui/icons/Archive";
import Snackbar from "@material-ui/core/Snackbar";
import CloseIcon from "@material-ui/icons/Close";
import IconButton from "@material-ui/core/IconButton";
import RestoreIcon from "@material-ui/icons/Restore";
import DeleteForeverIcon from "@material-ui/icons/DeleteForever";
import Backdrop from "@material-ui/core/Backdrop";
import CircularProgress from "@material-ui/core/CircularProgress";
import Rating from "@material-ui/lab/Rating";
import Modal from "@material-ui/core/Modal";
import Fade from "@material-ui/core/Fade";
import Radio from "@material-ui/core/Radio";
import RadioGroup from "@material-ui/core/RadioGroup";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import TextField from "@material-ui/core/TextField";

const productServices = new ProductServices();
const cartServices = new CartServices();
const wishlistServices = new WishListServices();

export default function GetProduct(props) {
  const [Message, setMessage] = useState("");
  const [OpenSnackBar, setOpenSnackBar] = useState(false);
  const [OpenLoader, setOpenLoader] = useState(false);
  const [RatingValue, setRatingValue] = useState(1);
  const [OpenPaymentGetwayModel, setOpenPaymentGetwayModel] = useState(false);
  const [datas, setdata] = useState({
    ProductName: "",
    ProductType: "",
    ProductPrice: "",
    CartID: 0,
    ProductCompany: "",
    ProductDetails: "",
    ProductImageUrl: "",
    PaymentMode: "card",
    Card: "",
    UPI: "",
  });

  const [Flag, setFlag] = useState({
    PaymentMode: false,
  });

  useEffect(() => {
    console.log("GetProduct Use Effect Calling");
    console.log("Data : ", props.List);
  }, []);

  const ProductMoveToArchive = async (productID) => {
    setOpenLoader(true);
    let data = {
      productID: productID,
    };
    await productServices
      .ProductMoveToArchive(data)
      .then((data) => {
        console.log("ProductMoveToArchive Data : ", data);
        setMessage(data.data.message);
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.productServices(props.PageNumber);
      })
      .catch((error) => {
        console.log("ProductMoveToArchive Error : ", error);
        setMessage("Something Went Wrong");
        setOpenSnackBar(true);
        setOpenLoader(false);
      });
  };

  const ProductMoveToTrash = async (productID) => {
    setOpenLoader(true);
    let data = {
      productID: productID,
    };
    await productServices
      .ProductMoveToTrash(data)
      .then((data) => {
        console.log("ProductMoveToTrash Data : ", data);
        setMessage(data.data.message);
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.productServices(props.PageNumber);
      })
      .catch((error) => {
        console.log("ProductMoveToTrash Error : ", error);
        setMessage("Something Went Wrong");
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.productServices(props.PageNumber);
      });
  };

  const ProductRestore = async (productID) => {
    setOpenLoader(true);
    let data = {
      productID: productID,
    };
    productServices
      .ProductRestore(data)
      .then((data) => {
        console.log("ProductRestore Data : ", data);
        setMessage(data.data.message);
        setOpenSnackBar(true);
        setOpenLoader(false);
        {
          props.State === "Trash"
            ? props.GetTrashList(props.PageNumber)
            : props.GetArchiveList(props.PageNumber);
        }
      })
      .catch((error) => {
        console.log("ProductRestore Error : ", error);
        setMessage("Something Went Wrong");
        setOpenSnackBar(true);
        setOpenLoader(false);
      });
  };

  const TrashProductRestore = async (productID) => {
    setOpenLoader(true);
    let data = {
      productID: productID,
    };
    productServices
      .TrashProductRestore(data)
      .then((data) => {
        console.log("ProductRestore Data : ", data);
        setMessage(data.data.message);
        setOpenSnackBar(true);
        setOpenLoader(false);
        {
          props.State === "Trash"
            ? props.GetTrashList(props.PageNumber)
            : props.GetArchiveList(props.PageNumber);
        }
      })
      .catch((error) => {
        console.log("ProductRestore Error : ", error);
        setMessage("Something Went Wrong");
        setOpenSnackBar(true);
        setOpenLoader(false);
      });
  };

  const ProductDeletePermenently = async (productID) => {
    setOpenLoader(true);

    productServices
      .ProductDeletePermenently(productID)
      .then((data) => {
        console.log("ProductDeletePermenently Data : ", data);
        setMessage(data.data.message);
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.productServices(props.PageNumber);
      })
      .catch((error) => {
        console.log("ProductDeletePermenently Error : ", error);
        setMessage("Something Went Wrong");
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.productServices(props.PageNumber);
      });
  };

  const handleSnackBarClose = (event, reason) => {
    if (reason === "clickaway") {
      return;
    }

    setOpenSnackBar(false);
  };

  const AddToCart = async (ProductID) => {
    setOpenLoader(true);
    let data = {
      productID: ProductID,
      userID: Number(localStorage.getItem("Customer_UserID")),
    };
    await cartServices
      .AddToCard(data)
      .then((data) => {
        console.log("AddToCart Data : ", data);
        setMessage(data.data.message);
        setOpenSnackBar(true);
        setOpenLoader(false);
      })
      .catch((error) => {
        console.log("AddToCart Data : ", error);
        setMessage("Something Went Wrong");
        setOpenSnackBar(true);
        setOpenLoader(false);
      });
  };

  const AddToWishList = async (ProductID) => {
    let data = {
      productID: ProductID,
      userID: localStorage.getItem("Customer_UserID"),
    };
    setOpenLoader(true);
    wishlistServices
      .AddToWishList(data)
      .then((data) => {
        console.log("AddToWishList Data :", data);
        setMessage(data.data.message);
        setOpenSnackBar(true);
        setOpenLoader(false);
      })
      .catch((error) => {
        console.log("AddToWishList Error : ", error);
        setMessage("Something Went Wrong");
        setOpenSnackBar(true);
        setOpenLoader(false);
      });
    setOpenLoader(true);
  };

  const MoveToCart = async (ProductID, WishListID) => {
    let data = {
      productID: ProductID,
      userID: localStorage.getItem("Customer_UserID"),
      wishListID: WishListID,
    };

    setOpenLoader(true);
    wishlistServices
      .MoveToCard(data)
      .then((data) => {
        console.log("MoveToCart Data :", data);
        setMessage(data.data.message);
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.GetAllWishListDetails(props.PageNumber);
      })
      .catch((error) => {
        console.log("MoveToCart Error : ", error);
        setMessage("Something Went Wrong");
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.GetAllWishListDetails(props.PageNumber);
      });
    setOpenLoader(true);
  };

  const RemoveWishList = async (WishListID) => {
    setOpenLoader(true);
    wishlistServices
      .RemoveWishListProduct(WishListID)
      .then((data) => {
        console.log("RemoveWishList Data :", data);
        setMessage(data.data.message);
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.GetAllWishListDetails(props.PageNumber);
      })
      .catch((error) => {
        console.log("RemoveWishList Error : ", error);
        setMessage("Something Went Wrong");
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.GetAllWishListDetails(props.PageNumber);
      });
    setOpenLoader(true);
  };

  const OrderProduct = async (CartID, ProductID) => {
    setOpenLoader(true);
    let data = {
      cartID: CartID,
      productID: ProductID,
    };
    await cartServices
      .OrderProduct(data)
      .then((data) => {
        console.log("OrderProduct Data : ", data);
        setMessage(data.data.message);
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.GetAllCardDetails(props.PageNumber);
      })
      .catch((error) => {
        console.log("OrderProduct Data : ", error);
        setMessage("Something Went Wrong");
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.GetAllCardDetails(props.PageNumber);
      });
  };

  const CancleOrder = async (CartID, ProductID) => {
    setOpenLoader(true);
    let data = {
      cartID: CartID,
      productID: ProductID,
    };
    await cartServices
      .CancleOrder(data)
      .then((data) => {
        console.log("CancleOrder Data : ", data);
        setMessage(data.data.message);
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.GetMyOrderList(props.PageNumber);
      })
      .catch((error) => {
        console.log("CancleOrder Data : ", error);
        setMessage("Something Went Wrong");
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.GetMyOrderList(props.PageNumber);
      });
  };

  const RemoveCart = async (CartID) => {
    setOpenLoader(true);

    await cartServices
      .RemoveCartProduct(CartID)
      .then((data) => {
        console.log("RemoveCartProduct Data : ", data);
        setMessage(data.data.message);
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.GetAllCardDetails(props.PageNumber);
      })
      .catch((error) => {
        console.log("RemoveCartProduct Data : ", error);
        setMessage("Something Went Wrong");
        setOpenSnackBar(true);
        setOpenLoader(false);
        props.GetAllCardDetails(props.PageNumber);
      });
  };

  const OpenPaymentModel = (data) => {
    debugger
    console.log("Data : ", data);
    setdata({
      ...datas,
      ProductName: data.productName,
      ProductType: data.productType,
      ProductPrice: data.productPrice,
      CartID: data.cartID,
      ProductCompany: data.productCompany,
      ProductDetails: data.productDetails,
      ProductImageUrl: data.productImageUrl,
    });
    setRatingValue(data.rating);
    setOpenPaymentGetwayModel(true);
  };

  const handleClose = () => {
    setOpenPaymentGetwayModel(false);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    console.log("Value : ", e.target.value);
    setdata({ ...datas, [name]: value });
  };

  const handleConfirmBook = async () => {
    console.log("handleConfirmBook Calling .... ");
    //debugger;
    setFlag({
      PaymentMode: false,
    });
    if (datas.PaymentMode === "upi" && datas.UPI === "") {
      setFlag({
        PaymentMode: true,
      });
      setOpenSnackBar(true);
      setMessage("Enter Required Filled");

      return;
    } else if (datas.PaymentMode === "card" && datas.Card === "") {
      setFlag({
        PaymentMode: true,
      });
      setOpenSnackBar(true);
      setMessage("Enter Required Filled");
      return;
    }
    setOpenLoader(true);
    debugger
    await handleProductRating(RatingValue, datas.CartID);

    let data = {
      cartNo: datas.CartID,
      upiid: datas.UPI,
      paymentType: datas.PaymentMode,
      cardNo: datas.Card,
    };
    cartServices
      .PaymentGetway(data)
      .then((data) => {
        // debugger;
        console.log("PaymentGetway Data : ", data);
        setOpenSnackBar(true);
        setOpenLoader(false);
        setMessage("Payment Successfully");
        setOpenPaymentGetwayModel(false);
        setdata({
          ProductName: "",
          ProductType: "",
          ProductPrice: "",
          CartID: 0,
          ProductCompany: "",
          ProductDetails: "",
          ProductImageUrl: "",
          PaymentMode: "card",
          Card: "",
          UPI: "",
        });
        props.GetMyOrderList(props.PageNumber);
      })
      .catch((error) => {
        // debugger;
        console.log("PaymentGetway Error : ", error);
        setOpenSnackBar(true);
        setOpenLoader(false);
        setOpenPaymentGetwayModel(false);
        setMessage("Something Went Wrong");
        setdata({
          ProductName: "",
          ProductType: "",
          ProductPrice: "",
          CartID: 0,
          ProductCompany: "",
          ProductDetails: "",
          ProductImageUrl: "",
          PaymentMode: "card",
          Card: "",
          UPI: "",
        });
        props.GetMyOrderList(props.PageNumber);
      });
  };

  const handleProductRating = async (RatingValue, CartID) => {
    debugger;
    console.log("Rating : ", RatingValue, " Cart ID : ", CartID);
    let data = {
      cartID: CartID,
      rating: RatingValue,
    };

    await cartServices
      .Rating(data)
      .then((data) => {
        // debugger;
        console.log("Rating Data : ", data);
        setOpenSnackBar(true);
        // setOpenLoader(false);
        setMessage("Rating Apply Successfully");
        props.GetMyOrderList(props.PageNumber);
      })
      .catch((error) => {
        console.log("Rating Error : ", error);
        setOpenSnackBar(true);
        // setOpenLoader(false);
        setMessage("Something Went Wrong");
        props.GetMyOrderList(props.PageNumber);
      });
  };

  return (
    <div className="GetProduct-Container">
      <div className="GetProduct-SubContainer">
        <div className="HomePage-SubContainer-Body-SubBody1">
          {Array.isArray(props.List) && props.List.length > 0
            ? props.List.map(function (data, index) {
              console.log("Get Product Data : ", data);
              return (
                <>
                  <Card
                    className=""
                    style={{ maxWidth: 350, margin: 15 }}
                    key={index}
                  >
                    <CardActionArea>
                      <CardMedia
                        style={{ height: 180, width: 260, margin: "0 auto" }}
                        image={data.productImageUrl}
                        title="Contemplative Reptile"
                      />

                      <CardContent
                        style={{
                          width: 228,
                          height: 130,
                          margin: "0 auto",
                        }}
                      >
                        <Typography gutterBottom variant="h5" component="h2">
                          {data.productName}
                        </Typography>
                        <Typography
                          variant="body2"
                          color="textSecondary"
                          component="p"
                          style={{
                            height: 80,
                            textOverflow: "ellipsis",
                            overflow: "hidden",
                          }}
                        >
                          {data.productDetails}
                        </Typography>
                        <Typography
                          variant="body2"
                          color="textSecondary"
                          component="p"
                          style={{
                            height: 40,
                            display: "flex",
                            justifyContent: "center",
                            alignItems: "center",
                            fontWeight: 600,
                            color: "blue",
                          }}
                        >
                          {props.State === "Home" ||
                            props.State === "Archive" ||
                            props.State === "UserHome" ||
                            props.State === "Trash" ||
                            props.State === "WishList" ? (
                            <>
                              {data.quantity !== 0 ? (
                                <>Available : {data.quantity}</>
                              ) : (
                                <>Not Available</>
                              )}
                            </>
                          ) : null}{" "}
                          &nbsp; &nbsp;{" "}
                          {props.State !== "MyOrder" ? (
                            <>Price : {data.productPrice} &#8377;</>
                          ) : null}
                        </Typography>
                      </CardContent>
                    </CardActionArea>
                    <CardActions
                      style={{
                        display: "flex",
                        justifyContent: "space-around",
                      }}
                    >
                      {props.State === "Home" ? (
                        <>
                          <Button
                            size="small"
                            color="primary"
                            onClick={() => {
                              ProductMoveToArchive(data.productID);
                            }}
                          >
                            <ArchiveIcon style={{ color: "black" }} />
                          </Button>
                          <Button
                            size="small"
                            color="primary"
                            onClick={() => {
                              ProductMoveToTrash(data.productID);
                            }}
                          >
                            <DeleteIcon style={{ color: "black" }} />
                          </Button>
                        </>
                      ) : props.State === "Archive" ? (
                        <Button
                          size="small"
                          color="primary"
                          onClick={() => {
                            ProductRestore(data.productID);
                          }}
                        >
                          <RestoreIcon style={{ color: "black" }} />
                        </Button>
                      ) : props.State === "UserHome" ? (
                        <>
                          {data.quantity !== 0 ? (
                            <Button
                              size="small"
                              color="primary"
                              style={{
                                fontSize: 12,
                                fontWeight: 700,
                                color: "black",
                              }}
                              onClick={() => {
                                AddToCart(data.productID);
                              }}
                            >
                              &nbsp;Add To Cart
                            </Button>
                          ) : (
                            <></>
                          )}
                          <Button
                            size="small"
                            color="primary"
                            style={{
                              fontSize: 12,
                              fontWeight: 700,
                              color: "red",
                            }}
                            onClick={() => {
                              AddToWishList(data.productID);
                            }}
                          >
                            &nbsp;Add To WishList
                          </Button>
                        </>
                      ) : props.State === "Trash" ? (
                        <>
                          <Button
                            size="small"
                            color="primary"
                            onClick={() => {
                              TrashProductRestore(data.productID);
                            }}
                          >
                            <RestoreIcon style={{ color: "black" }} />
                          </Button>
                          <Button
                            size="small"
                            color="primary"
                            onClick={() => {
                              ProductDeletePermenently(data.productID);
                            }}
                          >
                            <DeleteForeverIcon style={{ color: "black" }} />
                          </Button>
                        </>
                      ) : props.State === "Cart" ? (
                        <>
                          {data.quantity !== 0 ? (
                            <Button
                              size="small"
                              color="primary"
                              style={{
                                fontSize: 12,
                                fontWeight: 700,
                                color: "black",
                              }}
                              onClick={() => {
                                OrderProduct(data.cartID, data.productID);
                              }}
                            >
                              &nbsp;Order Product
                            </Button>
                          ) : (
                            <></>
                          )}
                          <Button
                            size="small"
                            color="primary"
                            style={{
                              fontSize: 12,
                              fontWeight: 700,
                              color: "red",
                            }}
                            onClick={() => {
                              RemoveCart(data.cartID);
                            }}
                          >
                            &nbsp;Remove Cart
                          </Button>
                        </>
                      ) : props.State === "WishList" ? (
                        <>
                          {data.quantity !== 0 ? (
                            <Button
                              size="small"
                              color="primary"
                              onClick={() => {
                                MoveToCart(data.productID, data.wishListID);
                              }}
                              style={{
                                fontSize: 12,
                                fontWeight: 700,
                                color: "black",
                              }}
                            >
                              Move To Cart
                            </Button>
                          ) : (
                            <></>
                          )}
                          <Button
                            size="small"
                            color="primary"
                            onClick={() => {
                              RemoveWishList(data.wishListID);
                            }}
                            style={{
                              fontSize: 12,
                              fontWeight: 700,
                              color: "red",
                            }}
                          >
                            Remove WishList
                          </Button>
                        </>
                      ) : props.State === "MyOrder" ? (
                        <>
                          <Rating
                            name="simple-controlled"
                            value={data.rating}
                          />
                          {!data.isPayment ? (
                            <Button
                              size="small"
                              color="primary"
                              onClick={() => {
                                OpenPaymentModel(data);
                              }}
                              style={{
                                fontSize: 12,
                                fontWeight: 700,
                                color: "red",
                              }}
                            >
                              Payment
                            </Button>
                          ) : (
                            <></>
                          )}
                          <Button
                            size="small"
                            color="primary"
                            onClick={() => {
                              CancleOrder(data.cartID, data.productID);
                            }}
                            style={{
                              fontSize: 12,
                              fontWeight: 700,
                              color: "black",
                            }}
                          >
                            Cancle Order
                          </Button>
                        </>
                      ) : null}
                    </CardActions>
                  </Card>
                  {/* ) : (
                      <></>
                    )} */}
                </>
              );
            })
            : null}
        </div>
        <div className="HomePage-SubContainer-Body-SubBody2">
          <Pagination
            count={props.TotalPages}
            Page={props.PageNumber}
            onChange={props.handlePaging}
            variant="outlined"
            shape="rounded"
            color="secondary"
          />
        </div>
      </div>

      <Modal
        open={OpenPaymentGetwayModel}
        onClose={() => {
          handleClose();
        }}
        closeAfterTransition
        BackdropComponent={Backdrop}
        BackdropProps={{
          timeout: 500,
        }}
        className="Model-Create-Ticket"
      >
        <Fade in={OpenPaymentGetwayModel}>
          <div className="Model-Open-Getway-Main">
            <div className="Model-Create-Ticket-Header">Product Detail</div>
            <div className="Model-Open-Getway-Body">
              <img
                src={datas.ProductImageUrl}
                alt="Product Image"
                style={{ height: 200, width: 200 }}
              />
              <div style={{ display: "flex" }}>
                <div className="Model-Create-Ticket-Body-Row">
                  Order ID :
                  <div style={{ color: "red" }}>&nbsp;{datas.CartID}</div>
                </div>
                <div className="Model-Create-Ticket-Body-Row">
                  Product Name :{" "}
                  <div style={{ color: "red" }}>&nbsp;{datas.ProductName}</div>
                </div>
              </div>
              <div style={{ display: "flex" }}>
                <div className="Model-Create-Ticket-Body-Row">
                  Product Price :{" "}
                  <div style={{ color: "red" }}>
                    &nbsp;
                    {datas.ProductPrice} &#8377;
                  </div>
                </div>
                <div className="Model-Create-Ticket-Body-Row">
                  Product Type :&nbsp;{" "}
                  <div style={{ color: "red" }}>{datas.ProductType}</div>
                </div>
              </div>
              <div style={{ display: "flex" }}>
                <div className="Model-Create-Ticket-Body-Row">
                  Product Comapny :{" "}
                  <div style={{ color: "red" }}>
                    &nbsp;{datas.ProductCompany}
                  </div>
                </div>
              </div>
              <div style={{ display: "flex" }}>
                <div className="Model-Create-Ticket-Body-Row">
                  Rating : &nbsp;
                  <Rating
                    name="RatingValue"
                    value={RatingValue}
                    onChange={(event, newValue) => {
                      // debugger;
                      console.log(
                        "New Value : ",
                        newValue,
                        " Cart ID : ",
                        datas.CartID
                      );
                      setRatingValue(newValue);
                    }}
                  />
                </div>
              </div>
              <div
                style={{ display: "flex" }}
                className="Model-Create-Ticket-Body-Row"
              >
                Payment Mode :
                <RadioGroup
                  name="PaymentMode"
                  style={{
                    display: "flex",
                    flexDirection: "row",
                    margin: "0 0 0 20px",
                  }}
                  value={datas.PaymentMode}
                  onChange={handleChange}
                >
                  <FormControlLabel
                    value="card"
                    control={<Radio />}
                    label="Card"
                  />
                  <FormControlLabel
                    value="cash"
                    control={<Radio />}
                    label="Cash"
                  />
                  <FormControlLabel
                    value="upi"
                    control={<Radio />}
                    label="UPI"
                  />
                </RadioGroup>
              </div>
              <div className="CartDetail">
                {datas.PaymentMode !== "cash" ? (
                  <TextField
                    variant="outlined"
                    size="small"
                    label={
                      datas.PaymentMode === "card"
                        ? "Card Number"
                        : "UPI Number"
                    }
                    name={datas.PaymentMode === "card" ? "Card" : "UPI"}
                    style={{ margin: "5px 0 20px 0" }}
                    error={Flag.PaymentMode}
                    value={
                      datas.PaymentMode === "card" ? datas.Card : datas.UPI
                    }
                    onChange={handleChange}
                  />
                ) : null}
              </div>
            </div>
            <div className="Model-Open-Getway-Footer">
              <Button
                // variant="contained"
                style={{ margin: "10px" }}
                onClick={() => {
                  handleClose();
                }}
              >
                Cancel
              </Button>
              <Button
                variant="contained"
                color="primary"
                style={{
                  backgroundColor: "#ff0000",
                  width: 100,
                  margin: "0px 0 0 10px",
                  color: "white",
                }}
                onClick={() => {
                  handleConfirmBook();
                }}
              >
                Book
              </Button>
            </div>
          </div>
        </Fade>
      </Modal>

      <Backdrop style={{ zIndex: "1", color: "#fff" }} open={OpenLoader}>
        <CircularProgress color="inherit" />
      </Backdrop>
      <Snackbar
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "left",
        }}
        open={OpenSnackBar}
        autoHideDuration={2000}
        onClose={handleSnackBarClose}
        message={Message}
        action={
          <React.Fragment>
            <Button
              color="secondary"
              size="small"
              onClick={handleSnackBarClose}
            >
              UNDO
            </Button>
            <IconButton
              size="small"
              aria-label="close"
              color="inherit"
              onClick={handleSnackBarClose}
            >
              <CloseIcon fontSize="small" />
            </IconButton>
          </React.Fragment>
        }
      />
    </div>
  );
}
