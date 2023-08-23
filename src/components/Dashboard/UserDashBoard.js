import React, { Component } from "react";

import "./UserDashBoard.css";
import GetProduct from "../Product/GetProduct";

import FeedbackServices from "../../services/FeedbackServices";
import ProductServices from "../../services/ProductServices";
import CartServices from "../../services/CartServices";
import WishListServices from "../../services/WishListServices";
import AuthServices from "../../services/AuthServices";

import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import Button from "@material-ui/core/Button";
import IconButton from "@material-ui/core/IconButton";
import HomeIcon from "@material-ui/icons/Home";
import FeedbackIcon from "@material-ui/icons/Feedback";
import Modal from "@material-ui/core/Modal";
import Backdrop from "@material-ui/core/Backdrop";
import Fade from "@material-ui/core/Fade";
import CircularProgress from "@material-ui/core/CircularProgress";
import Snackbar from "@material-ui/core/Snackbar";
import CloseIcon from "@material-ui/icons/Close";
import TextField from "@material-ui/core/TextField";
import PaymentIcon from "@material-ui/icons/Payment";
import SearchIcon from "@material-ui/icons/Search";
import InputBase from "@material-ui/core/InputBase";
import ShoppingCartIcon from "@material-ui/icons/ShoppingCart";
import BookmarkIcon from "@material-ui/icons/Bookmark";
import ShopIcon from "@material-ui/icons/Shop";

import InputLabel from "@material-ui/core/InputLabel";
import MenuItem from "@material-ui/core/MenuItem";
import FormControl from "@material-ui/core/FormControl";
import Select from "@material-ui/core/Select";

import TreeView from "@material-ui/lab/TreeView";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import TreeItem from "@material-ui/lab/TreeItem";
import Label from "@material-ui/icons/Label";
import SupervisorAccountIcon from "@material-ui/icons/SupervisorAccount";

import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableContainer from "@material-ui/core/TableContainer";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import Paper from "@material-ui/core/Paper";
import Settings from "@material-ui/icons/Settings";

const feedbackServices = new FeedbackServices();
const productServices = new ProductServices();
const cartServices = new CartServices();
const wishlistServices = new WishListServices();
const authServices = new AuthServices();

const MobileRegex = RegExp(/^[0-9]{11}$/i);
const PinCodeRegex = RegExp(/^[0-9]{7}$/i);
const EmailRegex = RegExp(
  /^(([^<>()[\].,;:\s@"]+(\.[^<>()[\].,;:\s@"]+)*)|(".+"))@(([^<>()[\].,;:\s@"]+.)+[^<>()[\].,;:\s@"]{2,})$/i
);

export default class UserDashBoard extends Component {
  constructor(props) {
    super(props);
    this.state = {
      //
      SearchProduct: "",
      FullName: "",
      MobileNumber: "",
      EmailID: "",
      //
      FullNameFlag: false,
      MobileNumberFlag: false,
      EmailIDFlag: false,
      //
      Address1: "",
      Address2: "",
      City: "",
      Distict: "",
      State: "",
      Country: "",
      Pincode: "",
      //
      Address1Flag: false,
      CityFlag: false,
      DistictFlag: false,
      StateFlag: false,
      CountryFlag: false,
      PincodeFlag: false,
      //
      Product: [],
      //
      FeedBack: "",
      FeedBackFlag: false,
      //
      Message: "",
      //
      NumberOfRecordPerPage: 6,
      //
      PageNumber: 1,
      //
      TotalPages: 0,
      TotalRecords: 0,

      Open: false, // Flag For Open Feedback
      OpenLoader: false,
      OpenSnackBar: false,

      Update: false,
      UpdateAddress: false,
      UpdateDetail: false,

      OpenUserHome: true,
      OpenMyOrder: false,
      OpenCard: false,
      OpenWishList: false,
      OpenCustomerSetting: false,
      OpenUserDetail: false,
      OpenUserAddress: false,
    };
  }

  async componentWillMount() {
    console.log("Component will mount calling ... ");

    this.setState({
      OpenUserHome:
        localStorage.getItem("OpenUserHome") === "true" ? true : false,
      OpenMyOrder:
        localStorage.getItem("OpenMyOrder") === "true" ? true : false,
      OpenCard: localStorage.getItem("OpenCard") === "true" ? true : false,
      OpenWishList:
        localStorage.getItem("OpenWishList") === "true" ? true : false,
      OpenCustomerSetting:
        localStorage.getItem("OpenCustomerSetting") === "true" ? true : false,
      OpenUserDetail:
        localStorage.getItem("OpenUserDetail") === "true" ? true : false,
      OpenUserAddress:
        localStorage.getItem("OpenUserAddress") === "true" ? true : false,
    });

    if (localStorage.getItem("OpenUserHome") === "true") {
      await this.productServices(this.state.PageNumber);
    } else if (localStorage.getItem("OpenMyOrder") === "true") {
      await this.GetMyOrderList(this.state.PageNumber);
    } else if (localStorage.getItem("OpenCard") === "true") {
      await this.GetAllCardDetails(this.state.PageNumber);
    } else if (localStorage.getItem("OpenWishList") === "true") {
      await this.GetAllWishListDetails(this.state.PageNumber);
    }

    await this.GetCustomerDetail(
      Number(localStorage.getItem("Customer_UserID"))
    );
    await this.GetCustomerAdderess(
      Number(localStorage.getItem("Customer_UserID"))
    );
  }

