import FeedbackConfigurations from '../configurations/FeedbackConfigurations'
import AxiosServices from './AxiosServices'

const axiosServices = new AxiosServices()

export default class FeedbackServices {
  GetFeedbacks(data) {
    return axiosServices.post(FeedbackConfigurations.GetFeedbacks, data, false)
  }

  AddFeedback(data) {
    return axiosServices.post(FeedbackConfigurations.AddFeedback, data, false)
  }

  DeleteFeedback(data) {
    return axiosServices.Delete(
      FeedbackConfigurations.DeleteFeedback + data,
      false,
    )
  }
}
