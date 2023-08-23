import AxiosServices from "./AxiosServices";
import AuthConfigurations from "../configurations/AuthConfigurations";

const axiosServices = new AxiosServices();

export default class AuthServices {
  SignUp(data) {
    return axiosServices.post(AuthConfigurations.SignUp, data, false);
  }

  SignIn(data) {
    return axiosServices.post(AuthConfigurations.SignIn, data, false);
  }

  AddCustomerDetail(data) {
    return axiosServices.post(
      AuthConfigurations.AddCustomerDetail,
      data,
      false
    );
  }

  CustomerList(data) {
    return axiosServices.post(AuthConfigurations.CustomerList, data, false);
  }

  AddCustomerAdderess(data) {
    return axiosServices.post(
      AuthConfigurations.AddCustomerAdderess,
      data,
      false
    );
  }

  GetCustomerAdderess(data) {
    return axiosServices.Get(
      AuthConfigurations.GetCustomerAdderess + data,
      false
    );
  }

  GetCustomerDetail(data){
    return axiosServices.Get(
      AuthConfigurations.GetCustomerDetail + data,
      false
    );
  }
}