  SearchProduct = async () => {
    console.log("productServices Calling ... ");

    if (this.state.SearchProduct === "") {
      this.setState({
        OpenSnackBar: true,
        Message: "Please Enter Search Keyword",
      });
      return;
    }
    this.setState({ OpenLoader: true, Product: [] });
    await productServices
      .SearchProduct(this.state.SearchProduct)
      .then((data) => {
        console.log("SearchProduct Data : ", data);
        if (data.data.data !== null) {
          this.setState({
            Product: data.data.data.reverse(),
            TotalPages: 0,
            PageNumber: data.data.currentPage,
          });
        }
        this.setState({
          OpenLoader: false,
          OpenSnackBar: true,
          Message: data.data.message,
        });
      })
      .catch((error) => {
        console.log("SearchProduct Error : ", error);
        this.setState({ OpenLoader: false });
      });
  };

  //
  productServices = async (CurrentPage) => {
    console.log("Get Jobs List Calling ... ");
    let data = {
      pageNumber: CurrentPage,
      numberOfRecordPerPage: 4,
    };
    this.setState({ OpenLoader: true, Product: [] });
    await productServices
      .GetAllProduct(data)
      .then((data) => {
        if (data.data.data === null && CurrentPage > 1) {
          this.setState({ PageNumber: CurrentPage - 1 })
          this.GetAllProduct(CurrentPage - 1)
        } else {
          this.setState({
            Product: data.data.data,
            TotalPages: data.data.totalPage,
            OpenLoader: false,
            OpenSnackBar: true,
            Message: data.data.message,
          });
        }
      })
      .catch((error) => {
        console.log("GetAllProduct Error : ", error);
        this.setState({ OpenLoader: false });
      });
  };

  //
  GetMyOrderList = async (CurrentPage) => {
    console.log("Get My Order List Calling ... ");
    let data = {
      pageNumber: CurrentPage,
      numberOfRecordPerPage: 3,
      userID: Number(localStorage.getItem("Customer_UserID")),
    };
    this.setState({ OpenLoader: true, Product: [] });
    await cartServices
      .GetOrderProduct(data)
      .then((data) => {
        console.log("GetMyOrderList Data : ", data);
        // debugger;
        if (data.data.data === null && CurrentPage > 1) {
          this.setState({ PageNumber: CurrentPage - 1 })
          this.GetMyOrderList(CurrentPage - 1)
        } else {
          this.setState({
            Product: data.data.data,
            TotalPages: data.data.totalPage,
            OpenLoader: false,
            OpenSnackBar: true,
            Message: data.data.message,
          });
        }
      })

      .catch((error) => {

        console.log("GetMyOrderList Error : ", error);
        this.setState({
          OpenLoader: false,
          OpenSnackBar: true,
          Message: "Something went wrong.",
        });
      });
  };

  //
  GetAllCardDetails = async (CurrentPage) => {
    console.log("Get All Card Details Calling ... ");
    let data = {
      pageNumber: CurrentPage,
      numberOfRecordPerPage: 4,
      userID: Number(localStorage.getItem("Customer_UserID")),
    };
    this.setState({ OpenLoader: true, Product: [] });
    await cartServices
      .GetAllCardDetails(data)
      .then((data) => {
        console.log("GetAllCardDetails Data : ", data);

        if (data.data.data === null && CurrentPage > 1) {
          this.setState({ PageNumber: CurrentPage - 1 })
          this.GetAllCardDetails(CurrentPage - 1)
        } else {
          this.setState({
            Product: data.data.data,
            TotalPages: data.data.totalPage,
            OpenLoader: false,
            OpenSnackBar: true,
            Message: data.data.message,
          });
        }
      })
      .catch((error) => {
        console.log("GetAllCardDetails Error : ", error);
        this.setState({ OpenLoader: false });
      });
  };

  //
  GetAllWishListDetails = async (CurrentPage) => {
    console.log("Get All Wish List Details Calling ... ");
    let data = {
      pageNumber: CurrentPage,
      numberOfRecordPerPage: 4,
      userID: Number(localStorage.getItem("Customer_UserID")),
    };
    this.setState({ OpenLoader: true, Product: [] });
    await wishlistServices
      .GetAllWishListDetails(data)
      .then((data) => {
        console.log("GetAllWishListDetails Data : ", data);

        if (data.data.data === null && CurrentPage > 1) {
          this.setState({ PageNumber: CurrentPage - 1 })
          this.GetAllWishListDetails(CurrentPage - 1)
        } else {
          this.setState({
            Product: data.data.data,
            TotalPages: data.data.totalPage,
            OpenLoader: false,
            OpenSnackBar: true,
            Message: data.data.message,
          });
        }
      })
      .catch((error) => {
        console.log("GetAllWishListDetails Error : ", error);
        this.setState({
          OpenLoader: false,
          OpenSnackBar: true,
          Message: "Something Went Wrong",
        });
      });
  };

  GetCustomerDetail = async (UserID) => {
    console.log("GetCustomerDetail Calling....");
    await authServices
      .GetCustomerDetail(UserID)
      .then((data) => {
        console.log("GetCustomerDetail Data : ", data);
        if (!data.data.isSuccess) {
          this.setState({
            Message: data.data.message,
            OpenSnackBar: true,
          });
          this.handleOpenUserAddress();
          return;
        }
        if (data.data.data !== null) {
          this.setState({
            FullName: data.data.data.fullName,
            EmailID: data.data.data.emailID,
            MobileNumber: data.data.data.mobileNumber,
            Message: data.data.message,
            OpenSnackBar: true,
            UpdateDetail: true,
          });
        } else {
          this.setState({ UpdateDetail: false });
        }
      })
      .catch((error) => {
        console.log("GetCustomerDetail Error : ", error);
      });
  };

