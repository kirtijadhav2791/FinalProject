import React, { Component } from "react";
import "./HomePage.scss";

import ProductServices from "../../services/ProductServices";

import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Button from "@material-ui/core/Button";
import ShopIcon from "@material-ui/icons/Shop";
// import SearchIcon from "@material-ui/icons/Search";
import InputBase from "@material-ui/core/InputBase";

import Card from "@material-ui/core/Card";
import CardActionArea from "@material-ui/core/CardActionArea";
import CardActions from "@material-ui/core/CardActions";
import CardContent from "@material-ui/core/CardContent";
import CardMedia from "@material-ui/core/CardMedia";
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

const productServices = new ProductServices();

export default class HomePage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      Feedback: [],
      List: [],
      //
      Message: "",
      //
      NumberOfRecordPerPage: 6,
      PageNumber: 1,

      FeedbackPageNumber: 1,
      //
      TotalPages: 2,
      TotalRecords: 0,

      open: false,
      OpenEdit: false, // Open Editing Booking Model
      OpenLoader: false,
      OpenSnackBar: false,
    };
  }

  componentWillMount() {
    console.log("HomePage componentWillMount Calling ...");
    this.productServices(this.state.PageNumber);
  }

  productServices = async (CurrentPage) => {
    console.log("Get Jobs List Calling ... ");
    let data = {
      pageNumber: CurrentPage,
      numberOfRecordPerPage: 5,
    };
    this.setState({ OpenLoader: true });
    productServices
      .GetAllProduct(data)
      .then((data) => {
        console.log("GetAllProduct Data : ", data);
        this.setState({
          List: data.data.data,
          TotalPages: data.data.totalPage,
          //   PageNumber: data.data.currentPage,
          OpenLoader: false,
          OpenSnackBar: true,
          Message: "Fetch Available Product",
        });
      })
      .catch((error) => {
        console.log("GetAllProduct Error : ", error);
        this.setState({
          OpenLoader: false,
          OpenSnackBar: true,
          Message: "Something Went Wrong",
        });
      });
  };

  handlePaging = async (e, value) => {
    let state = this.state;
    console.log("Current Page : ", value);

    this.setState({
      PageNumber: value,
    });
    await this.productServices(value);
  };

  SignUp = () => {
    window.location = "/SignUp";
  };

  SignIn = () => {
    window.location = "/SignIn";
  };

  handleSnackBarClose = (event, reason) => {
    if (reason === "clickaway") {
      return;
    }

    this.setState({ OpenSnackBar: false });
  };

  render() {
    let state = this.state;
    return (
      <div className="HomePage-Container">
        <div className="HomePage-SubContainer">
          <div className="HomePage-SubContainer-Header">
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
                  Welcome To Grocery Management System &nbsp;
                  <div style={{ margin: "3px 0 0 0" }}>
                    <ShopIcon />
                  </div>
                </Typography>

                <Button
                  color="inherit"
                  onClick={() => {
                    this.SignUp();
                  }}
                >
                  SignUp
                </Button>
                <Button
                  color="inherit"
                  onClick={() => {
                    this.SignIn();
                  }}
                >
                  SignIn
                </Button>
              </Toolbar>
            </AppBar>
          </div>
          <div className="HomePage-SubContainer-Body">
            <div className="HomePage-SubContainer-Body-SubBody1">
              {Array.isArray(this.state.List) && this.state.List.length > 0
                ? this.state.List.map(function (data, index) {
                    console.log("Get Product Data : ", data);
                    return (
                      <>
                        {!data.isArchive && data.isActive ? (
                          <Card
                            className=""
                            style={{ maxWidth: 350, margin: 15 }}
                            key={index}
                          >
                            <CardActionArea>
                              <CardMedia
                                style={{ height: 180, width: 260 }}
                                image={data.productImageUrl}
                                title="Contemplative Reptile"
                              />

                              <CardContent
                                style={{
                                  width: 228,
                                  height: 130,
                                }}
                              >
                                <Typography
                                  gutterBottom
                                  variant="h5"
                                  component="h2"
                                >
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
                                  <>
                                    {data.quantity !== 0 ? (
                                      <> Available : {data.quantity}</>
                                    ) : (
                                      <>Not Available</>
                                    )}
                                  </>
                                  &nbsp; &nbsp;
                                  <>Price : {data.productPrice} &#8377;</>
                                </Typography>
                              </CardContent>
                            </CardActionArea>
                            <CardActions></CardActions>
                          </Card>
                        ) : (
                          <></>
                        )}
                      </>
                    );
                  })
                : null}
            </div>
            <div className="HomePage-SubContainer-Body-SubBody2">
              <Pagination
                count={this.state.TotalPages}
                Page={this.state.PageNumber}
                onChange={this.handlePaging}
                variant="outlined"
                shape="rounded"
                color="secondary"
              />
            </div>
          </div>
          <div className="HomePage-SubContainer-Footer">
            Contact Grocery Management System Customer Care Number Anytime At 1234567890
          </div>
        </div>

        <Backdrop
          style={{ zIndex: "1", color: "#fff" }}
          open={state.OpenLoader}
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
