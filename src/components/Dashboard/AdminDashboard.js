import React, { Component } from "react";
import "./AdminDashboard.css";
import AddProduct from "../Product/AddProduct";
import GetProduct from "../Product/GetProduct";
import FeedbackServices from "../../services/FeedbackServices";
import ProductServices from "../../services/ProductServices";
import AuthServices from "../../services/AuthServices";
import CartServices from "../../services/CartServices";
import Rating from "@material-ui/lab/Rating";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import Button from "@material-ui/core/Button";
import IconButton from "@material-ui/core/IconButton";
import HomeIcon from "@material-ui/icons/Home";
import ViewListIcon from "@material-ui/icons/ViewList";
import ArchiveIcon from "@material-ui/icons/Archive";
import DeleteIcon from "@material-ui/icons/Delete";
import TextField from "@material-ui/core/TextField";
import Modal from "@material-ui/core/Modal";
import Backdrop from "@material-ui/core/Backdrop";
import Fade from "@material-ui/core/Fade";
import Pagination from "@material-ui/lab/Pagination";
import FeedbackIcon from "@material-ui/icons/Feedback";
import AddBoxIcon from "@material-ui/icons/AddBox";
import CircularProgress from "@material-ui/core/CircularProgress";
import Snackbar from "@material-ui/core/Snackbar";
import CloseIcon from "@material-ui/icons/Close";
import ErrorIcon from "@material-ui/icons/Error";
import ShopIcon from "@material-ui/icons/Shop";
import SearchIcon from "@material-ui/icons/Search";
import InputBase from "@material-ui/core/InputBase";
import PeopleAltIcon from "@material-ui/icons/PeopleAlt";
import InputLabel from "@material-ui/core/InputLabel";
import MenuItem from "@material-ui/core/MenuItem";
import FormControl from "@material-ui/core/FormControl";
import Select from "@material-ui/core/Select";

//Table Library
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableContainer from "@material-ui/core/TableContainer";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import Paper from "@material-ui/core/Paper";
import Grid from "@material-ui/core/Grid";

//Card
import Card from "@material-ui/core/Card";
import CardActions from "@material-ui/core/CardActions";
import CardContent from "@material-ui/core/CardContent";

const feedbackServices = new FeedbackServices();
const productServices = new ProductServices();
const authServices = new AuthServices();
const cartServices = new CartServices();

const MobileRegex = RegExp(/^[0-9]{11}$/i);
const PinCodeRegex = RegExp(/^[0-9]{7}$/i);
const EmailRegex = RegExp(
  /^(([^<>()[\].,;:\s@"]+(\.[^<>()[\].,;:\s@"]+)*)|(".+"))@(([^<>()[\].,;:\s@"]+.)+[^<>()[\].,;:\s@"]{2,})$/i
);

export default class AdminDashboard extends Component {
  constructor(props) {
    super(props);
    this.state = {
      Feedback: [],
      Product: [],
      //

      //
      SearchProduct: "",
      Message: "",
      //
      NumberOfRecordPerPage: 6,
      PageNumber: 1,

      FeedbackPageNumber: 1,
      //
      TotalPages: 0,
      TotalRecords: 0,

      open: false,
      OpenEdit: false, // Open Editing Booking Model
      OpenLoader: false,
      OpenSnackBar: false,

      OpenHome: true,
      OpenAddProduct: false,
      OpenArchive: false,
      OpenTrash: false,
      OpenCustomerList: false,
      OpenOrderList: false,
      OpenFeedBack: false,

      Update: false,
      ShowApplicantInfo: false,
      OpenBookModel: false, //Editing Booking Application
    };
  }