  GetCustomerAdderess = async (UserID) => {
    console.log("GetCustomerAdderess Calling....");

    await authServices
      .GetCustomerAdderess(UserID)
      .then((data) => {
        console.log("GetCustomerAdderess Data : ", data);
        if (!data.data.isSuccess) {
          this.setState({
            Message: data.data.message,
            OpenSnackBar: true,
          });
          this.handleOpenUserAddress();
          return;
        }
        if (data.data.data !== null) {
          this.setState({
            Address1: data.data.data.address1,
            Address2: data.data.data.address2,
            City: data.data.data.city,
            Distict: data.data.data.distict,
            State: data.data.data.state,
            Country: data.data.data.country,
            Pincode: data.data.data.pincode,
            Message: data.data.message,
            OpenSnackBar: true,
            UpdateAddress: true,
          });
        } else {
          this.setState({ UpdateAddress: false });
        }
      })
      .catch((error) => {
        console.log("GetCustomerAdderess Error : ", error);
      });
  };

  handleClose = () => {
    console.log("Handle Close Calling ...");
    this.setState({
      Open: false,
      Update: false,
    });
  };

  handleFeedOpen = () => {
    this.setState({ Open: !this.state.Open });
  };

  handleChanges = (e) => {
    const { name, value } = e.target;

    this.setState(
      { [name]: value },
      console.log("Name : ", name, " value : ", value)
    );
  };

  handleSubmitUserDetail = () => {
    let state = this.state;
    this.CheckUserDetailValidation();
    if (
      state.FullNameFlag === false &&
      state.MobileNumberFlag === false &&
      state.EmailIDFlag === false
    ) {
      console.log("Acceptable");
      let data = {
        isUpdate: state.UpdateDetail,
        userID: localStorage.getItem("Customer_UserID"),
        userName: localStorage.getItem("Customer_UserName"),
        fullName: state.FullName,
        emailID: state.EmailID,
        mobileNumber: state.MobileNumber,
      };
      authServices
        .AddCustomerDetail(data)
        .then((data) => {
          console.log("AddCustomerDetail Data : ", data);
          this.setState({
            OpenSnackBar: true,
            Message: data.data.message,
          });
        })
        .catch((error) => {
          console.log("AddCustomerDetail Error : ", error);
          this.setState({
            OpenSnackBar: true,
            Message: "Something Went Wrong",
          });
        });
    } else {
      console.log("Please Fill Required Field");
    }
  };

  handleSubmitAddress = () => {
    let state = this.state;
    this.CheckValidation();
    if (
      state.Address1Flag === false &&
      state.CityFlag === false &&
      state.DistictFlag === false &&
      state.StateFlag === false &&
      state.CountryFlag === false &&
      state.PincodeFlag === false
    ) {
      console.log("Acceptable");
      let data = {
        isUpdate: state.UpdateAddress,
        userID: localStorage.getItem("Customer_UserID"),
        address1: state.Address1,
        address2: state.Address2,
        city: state.City,
        distict: state.Distict,
        state: state.State,
        country: state.Country,
        pincode: state.Pincode,
      };
      authServices
        .AddCustomerAdderess(data)
        .then((data) => {
          console.log("handleSubmitAddress Data :", data);
          this.setState({
            OpenSnackBar: true,
            Message: data.data.message,
          });
          // localStorage.setItem("UserAddress", "true");
        })
        .catch((error) => {
          console.log("handleSubmitAddress Error : ", error);
          this.setState({
            OpenSnackBar: true,
            Message: "Something Went Wrong",
          });
        });
    } else {
      console.log("Please Fill Required Field");
    }
  };

  CheckUserDetailValidation = () => {
    console.log("CheckUserDetailValidation Calling....");
    let state = this.state;
    this.setState({
      FullNameFlag: false,
      MobileNumberFlag: false,
      EmailIDFlag: false,
    });

    if (state.FullName === "") {
      this.setState({ FullNameFlag: true });
    }

    if (state.MobileNumber === "") {
      this.setState({ MobileNumberFlag: true });
    }

    if (state.EmailID === "") {
      this.setState({ EmailIDFlag: true });
    }
  };

  CheckValidation = () => {
    console.log("Check Validation Calling....");
    let state = this.state;
    this.setState({
      Address1Flag: false,
      CityFlag: false,
      DistictFlag: false,
      StateFlag: false,
      CountryFlag: false,
      PincodeFlag: false,
    });

    if (state.Address1 === "") {
      this.setState({ Address1Flag: true });
    }

    if (state.City === "") {
      this.setState({ CityFlag: true });
    }

    if (state.Distict === "") {
      this.setState({ DistictFlag: true });
    }

    if (state.State === "") {
      this.setState({ StateFlag: true });
    }

    if (state.Country === "") {
      this.setState({ CountryFlag: true });
    }

    if (state.Pincode === "") {
      this.setState({ PincodeFlag: true });
    }
  };

  handleChangeContact = (e) => {
    const { name, value } = e.target;
    console.log("Regex Match : ", MobileRegex.test(value));
    if (!MobileRegex.test(value)) {
      this.setState({ ContactFlag: true });
    } else {
      this.setState({ ContactFlag: false });
    }
    //

    if (value.toString().length <= 10) {
      this.setState(
        { [name]: value },
        console.log("Name : ", name, "Value : ", value)
      );
      if (value.toString().length === 10) {
        this.setState({ ContactFlag: false });
      }
    }
  };

  handleChangeEmail = (e) => {
    const { name, value } = e.target;
    console.log("Regex Match : ", EmailRegex.test(value));
    if (!EmailRegex.test(value)) {
      this.setState({ EmailIDFlag: true });
    } else {
      this.setState({ EmailIDFlag: false });
    }
    this.setState(
      { [name]: value },
      console.log("Name : ", name, "Value : ", value)
    );
  };

  handleChangePinCode = (e) => {
    const { name, value } = e.target;
    console.log("Regex Match : ", PinCodeRegex.test(value));
    if (!PinCodeRegex.test(value)) {
      this.setState(
        { [name]: value },
        console.log("Name : ", name, "Value : ", value)
      );
    }
  };

  handleSnackBarClose = (event, reason) => {
    if (reason === "clickaway") {
      return;
    }

    this.setState({ OpenSnackBar: false });
  };

