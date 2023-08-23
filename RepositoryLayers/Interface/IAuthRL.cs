using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayers.Interface
{
    public interface IAuthRL
    {
        public Task<SignUpResponse> SignUp(SignUpRequest request);
        public Task<SignInResponse> SignIn(SignInRequest request);
        public Task<CustomerListResponse> CustomerList(CustomerListRequest request);
        public Task<AddCustomerDetailResponse> AddCustomerDetail(AddCustomerDetailRequest request);
        public Task<GetCustomerDetailResponse> GetCustomerDetail(int UserID);
        public Task<AddCustomerAdderessResponse> AddCustomerAdderess(AddCustomerAdderessRequest request);
        public Task<GetCustomerAdderessResponse> GetCustomerAdderess(int UserID);
        public Task<GetIsCustomerDetailsFoundResponse> GetIsCustomerDetailsFound(int UserID);
    }
}