  //
  componentWillMount() {
    console.log("Component will mount calling ...  State : ", this.state);
    this.setState({
      OpenHome: localStorage.getItem("OpenHome") === "true" ? true : false,
      OpenAddProduct:
        localStorage.getItem("OpenAddProduct") === "true" ? true : false,
      OpenArchive:
        localStorage.getItem("OpenArchive") === "true" ? true : false,
      OpenTrash: localStorage.getItem("OpenTrash") === "true" ? true : false,
      OpenCustomerList:
        localStorage.getItem("OpenCustomerList") === "true" ? true : false,
      OpenOrderList:
        localStorage.getItem("OpenOrderList") === "true" ? true : false,
      OpenFeedBack:
        localStorage.getItem("OpenFeedBack") === "true" ? true : false,
    });

    if (localStorage.getItem("OpenHome") === "true") {
      this.productServices(this.state.PageNumber);
    } else if (localStorage.getItem("OpenArchive") === "true") {
      this.GetArchiveList(this.state.PageNumber);
    } else if (localStorage.getItem("OpenTrash") === "true") {
      this.GetTrashList(this.state.PageNumber);
    } else if (localStorage.getItem("OpenCustomerList") === "true") {
      this.GetCustomerList(this.state.PageNumber);
    } else if (localStorage.getItem("OpenOrderList") === "true") {
      this.GetAllOrderList(this.state.PageNumber);
    } else if (localStorage.getItem("OpenFeedBack") === "true") {
      this.GetFeedBack(this.state.PageNumber);
    }
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
            Product: data.data.data,
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
        this.setState({ OpenLoader: false, Message: "Something went wrong" });
      });
  };

  //
  productServices = async (CurrentPage) => {
    console.log("Get Jobs List Calling ... ");
    let data = {
      pageNumber: CurrentPage,
      numberOfRecordPerPage: 4,
    };
    this.setState({ OpenLoader: true });
    productServices
      .GetAllProduct(data)
      .then((data) => {
        console.log("GetAllProduct Data : ", data);
        // debugger
        if (data.data.data === null && this.state.PageNumber > 1) {
          this.setState({ PageNumber: this.state.PageNumber - 1 });
          this.productServices(this.state.PageNumber);
        } else {
          this.setState({
            Product: data.data.data,
            TotalPages: data.data.totalPage,
            PageNumber: data.data.currentPage,
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
  GetArchiveList = (CurrentPage) => {
    let data = {
      pageNumber: CurrentPage,
      numberOfRecordPerPage: 4,
    };
    this.setState({ OpenLoader: true });
    productServices
      .GetArchiveProduct(data)
      .then((data) => {
        console.log("GetArchiveList Data : ", data);
        this.setState({ Product: [] })
        if (data.data.data === null && this.state.PageNumber > 1) {
          this.setState({ PageNumber: this.state.PageNumber - 1 });
          this.GetArchiveList(this.state.PageNumber);
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
        console.log("GetArchiveList Error : ", error);
        this.setState({ OpenLoader: false });
      });
  };

  //
  GetTrashList = (CurrentPage) => {
    let data = {
      pageNumber: CurrentPage,
      numberOfRecordPerPage: 4,
    };
    this.setState({ OpenLoader: true });
    productServices
      .GetTrashProduct(data)
      .then((data) => {
        console.log("GetTrashProduct Data : ", data);
        this.setState({ Product: [] })
        if (data.data.data === null && this.state.PageNumber > 1) {
          this.setState({ PageNumber: this.state.PageNumber - 1 });
          this.GetTrashList(this.state.PageNumber);
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
        console.log("GetTrashProduct Error : ", error);
        this.setState({ OpenLoader: false });
      });
  };

  //
  GetCustomerList = (CurrentPage) => {
    let data = {
      pageNumber: CurrentPage,
      numberOfRecordPerPage: 8,
    };
    this.setState({ OpenLoader: true, Product: [] });
    authServices
      .CustomerList(data)
      .then((data) => {
        console.log("GetCustomerList Data : ", data);
        if (data.data.data === null && this.state.PageNumber > 1) {
          this.setState({ PageNumber: this.state.PageNumber - 1 });
          this.GetCustomerList(this.state.PageNumber);
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
        console.log("GetCustomerList Error : ", error);
        this.setState({ OpenLoader: false });
      });
  };

  //
  GetAllOrderList = async (CurrentPage) => {
    console.log("Get My Order List Calling ... ");
    let data = {
      pageNumber: CurrentPage,
      numberOfRecordPerPage: 8,
      userID: -1,
    };
    this.setState({ OpenLoader: true, Product: [] });
    cartServices
      .GetOrderProduct(data)
      .then((data) => {
        debugger
        console.log("GetMyOrderList Data : ", data);
        if (data.data.data === null && this.state.PageNumber > 1) {
          this.setState({ PageNumber: this.state.PageNumber - 1 });
          this.GetAllOrderList(this.state.PageNumber);
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
        this.setState({ OpenLoader: false });
      });
  };

  GetFeedBack = async (CurrentPage) => {
    // debugger;
    let data = {
      pageNumber: CurrentPage,
      numberOfRecordPerPage: 8,
    };
    this.setState({ Product: [] })
    await feedbackServices
      .GetFeedbacks(data)
      .then((data) => {
        console.log("Feedback Data : ", data);
        this.setState({
          OpenSnackBar: true,
          Open: false,
          Message: data.data.message,
          Product: data.data.data,
          TotalPages: data.data.totalPage,
        });
      })
      .catch((error) => {
        console.log("Feedback Error : ", error);
        this.setState({
          OpenSnackBar: true,

          Open: false,
          Message: "Something Went Wrong.",
        });
      });
  };

  handleMenuButton = (e) => {
    console.log("Handle Menu Button Calling ... ");
    this.setState({
      MenuOpen: !this.state.MenuOpen,
    });
  };

  handleOpen = () => {
    console.log("Handle Open Calling ... ");

    this.setState({
      open: true,
      OpenHome: true,
    });
  };

  handleClose = () => {
    console.log("Handle Close Calling ...");
    this.setState({
      open: false,
      Update: false,
      OpenEdit: false,
      OpenBookModel: false,
    });
  };

  handleSnackBarClose = (event, reason) => {
    if (reason === "clickaway") {
      return;
    }

    this.setState({ OpenSnackBar: false });
  };

  //
  handleOpenHomeNav = async () => {
    console.log("Handle Open List Calling ... ");

    await this.productServices(
      this.state.PageNumber == 0 ? 1 : this.state.PageNumber
    );

    //
    localStorage.setItem("OpenHome", true);
    localStorage.setItem("OpenAddProduct", false);
    localStorage.setItem("OpenArchive", false);
    localStorage.setItem("OpenTrash", false);
    localStorage.setItem("OpenCustomerList", false);
    localStorage.setItem("OpenOrderList", false);
    localStorage.setItem("OpenFeedBack", false);
    //
    this.setState({
      PageNumber: 1,
      OpenHome: true,
      OpenAddProduct: false,
      OpenArchive: false,
      OpenTrash: false,
      OpenCustomerList: false,
      OpenOrderList: false,
      OpenFeedBack: false,
    });
    //
  };

  //
  handleOpenAddProductNav = () => {
    console.log("Handle Add Product Nav Calling ... ");
    //
    localStorage.setItem("OpenHome", false);
    localStorage.setItem("OpenAddProduct", true);
    localStorage.setItem("OpenArchive", false);
    localStorage.setItem("OpenTrash", false);
    localStorage.setItem("OpenCustomerList", false);
    localStorage.setItem("OpenOrderList", false);
    localStorage.setItem("OpenFeedBack", false);
    //
    this.setState({
      PageNumber: 1,
      OpenHome: false,
      OpenAddProduct: true,
      OpenArchive: false,
      OpenTrash: false,
      OpenCustomerList: false,
      OpenOrderList: false,
      OpenFeedBack: false,
    });
  };

  //
  handleOpenArchiveNav = async () => {
    console.log("Handle Open Archive Nav Calling ... ");

    await this.GetArchiveList(
      this.state.PageNumber == 0 ? 1 : this.state.PageNumber
    );

    localStorage.setItem("OpenHome", false);
    localStorage.setItem("OpenAddProduct", false);
    localStorage.setItem("OpenArchive", true);
    localStorage.setItem("OpenTrash", false);
    localStorage.setItem("OpenCustomerList", false);
    localStorage.setItem("OpenOrderList", false);
    localStorage.setItem("OpenFeedBack", false);

    this.setState({
      PageNumber: 1,
      OpenHome: false,
      OpenAddProduct: false,
      OpenArchive: true,
      OpenTrash: false,
      OpenCustomerList: false,
      OpenOrderList: false,
      OpenFeedBack: false,
    });

    // this.productServices(
    //   this.state.PageNumber == 0 ? 1 : this.state.PageNumber
    // );
  };

  //
  handleOpenTrashNav = async () => {
    console.log("Handle Open Trash Nav Calling...");

    await this.GetTrashList(
      this.state.PageNumber == 0 ? 1 : this.state.PageNumber
    );

    localStorage.setItem("OpenHome", false);
    localStorage.setItem("OpenAddProduct", false);
    localStorage.setItem("OpenArchive", false);
    localStorage.setItem("OpenTrash", true);
    localStorage.setItem("OpenCustomerList", false);
    localStorage.setItem("OpenOrderList", false);
    localStorage.setItem("OpenFeedBack", false);

    this.setState({
      PageNumber: 1,
      OpenHome: false,
      OpenAddProduct: false,
      OpenArchive: false,
      OpenTrash: true,
      OpenCustomerList: false,
      OpenOrderList: false,
      OpenFeedBack: false,
    });

    // this.productServices(
    //   this.state.PageNumber == 0 ? 1 : this.state.PageNumber
    // );
  };

  //
  handleOpenCustomerListNav = () => {
    console.log("Handle Open Customer List Nav Calling...");
    //
    localStorage.setItem("OpenHome", false);
    localStorage.setItem("OpenAddProduct", false);
    localStorage.setItem("OpenArchive", false);
    localStorage.setItem("OpenTrash", false);
    localStorage.setItem("OpenCustomerList", true);
    localStorage.setItem("OpenOrderList", false);
    localStorage.setItem("OpenFeedBack", false);
    //
    this.setState({
      PageNumber: 1,
      OpenHome: false,
      OpenAddProduct: false,
      OpenArchive: false,
      OpenTrash: false,
      OpenCustomerList: true,
      OpenOrderList: false,
      OpenFeedBack: false,
    });

    this.GetCustomerList(this.state.PageNumber);
  };

  //
  handleOpenOrderListNav = () => {
    console.log("Handle Open Customer List Nav Calling...");
    //
    localStorage.setItem("OpenHome", false);
    localStorage.setItem("OpenAddProduct", false);
    localStorage.setItem("OpenArchive", false);
    localStorage.setItem("OpenTrash", false);
    localStorage.setItem("OpenCustomerList", false);
    localStorage.setItem("OpenOrderList", true);
    localStorage.setItem("OpenFeedBack", false);
    //
    this.setState({
      PageNumber: 1,
      OpenHome: false,
      OpenAddProduct: false,
      OpenArchive: false,
      OpenTrash: false,
      OpenCustomerList: false,
      OpenOrderList: true,
      OpenFeedBack: false,
    });

    this.GetAllOrderList(this.state.PageNumber);
  };

  //
  handleOpenFeedBackNav = (e) => {
    console.log("Handle FeedBack Open Calling...");
    //
    localStorage.setItem("OpenHome", false);
    localStorage.setItem("OpenAddProduct", false);
    localStorage.setItem("OpenArchive", false);
    localStorage.setItem("OpenTrash", false);
    localStorage.setItem("OpenCustomerList", false);
    localStorage.setItem("OpenOrderList", false);
    localStorage.setItem("OpenFeedBack", true);
    //
    this.setState({
      // PageNumber: 1,
      OpenHome: false,
      OpenAddProduct: false,
      OpenArchive: false,
      OpenTrash: false,
      OpenCustomerList: false,
      OpenOrderList: false,
      OpenFeedBack: true,
    });

    this.GetFeedBack(this.state.PageNumber);
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

  handleDeleteFeedback = async (ID) => {
    console.log("handleDeleteFeedback Calling ..... ID :", ID);
    // let data = { ID: ID };
    await feedbackServices
      .DeleteFeedback(ID)
      .then((data) => {
        console.log("Feedback Data : ", data);
        this.setState({
          OpenSnackBar: true,
          Open: false,
          Message: data.data.message,
        });
        this.GetFeedBack(this.state.FeedbackPageNumber);
      })
      .catch((error) => {
        console.log("Feedback Error : ", error);
        this.setState({
          OpenSnackBar: true,

          Open: false,
          Message: "Something Went Wrong.",
        });
        this.GetFeedBack(this.state.FeedbackPageNumber);
      });
  };

  handlePaging = async (e, value) => {
    let state = this.state;
    console.log("Current Page : ", value);

    this.setState({
      PageNumber: value,
    });

    if (state.OpenHome) {
      await this.productServices(value);
    } else if (state.OpenArchive) {
      await this.GetArchiveList(value);
    } else if (state.OpenTrash) {
      await this.GetTrashList(value);
    } else if (state.OpenCustomerList) {
      await this.GetCustomerList(value);
    } else if (state.OpenOrderList) {
      await this.GetAllOrderList(value);
    } else if (state.OpenFeedBack) {
      await this.GetFeedBack(value);
    }
  };

  SignOut = async () => {
    await localStorage.removeItem("Admin_token");
    await localStorage.removeItem("Admin_UserID");
    await localStorage.removeItem("OpenHome");
    await localStorage.removeItem("OpenAddProduct");
    await localStorage.removeItem("OpenArchive");
    await localStorage.removeItem("OpenTrash");
    await localStorage.removeItem("OpenCustomerList");
    await localStorage.removeItem("OpenOrderList");
    await localStorage.removeItem("OpenFeedBack");

    this.props.history.push("/SignIn");
  };

  //
  OpenHomeNav = () => {
    // debugger;
    return (
      <GetProduct
        List={this.state.Product}
        State="Home"
        TotalPages={this.state.TotalPages}
        PageNumber={this.state.PageNumber}
        handlePaging={this.handlePaging}
        productServices={this.productServices}
      />
    );
  };

  //
  OpenAddProductNav = () => {
    return (
      <AddProduct
        handleOpenHomeNav={() => {
          this.handleOpenHomeNav();
        }}
      />
    );
  };

  //
  OpenFeedBackNav = () => {
    return (
      <TableContainer
        component={Paper}
        style={{
          display: "flex",
          justifyContent: "flex-Start",
          alignItems: "flex-Start",
          margin: 10,
          boxSizing: "border-box",
          width: "98.5%",
          height: "90%",
          // background: "#202020af",
        }}
      >
        <Table
          aria-label="customized table"
          style={{ height: "fit-Content", width: "100%" }}
        >
          <TableHead>
            <TableRow
              style={{
                display: "flex",
                minHeight: "50px",
                flex: 9,
              }}
            >
              <div className="Header" style={{ flex: 0.7 }}>
                FeedBack ID
              </div>
              <div className="Header" style={{ flex: 2 }}>
                FeedBack User
              </div>
              <div className="Header" style={{ width: 700 }}>
                FeedBack
              </div>
              <div className="Header" style={{ flex: 0.5 }}></div>
            </TableRow>
          </TableHead>
          <TableBody style={{ height: "fit-content" }}>
            {this.handleFeedbackList()}
          </TableBody>
        </Table>
      </TableContainer>
    );
  };

  //
  handleFeedbackList = (e) => {
    let self = this;
    return Array.isArray(this.state.Product) && this.state.Product.length > 0
      ? this.state.Product.map(function (data, index) {
        return (
          <TableRow
            key={index}
            style={{
              height: "50px",
              display: "flex",
              borderBottom: "0.5px solid lightgray",
              flex: 9,
            }}
          >
            <div className="Row" style={{ flex: 0.7 }}>
              {data.feedbackID}
            </div>
            <div className="Row" style={{ flex: 2 }}>
              {data.userName}
            </div>
            <div
              className="Row"
              style={{
                width: "700px",
                textOverflow: "ellipsis",
                overflow: "hidden",
                whiteSpace: "nowrap",
              }}
            >
              {data.feedBack}
            </div>
            <div className="Row" style={{ flex: 0.5 }}>
              <IconButton
                variant="outlined"
                style={{ color: "black" }}
                onClick={() => {
                  self.handleDeleteFeedback(data.feedbackID);
                }}
              >
                <DeleteIcon />
              </IconButton>
            </div>
          </TableRow>
        );
      })
      : null;
  };

  //
  OpenArchiveNav = () => {
    console.log("OpenArchiveNav Calling....");
    return (
      <GetProduct
        List={this.state.Product}
        State="Archive"
        TotalPages={this.state.TotalPages}
        PageNumber={this.state.PageNumber}
        handlePaging={this.handlePaging}
        GetArchiveList={this.GetArchiveList}
        productServices={this.productServices}
      />
    );
  };

  //
  OpenTrashNav = () => {
    return (
      <GetProduct
        List={this.state.Product}
        State="Trash"
        TotalPages={this.state.TotalPages}
        PageNumber={this.state.PageNumber}
        handlePaging={this.handlePaging}
        GetTrashList={this.GetTrashList}
        productServices={this.productServices}
      />
    );
  };

  //
  OpenCustomerListNav = () => {
    return (
      <TableContainer
        component={Paper}
        style={{
          display: "flex",
          justifyContent: "flex-Start",
          alignItems: "flex-start",
          margin: 10,
          width: "98.5%",
          height: "90%",
          // background: "#202020af",
        }}
      >
        <Table
          aria-label="customized table"
          style={{ height: "fit-Content", width: "100%" }}
        >
          <TableHead>
            <TableRow
              style={{
                display: "flex",
                minHeight: "50px",
                flex: 8,
              }}
            >
              {/* <div className="Header" style={{ flex: 1 }}>
                Customer ID
              </div> */}
              <div className="Header" style={{ flex: 2 }}>
                UserName
              </div>
              <div className="Header" style={{ flex: 2 }}>
                Full Name
              </div>
              <div className="Header" style={{ flex: 2 }}>
                Email ID
              </div>
              <div className="Header" style={{ flex: 2 }}>
                Mobile Number
              </div>
            </TableRow>
          </TableHead>
          <TableBody style={{ height: "fit-content" }}>
            {this.handleCustomerList()}
          </TableBody>
        </Table>
      </TableContainer>
    );
  };

  //
  handleCustomerList = (e) => {
    let self = this;
    return Array.isArray(this.state.Product) && this.state.Product.length > 0
      ? this.state.Product.map(function (data, index) {
        return (
          <TableRow
            key={index}
            style={{
              height: "50px",
              display: "flex",
              borderBottom: "0.5px solid lightgray",
              flex: 8,
            }}
          >
            {/* <div className="Row" style={{ flex: 1 }}>
                {data.id == -1 ? <>NA</> : <>{data.id}</>}
              </div> */}
            <div className="Row" style={{ flex: 2 }}>
              {data.userName}
            </div>
            <div className="Row" style={{ flex: 2 }}>
              {/* {data.fullName} */}
              {data.fullName == "" ? <>NA</> : <>{data.fullName}</>}
            </div>
            <div className="Row" style={{ flex: 2 }}>
              {/* {data.emailID} */}
              {data.emailID == "" ? <>NA</> : <>{data.emailID}</>}
            </div>
            <div className="Row" style={{ flex: 2 }}>
              {/* {data.mobileNumber} */}
              {data.mobileNumber == "" ? <>NA</> : <>{data.mobileNumber}</>}
            </div>
          </TableRow>
        );
      })
      : null;
  };

  //
  OpenOrderListNav = () => {
    return (
      <TableContainer
        component={Paper}
        style={{
          display: "flex",
          justifyContent: "flex-Start",
          alignItems: "flex-Start",
          margin: 10,
          width: "98.5%",
          height: "90%",
          background: "white",
        }}
      >
        <Table
          aria-label="customized table"
          style={{ height: "fit-Content", width: "100%" }}
        >
          <TableHead>
            <TableRow
              style={{
                display: "flex",
                minHeight: "50px",
                flex: 10,
              }}
            >
              <div className="Header" style={{ flex: 1 }}>
                Order ID
              </div>
              <div className="Header" style={{ flex: 2 }}>
                Customer Name
              </div>
              <div className="Header" style={{ flex: 2 }}>
                Product Name
              </div>
              <div className="Header" style={{ flex: 2 }}>
                Product Type
              </div>
              <div className="Header" style={{ flex: 1 }}>
                Product Price
              </div>
              <div className="Header" style={{ flex: 2 }}>
                Rating
              </div>
            </TableRow>
          </TableHead>
          <TableBody style={{ height: "fit-content" }}>
            {this.handleOrderList()}
          </TableBody>
        </Table>
      </TableContainer>
    );
  };

  //
  handleOrderList = (e) => {
    let self = this;
    return Array.isArray(this.state.Product) && this.state.Product.length > 0
      ? this.state.Product.map(function (data, index) {
        return (
          <TableRow
            key={index}
            style={{
              height: "50px",
              display: "flex",
              borderBottom: "0.5px solid lightgray",
              flex: 10,
            }}
          >
            <div className="Row" style={{ flex: 1 }}>
              {data.cartID}
            </div>
            <div className="Row" style={{ flex: 2 }}>
              {data.fullName}
            </div>
            <div className="Row" style={{ flex: 2 }}>
              {data.productName}
            </div>
            <div className="Row" style={{ flex: 2 }}>
              {data.productType}
            </div>
            <div className="Row" style={{ flex: 1 }}>
              {data.productPrice}
            </div>
            <div className="Row" style={{ flex: 2 }}>
              <Rating name="Rating" value={data.rating} />
              {/* {data.productPrice} */}
            </div>
          </TableRow>
        );
      })
      : null;
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
    console.log("state : ", state);
    return (
      <div className="AdminDashboard-Container">
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
                  Glocery Management System &nbsp;
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
                  className={state.OpenHome ? "NavButton1" : "NavButton2"}
                  onClick={this.handleOpenHomeNav}
                >
                  <IconButton edge="start" className="NavBtn" color="inherit">
                    <HomeIcon style={{ color: "white" }} />
                  </IconButton>
                  <div className="NavButtonText">Home</div>
                </div>

                <div
                  className={state.OpenAddProduct ? "NavButton1" : "NavButton2"}
                  onClick={this.handleOpenAddProductNav}
                >
                  <IconButton edge="start" className="NavBtn" color="inherit">
                    <AddBoxIcon style={{ color: "white" }} />
                  </IconButton>
                  <div className="NavButtonText">Add Product</div>
                </div>
                <div
                  className={state.OpenArchive ? "NavButton1" : "NavButton2"}
                  onClick={this.handleOpenArchiveNav}
                >
                  <IconButton edge="start" className="NavBtn" color="inherit">
                    <ArchiveIcon style={{ color: "white" }} />
                  </IconButton>
                  <div className="NavButtonText">Archive</div>
                </div>
                <div
                  className={state.OpenTrash ? "NavButton1" : "NavButton2"}
                  onClick={this.handleOpenTrashNav}
                >
                  <IconButton edge="start" className="NavBtn" color="inherit">
                    <DeleteIcon style={{ color: "white" }} />
                  </IconButton>
                  <div className="NavButtonText">Trash</div>
                </div>
                <div
                  className={
                    state.OpenCustomerList ? "NavButton1" : "NavButton2"
                  }
                  onClick={this.handleOpenCustomerListNav}
                >
                  <IconButton edge="start" className="NavBtn" color="inherit">
                    <PeopleAltIcon style={{ color: "white" }} />
                  </IconButton>

                  <div className="NavButtonText">Customer List</div>
                </div>
                <div
                  className={state.OpenOrderList ? "NavButton1" : "NavButton2"}
                  onClick={this.handleOpenOrderListNav}
                >
                  <IconButton edge="start" className="NavBtn" color="inherit">
                    <ViewListIcon style={{ color: "white" }} />
                  </IconButton>
                  <div className="NavButtonText">Order List</div>
                </div>
                <div
                  className={state.OpenFeedBack ? "NavButton1" : "NavButton2"}
                  onClick={this.handleOpenFeedBackNav}
                >
                  <IconButton edge="start" className="NavBtn" color="inherit">
                    <FeedbackIcon style={{ color: "white" }} />
                  </IconButton>

                  <div className="NavButtonText">FeedBack</div>
                </div>
              </div>
              <div className="SubBody21">
                <div
                  style={{
                    height: "100%",
                    width: "100%",
                    background: "#202020af",
                  }}
                >
                  {state.OpenHome ? (
                    this.OpenHomeNav()
                  ) : state.OpenAddProduct ? (
                    this.OpenAddProductNav()
                  ) : state.OpenFeedBack ? (
                    this.OpenFeedBackNav()
                  ) : state.OpenArchive ? (
                    this.OpenArchiveNav()
                  ) : state.OpenTrash ? (
                    this.OpenTrashNav()
                  ) : state.OpenCustomerList ? (
                    this.OpenCustomerListNav()
                  ) : state.OpenOrderList ? (
                    this.OpenOrderListNav()
                  ) : (
                    <></>
                  )}
                  {state.OpenCustomerList ||
                    state.OpenOrderList ||
                    state.OpenFeedBack ? (
                    <div
                      style={{
                        width: "100%",
                        display: "flex",
                        justifyContent: "center",
                      }}
                    >
                      <Pagination
                        count={this.state.TotalPages}
                        Page={this.state.PageNumber}
                        onChange={this.handlePaging}
                        variant="outlined"
                        shape="rounded"
                        color="secondary"
                      />
                    </div>
                  ) : (
                    <></>
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
        <Backdrop
          style={{ zIndex: "1", color: "#fff" }}
          open={this.state.OpenLoader}
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