  handleSubmitFeedback = async (e) => {
    this.setState({ FeedBackFlag: false, OpenLoader: true });

    if (this.state.FeedBack) {
      let data = {
        feedback: this.state.FeedBack,
        userId: Number(localStorage.getItem("Customer_UserID")),
      };

      await feedbackServices
        .AddFeedback(data)
        .then((data) => {
          console.log("Feedback Data : ", data);
          this.setState({
            OpenSnackBar: true,
            FeedBackFlag: false,
            Open: false,
            OpenLoader: false,
            Message: data.data.message,
          });
        })
        .catch((error) => {
          console.log("Feedback Error : ", error);
          this.setState({
            OpenSnackBar: true,
            FeedBackFlag: false,
            Open: false,
            OpenLoader: false,
            Message: "Something Went Wrong.",
          });
        });
    } else {
      this.setState({
        OpenSnackBar: true,
        FeedBackFlag: true,
        Message: "Please Fill FeedBack",
      });
    }
  };

  handlePaging = async (e, value) => {
    let state = this.state;
    console.log("Current Page : ", value);

    this.setState({
      PageNumber: value,
    });

    if (state.OpenUserHome) {
      await this.productServices(value);
    } else if (state.OpenCard) {
      await this.GetAllCardDetails(value);
    } else if (state.OpenMyOrder) {
      await this.GetMyOrderList(value);
    } else if (state.OpenWishList) {
      await this.GetAllWishListDetails(value);
    }
  };

  SignOut = async () => {
    await localStorage.removeItem("customer_token");
    await localStorage.removeItem("Customer_UserID");
    await localStorage.removeItem("Customer_UserName");
    await localStorage.removeItem("UserAddress");
    await localStorage.removeItem("OpenUserAddress");
    await localStorage.removeItem("OpenUserDetail");
    await localStorage.removeItem("OpenUserHome");
    await localStorage.removeItem("OpenMyOrder");
    await localStorage.removeItem("OpenCard");
    await localStorage.removeItem("OpenWishList");
    await localStorage.removeItem("OpenCustomerSetting");
    this.props.history.push("/SignIn");
  };

  //
  handleOpenHomeNav = async () => {
    console.log("Handle Open Home Nav Calling .....");

    await this.productServices(
      this.state.PageNumber == 0 ? 1 : this.state.PageNumber
    );

    localStorage.setItem("OpenUserHome", true);
    localStorage.setItem("OpenMyOrder", false);
    localStorage.setItem("OpenCard", false);
    localStorage.setItem("OpenWishList", false);
    localStorage.setItem("OpenCustomerSetting", false);
    localStorage.setItem("OpenUserDetail", false);
    localStorage.setItem("OpenUserAddress", false);

    this.setState({
      PageNumber: 1,
      OpenUserHome: true,
      OpenMyOrder: false,
      OpenCard: false,
      OpenWishList: false,
      OpenCustomerSetting: false,
      OpenUserDetail: false,
      OpenUserAddress: false,
    });
  };

  //
  handleOpenMyOrderNav = async () => {
    console.log("Handle Open My Order Nav Calling .....");

    await this.GetMyOrderList(
      this.state.PageNumber == 0 ? 1 : this.state.PageNumber
    );

    localStorage.setItem("OpenUserHome", false);
    localStorage.setItem("OpenMyOrder", true);
    localStorage.setItem("OpenCard", false);
    localStorage.setItem("OpenWishList", false);
    localStorage.setItem("OpenCustomerSetting", false);
    localStorage.setItem("OpenUserDetail", false);
    localStorage.setItem("OpenUserAddress", false);
    this.setState({
      PageNumber: 1,
      OpenUserHome: false,
      OpenMyOrder: true,
      OpenCard: false,
      OpenWishList: false,
      OpenCustomerSetting: false,
      OpenUserDetail: false,
      OpenUserAddress: false,
    });
  };

  //
  handleOpenCartNav = async () => {
    console.log("Handle Open Cart Nav Calling .....");

    await this.GetAllCardDetails(
      this.state.PageNumber == 0 ? 1 : this.state.PageNumber
    );

    localStorage.setItem("OpenUserHome", false);
    localStorage.setItem("OpenMyOrder", false);
    localStorage.setItem("OpenCard", true);
    localStorage.setItem("OpenWishList", false);
    localStorage.setItem("OpenCustomerSetting", false);
    localStorage.setItem("OpenUserDetail", false);
    localStorage.setItem("OpenUserAddress", false);

    this.setState({
      PageNumber: 1,
      OpenUserHome: false,
      OpenMyOrder: false,
      OpenCard: true,
      OpenWishList: false,
      OpenCustomerSetting: false,
      OpenUserDetail: false,
      OpenUserAddress: false,
    });
  };

  //
  handleOpenWishListNav = async () => {
    console.log("Handle Open Wish List Nav Calling .....");

    await this.GetAllWishListDetails(
      this.state.PageNumber == 0 ? 1 : this.state.PageNumber
    );

    localStorage.setItem("OpenUserHome", false);
    localStorage.setItem("OpenMyOrder", false);
    localStorage.setItem("OpenCard", false);
    localStorage.setItem("OpenWishList", true);
    localStorage.setItem("OpenCustomerSetting", false);
    localStorage.setItem("OpenUserDetail", false);
    localStorage.setItem("OpenUserAddress", false);

    this.setState({
      PageNumber: 1,
      OpenUserHome: false,
      OpenMyOrder: false,
      OpenCard: false,
      OpenWishList: true,
      OpenCustomerSetting: false,
      OpenUserDetail: false,
      OpenUserAddress: false,
    });
  };

