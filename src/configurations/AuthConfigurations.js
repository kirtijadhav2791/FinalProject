const ParentConfiguration = require("./ParentConfiguration");

module.exports = {
  SignUp: ParentConfiguration.Auth + "api/Auth/SignUp",
  SignIn: ParentConfiguration.Auth + "api/Auth/SignIn",
  AddCustomerDetail: ParentConfiguration.Auth + "api/Auth/AddCustomerDetail",
  CustomerList: ParentConfiguration.Auth + "api/Auth/CustomerList",
  AddCustomerAdderess:
    ParentConfiguration.Auth + "api/Auth/AddCustomerAdderess",
  GetCustomerAdderess:
    ParentConfiguration.Auth + "api/Auth/GetCustomerAdderess?userId=",
  GetCustomerDetail:
    ParentConfiguration.Auth + "api/Auth/GetCustomerDetail?userId=",
};