  //
  handleOpenCustomerSettingNav = () => {
    console.log("handle Open Customer Setting Nav Calling .....");

    localStorage.setItem("OpenUserHome", false);
    localStorage.setItem("OpenMyOrder", false);
    localStorage.setItem("OpenCard", false);
    localStorage.setItem("OpenWishList", false);
    localStorage.setItem("OpenCustomerSetting", true);
    localStorage.setItem("OpenUserDetail", true);
    localStorage.setItem("OpenUserAddress", false);

    this.setState({
      OpenUserHome: false,
      OpenMyOrder: false,
      OpenCard: false,
      OpenWishList: false,
      OpenCustomerSetting: true,
      OpenUserDetail: true,
      OpenUserAddress: false,
    });
  };

  //
  handleOpenUserAddress = () => {
    console.log("handle Open User Address Nav Calling .....");

    if (localStorage.getItem("UserAddress") === "true") {
      this.GetCustomerAdderess(localStorage.getItem("Customer_UserID"));
    }

    localStorage.setItem("OpenUserHome", false);
    localStorage.setItem("OpenMyOrder", false);
    localStorage.setItem("OpenCard", false);
    localStorage.setItem("OpenWishList", false);
    localStorage.setItem("OpenCustomerSetting", true);
    localStorage.setItem("OpenUserDetail", false);
    localStorage.setItem("OpenUserAddress", true);

    this.setState({
      OpenUserHome: false,
      OpenMyOrder: false,
      OpenCard: false,
      OpenWishList: false,
      OpenCustomerSetting: true,
      OpenUserDetail: false,
      OpenUserAddress: true,
    });
  };

  //
  OpenUserHomeNav = () => {
    return (
      <GetProduct
        List={this.state.Product}
        State="UserHome"
        TotalPages={this.state.TotalPages}
        PageNumber={this.state.PageNumber}
        handlePaging={this.handlePaging}
        productServices={this.productServices}
      />
    );
  };

  OpenMyOrderNav = () => {
    return (
      <GetProduct
        List={this.state.Product}
        State="MyOrder"
        TotalPages={this.state.TotalPages}
        PageNumber={this.state.PageNumber}
        handlePaging={this.handlePaging}
        GetMyOrderList={this.GetMyOrderList}
      />
    );
  };

  //
  OpenCartNav = () => {
    return (
      <GetProduct
        List={this.state.Product}
        State="Cart"
        TotalPages={this.state.TotalPages}
        PageNumber={this.state.PageNumber}
        handlePaging={this.handlePaging}
        GetAllCardDetails={this.GetAllCardDetails}
      />
    );
  };

  //
  OpenWishListNav = () => {
    return (
      <GetProduct
        List={this.state.Product}
        State="WishList"
        TotalPages={this.state.TotalPages}
        PageNumber={this.state.PageNumber}
        handlePaging={this.handlePaging}
        GetAllWishListDetails={this.GetAllWishListDetails}
      />
    );
  };

  //
  OpenUserDetailNav = () => {
    let state = this.state;
    return (
      <div className="OpenUserDetailNav-Container">
        <div className="OpenUserDetailNav-SubContainer">
          <TextField
            label="Full Name"
            variant="outlined"
            style={{ margin: 20, width: 500 }}
            size="small"
            name="FullName"
            value={state.FullName}
            onChange={this.handleChanges}
          />
          <TextField
            label="Email ID"
            variant="outlined"
            style={{ margin: 20, width: 500 }}
            size="small"
            name="EmailID"
            value={state.EmailID}
            onChange={this.handleChanges}
          />
          <TextField
            label="Mobile Number"
            variant="outlined"
            style={{ margin: 20, width: 500 }}
            size="small"
            name="MobileNumber"
            value={state.MobileNumber}
            onChange={this.handleChanges}
          />
          <Button
            variant="contained"
            color="primary"
            onClick={this.handleSubmitUserDetail}
          >
            Confirm
          </Button>
        </div>
      </div>
    );
  };

  //
  OpenUserAddressNav = () => {
    let state = this.state;
    return (
      <div className="OpenUserAddressNav-Container">
        <div className="OpenUserAddressNav-SubContainer">
          <TextField
            label="Address1"
            name="Address1"
            variant="outlined"
            style={{ margin: 10, width: 500 }}
            size="small"
            value={state.Address1}
            onChange={this.handleChanges}
          />
          <TextField
            label="Address2"
            name="Address2"
            variant="outlined"
            style={{ margin: 10, width: 500 }}
            size="small"
            value={state.Address2}
            onChange={this.handleChanges}
          />
          <TextField
            label="City"
            name="City"
            variant="outlined"
            style={{ margin: 10, width: 500 }}
            size="small"
            value={state.City}
            onChange={this.handleChanges}
          />
          <TextField
            label="Distict"
            name="Distict"
            variant="outlined"
            style={{ margin: 10, width: 500 }}
            size="small"
            value={state.Distict}
            onChange={this.handleChanges}
          />
          <TextField
            label="State"
            name="State"
            variant="outlined"
            style={{ margin: 10, width: 500 }}
            size="small"
            value={state.State}
            onChange={this.handleChanges}
          />
          <TextField
            label="Country"
            name="Country"
            variant="outlined"
            style={{ margin: 10, width: 500 }}
            size="small"
            value={state.Country}
            onChange={this.handleChanges}
          />
          <TextField
            label="Pincode"
            name="Pincode"
            variant="outlined"
            style={{ margin: "10px 0 20px 0", width: 500 }}
            size="small"
            value={state.Pincode}
            onChange={this.handleChanges}
          />
          <Button
            variant="contained"
            color="primary"
            onClick={() => {
              this.handleSubmitAddress();
            }}
          >
            {state.Update ? <>Update</> : <>Submit</>}
          </Button>
        </div>
      </div>
    );
  };

  handleOpenMyBookingList = () => {
    return (
      <TableContainer component={Paper} style={{ height: "fit-Content" }}>
        <Table aria-label="customized table">
          <TableHead>
            <TableRow
              style={{
                display: "flex",
                minHeight: "50px",
              }}
            >
              <div className="Header" style={{ flex: 1 }}>
                Cust.ID
              </div>
              <div className="Header" style={{ flex: 0.5 }}>
                Type
              </div>
              <div className="Header" style={{ flex: 1 }}>
                Scenerio
              </div>
              <div className="Header" style={{ flex: 1 }}>
                Price
              </div>
              <div className="Header" style={{ flex: 2 }}>
                Cust. Name
              </div>
              <div className="Header" style={{ flex: 1 }}>
                Contact
              </div>
              <div className="Header" style={{ flex: 2 }}>
                CheckInTime
              </div>
              <div className="Header" style={{ flex: 2 }}>
                CheckOutTime
              </div>
              <div className="Header" style={{ flex: 1 }}>
                Status
              </div>
              <div className="Header" style={{ flex: 2 }}></div>
            </TableRow>
          </TableHead>
          <TableBody style={{ height: "fit-content" }}>
            {this.handleBookingList()}
          </TableBody>
        </Table>
      </TableContainer>
    );
  };

  handleChange = (e) => {
    const { name, value } = e.target;
    this.setState(
      { [name]: value },
      console.log("Name : ", name, "Value : ", value)
    );
  };

  render() {
    let state = this.state;
    let self = this;
    console.log("State : ", state);
    return (
      <div className="UserDashBoard-Container">
        <div className="Sub-Container">
          <div className="Header">
            <AppBar position="static" style={{ backgroundColor: "#202020" }}>
              <Toolbar>
                <Typography
                  variant="h6"
                  style={{
                    flexGrow: 3,
                    display: "flex",
                    padding: "5px 0 0 200px",
                    boxSizing: "border-box",
                  }}
                >
                  Grocery Management System &nbsp;
                  <div style={{ margin: "3px 0 0 0" }}>
                    <ShopIcon />
                  </div>
                </Typography>
                {/* <div className="search" style={{ flexGrow: 0.5 }}>
                  <div
                    className="searchIcon"
                    onClick={() => {
                      this.SearchProduct();
                    }}
                    style={{ cursor: "pointer" }}
                  >
                    <SearchIcon
                      onClick={() => {
                        this.SearchProduct();
                      }}
                      style={{ cursor: "pointer" }}
                    />
                  </div>
                  <InputBase
                    placeholder="Search Product"
                    classes={{
                      root: "inputRoot",
                      input: "inputInput",
                    }}
                    name="SearchProduct"
                    inputProps={{ "aria-label": "search" }}
                    value={this.state.SearchProduct}
                    onChange={this.handleChange}
                  />
                </div> */}
                <Button
                  variant="outlined"
                  style={{
                    color: "white",
                    margin: "0 50px",
                  }}
                  onClick={() => {
                    this.handleFeedOpen();
                  }}
                >
                  Feedback &nbsp;
                  <FeedbackIcon />
                </Button>
                <Button
                  // style={{ flexGrow: 1 }}
                  color="inherit"
                  onClick={() => {
                    this.SignOut();
                  }}
                >
                  LogOut
                </Button>
              </Toolbar>
            </AppBar>
          </div>
          <div className="Body">
            <div className="Sub-Body">
              <div className="SubBody11">
                <div
                  className={state.OpenUserHome ? "NavButton1" : "NavButton2"}
                  onClick={() => {
                    this.handleOpenHomeNav();
                  }}
                >
                  <IconButton edge="start" className="NavBtn" color="inherit">
                    <HomeIcon style={{ color: "white" }} />
                  </IconButton>
                  <div className="NavButtonText">Home</div>
                </div>
                <div
                  className={state.OpenMyOrder ? "NavButton1" : "NavButton2"}
                  onClick={() => {
                    this.handleOpenMyOrderNav();
                  }}
                >
                  <IconButton edge="start" className="NavBtn" color="inherit">
                    <PaymentIcon style={{ color: "white" }} />
                  </IconButton>
                  <div className="NavButtonText">My Order</div>
                </div>
                <div
                  className={state.OpenCard ? "NavButton1" : "NavButton2"}
                  onClick={() => {
                    this.handleOpenCartNav();
                  }}
                >
                  <IconButton edge="start" className="NavBtn" color="inherit">
                    <ShoppingCartIcon style={{ color: "white" }} />
                  </IconButton>
                  <div className="NavButtonText">Cart</div>
                </div>
                <div
                  className={state.OpenWishList ? "NavButton1" : "NavButton2"}
                  onClick={() => {
                    this.handleOpenWishListNav();
                  }}
                >
                  <IconButton edge="start" className="NavBtn" color="inherit">
                    <BookmarkIcon style={{ color: "white" }} />
                  </IconButton>
                  <div className="NavButtonText">Wish List</div>
                </div>

                <div
                  className={
                    state.OpenUserDetail || state.OpenUserAddress
                      ? "NavButton11"
                      : "NavButton22"
                  }
                  style={{
                    height: "fit-content",
                    display: "flex",
                    padding: "5px 0 0 15px",
                  }}
                >
                  <IconButton
                    edge="start"
                    className="NavBtn"
                    color="inherit"
                    style={{ position: "fixed" }}
                  >
                    <Settings style={{ color: "white" }} />
                  </IconButton>
                  <div
                    style={{ height: "fit-content", margin: "10px 0 0 70px" }}
                  >
                    <TreeView style={{ margin: "5px 0 0 0" }}>
                      <TreeItem nodeId="1" label="Customer Setting">
                        <TreeItem
                          nodeId="2"
                          label="User Details"
                          className={
                            state.OpenUserDetail ? "NavButton11" : "NavButton22"
                          }
                          onClick={() => {
                            this.handleOpenCustomerSettingNav();
                          }}
                        />
                        <TreeItem
                          nodeId="3"
                          label="Address"
                          className={
                            state.OpenUserAddress
                              ? "NavButton11"
                              : "NavButton22"
                          }
                          onClick={() => {
                            this.handleOpenUserAddress();
                          }}
                        />
                      </TreeItem>
                    </TreeView>
                  </div>
                </div>
              </div>
              <div className="SubBody22">
                <div style={{ height: "100%", width: "100%" }}>
                  {state.OpenUserHome
                    ? this.OpenUserHomeNav()
                    : state.OpenMyOrder
                      ? this.OpenMyOrderNav()
                      : state.OpenCard
                        ? this.OpenCartNav()
                        : state.OpenWishList
                          ? this.OpenWishListNav()
                          : state.OpenUserDetail
                            ? this.OpenUserDetailNav()
                            : state.OpenUserAddress
                              ? this.OpenUserAddressNav()
                              : null}
                </div>

                <Modal
                  style={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                  }}
                  open={this.state.Open}
                  // open={true}
                  onClose={this.handleClose}
                  closeAfterTransition
                  BackdropComponent={Backdrop}
                  BackdropProps={{
                    timeout: 500,
                  }}
                >
                  <Fade in={this.state.Open}>
                    {state.OpenBookModel ? (
                      <div
                        style={{
                          backgroundColor: "white",
                          boxShadow: "5",
                          padding: "2px 4px 3px",
                          width: "1000px",
                          height: "600px",
                          display: "flex",
                          alignItems: "center",
                          justifyContent: "center",
                          flexDirection: "column",
                        }}
                      >
                        <div className="Input-Field1">
                          <div className="Text">Room Type :</div>
                          <div className="Text-Input">{state.RoomType}</div>
                        </div>
                        <div className="Input-Field1">
                          <div className="Text">Room Scenerio :</div>
                          <div className="Text-Input">{state.RoomScenerio}</div>
                        </div>
                        <div className="Input-Field1">
                          <div className="Text">Room Price :</div>
                          <div className="Text-Input">
                            {state.TotalRoomPrice} Rs.
                          </div>
                        </div>
                        <div style={{ display: "flex", flexDirection: "row" }}>
                          <div
                            style={{
                              display: "flex",
                              justifyContent: "center",
                              alignItems: "center",
                              flexDirection: "column",
                              padding: "10px",
                            }}
                          >
                            <div className="Input-Field">
                              <div className="Text">Customer Name</div>
                              <TextField
                                autoComplete="off"
                                error={state.CustomerNameFlag}
                                className="Text-Input"
                                label="Name"
                                variant="outlined"
                                size="small"
                                name="CustomerName"
                                value={state.CustomerName}
                                onChange={this.handleChanges}
                              />
                            </div>
                            <div className="Input-Field">
                              <div className="Text">Contact</div>
                              <TextField
                                autoComplete="off"
                                error={state.ContactFlag}
                                className="Text-Input"
                                label="Contact"
                                variant="outlined"
                                size="small"
                                name="Contact"
                                value={state.Contact}
                                onChange={this.handleChangeContact}
                              />
                            </div>
                            <div className="Input-Field">
                              <div className="Text">EmailID</div>
                              <TextField
                                autoComplete="off"
                                error={state.EmailIDFlag}
                                className="Text-Input"
                                label="EmailID"
                                variant="outlined"
                                size="small"
                                name="EmailID"
                                value={state.EmailID}
                                onChange={this.handleChangeEmail}
                              />
                            </div>
                            <div className="Input-Field">
                              <div className="Text">Address</div>
                              <TextField
                                autoComplete="off"
                                error={state.AddressFlag}
                                className="Text-Input"
                                label="Address"
                                variant="outlined"
                                size="small"
                                name="Address"
                                value={state.Address}
                                onChange={this.handleChanges}
                              />
                            </div>
                            <div className="Input-Field">
                              <div className="Text">Age</div>
                              <TextField
                                error={state.AgeFlag}
                                autoComplete="off"
                                className="Text-Input"
                                label="Age"
                                placeholder="Ex. 24"
                                variant="outlined"
                                size="small"
                                name="Age"
                                value={state.Age}
                                onChange={this.handleChanges}
                              />
                            </div>
                          </div>
                          <div
                            style={{
                              display: "flex",
                              justifyContent: "center",
                              alignItems: "center",
                              flexDirection: "column",
                              padding: "10px",
                            }}
                          >
                            <div className="Input-Field">
                              <div className="Text">Check In Time</div>
                              <TextField
                                error={state.CheckInTimeFlag}
                                autoComplete="off"
                                className="Text-Input"
                                placeholder="dd/MM/yyyy HH:mm tt"
                                variant="outlined"
                                size="small"
                                name="CheckInTime"
                                type="datetime-local"
                                value={state.CheckInTime}
                                onChange={this.handleChanges}
                              />
                            </div>
                            <div className="Input-Field">
                              <div className="Text">Check Out Time</div>
                              <TextField
                                error={state.CheckOutTimeFlag}
                                autoComplete="off"
                                className="Text-Input"
                                // label="Skill"
                                placeholder="Ex. Coding etc."
                                variant="outlined"
                                size="small"
                                name="CheckOutTime"
                                type="datetime-local"
                                value={state.CheckOutTime}
                                onChange={this.handleChanges}
                              />
                            </div>
                            <div className="Input-Field">
                              <div className="Text">ID Proof</div>
                              <FormControl
                                variant="outlined"
                                style={{ minWidth: 292 }}
                                size="small"
                              >
                                <InputLabel id="demo-simple-select-outlined-label">
                                  ID
                                </InputLabel>

                                <Select
                                  error={state.IDProofFlag}
                                  labelId="demo-simple-select-outlined-label"
                                  value={state.IDProof}
                                  name="IDProof"
                                  onChange={this.handleField}
                                  label="IDProof"
                                >
                                  {Array.isArray(state.IDProofList) &&
                                    state.IDProofList.length > 0 ? (
                                    state.IDProofList.map(function (
                                      data,
                                      index
                                    ) {
                                      // console.log('Field : ', data.fieldName)
                                      return (
                                        <MenuItem value={data} key={index}>
                                          {data}
                                        </MenuItem>
                                      );
                                    })
                                  ) : (
                                    <></>
                                  )}
                                </Select>
                              </FormControl>
                            </div>

                            <div className="Input-Field">
                              <div className="Text">ID Number</div>
                              <TextField
                                autoComplete="off"
                                className="Text-Input"
                                label="ID Number"
                                error={state.IDNumberFlag}
                                variant="outlined"
                                size="small"
                                type="number"
                                name="IDNumber"
                                value={state.IDNumber}
                                onChange={this.handleChanges}
                              />
                            </div>
                            <div className="Input-Field">
                              <div className="Text">PinCode</div>
                              <TextField
                                autoComplete="off"
                                className="Text-Input"
                                label="PinCode"
                                placeholder="Ex: 4110048"
                                variant="outlined"
                                size="small"
                                type="number"
                                name="Pincode"
                                value={state.Pincode}
                                onChange={this.handleChangePinCode}
                              />
                            </div>
                          </div>
                        </div>
                        <div
                          className="Input-Field"
                          style={{
                            display: "flex",
                            justifyContent: "space-around",
                          }}
                        >
                          <Button
                            variant="contained"
                            color="primary"
                            component="span"
                            style={{ margin: "10px 10px 0 0" }}
                            onClick={
                              state.Update
                                ? this.handleUpdate
                                : this.handleSubmit
                            }
                          >
                            {state.Update ? <>Update</> : <>Submit</>}
                            &nbsp;Application
                          </Button>
                          <Button
                            variant="outlined"
                            style={{ margin: "10px 0 0 10px" }}
                            onClick={this.handleClose}
                          >
                            Cancle
                          </Button>
                        </div>
                      </div>
                    ) : state.OpenPayBill ? (
                      <div
                        style={{
                          backgroundColor: "white",
                          boxShadow: "5",
                          padding: "2px 4px 3px",
                          width: "500px",
                          height: "250px",
                          display: "flex",
                          alignItems: "center",
                          justifyContent: "center",
                          flexDirection: "column",
                        }}
                      >
                        <div
                          className="Input-Field"
                          style={{
                            color: "red",
                            fontSize: "20px",
                            fontFamily: "Roboto",
                            display: "flex",
                            justifyContent: "center",
                            alignItems: "center",
                            margin: " 0 0 50px 0",
                            fontWeight: 500,
                          }}
                        >
                          Are You Sure To Pay Bill For Booking ID{" "}
                          {state.CustomerID}&nbsp;?
                        </div>
                        <div style={{ display: "flex" }}>
                          <Button
                            variant="contained"
                            color="primary"
                            component="span"
                            style={{ margin: "10px 10px 0 0" }}
                            onClick={() => {
                              this.handlePayCustomerBill(this.state.CustomerID);
                            }}
                          >
                            Yes
                          </Button>
                          <Button
                            variant="outlined"
                            style={{
                              margin: "10px 0 0 10px",
                              color: "white",
                              background: "black",
                            }}
                            onClick={this.handleClose}
                          >
                            No
                          </Button>
                        </div>
                      </div>
                    ) : (
                      <div
                        style={{
                          backgroundColor: "white",
                          boxShadow: "5",
                          padding: "2px 4px 3px",
                          width: "500px",
                          height: "250px",
                          display: "flex",
                          alignItems: "center",
                          justifyContent: "center",
                          flexDirection: "column",
                        }}
                      >
                        <div
                          className="Input-Field"
                          style={{ margin: "20px 0" }}
                        >
                          <TextField
                            error={this.state.FeedBackFlag}
                            id="outlined-multiline-static"
                            label="Feedback"
                            multiline
                            rows={4}
                            fullWidth
                            variant="outlined"
                            name="FeedBack"
                            value={this.state.FeedBack}
                            onChange={this.handleChanges}
                          />
                        </div>
                        <div style={{ display: "flex" }}>
                          <Button
                            variant="contained"
                            color="primary"
                            component="span"
                            style={{ margin: "10px 10px 0 0" }}
                            onClick={this.handleSubmitFeedback}
                          >
                            Submit Feedback
                          </Button>
                          <Button
                            variant="outlined"
                            style={{ margin: "10px 0 0 10px" }}
                            onClick={this.handleClose}
                          >
                            Cancle
                          </Button>
                        </div>
                      </div>
                    )}
                  </Fade>
                </Modal>
              </div>
            </div>
          </div>
        </div>

        <Backdrop
          style={{ zIndex: "1", color: "#fff" }}
          open={this.state.OpenLoader}
          onClick={() => {
            this.setState({ OpenLoader: false });
          }}
        >
          <CircularProgress color="inherit" />
        </Backdrop>
        <Snackbar
          anchorOrigin={{
            vertical: "bottom",
            horizontal: "left",
          }}
          open={state.OpenSnackBar}
          autoHideDuration={2000}
          onClose={this.handleSnackBarClose}
          message={state.Message}
          action={
            <React.Fragment>
              <Button
                color="secondary"
                size="small"
                onClick={this.handleSnackBarClose}
              >
                UNDO
              </Button>
              <IconButton
                size="small"
                aria-label="close"
                color="inherit"
                onClick={this.handleSnackBarClose}
              >
                <CloseIcon fontSize="small" />
              </IconButton>
            </React.Fragment>
          }
        />
      </div>
    );
  }
}
