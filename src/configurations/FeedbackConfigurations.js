const ParentConfiguration = require("./ParentConfiguration");

module.exports = {
  GetFeedbacks: ParentConfiguration.Feedback + "api/Feedback/GetFeedbacks",
  AddFeedback: ParentConfiguration.Feedback + "api/Feedback/AddFeedback",
  DeleteFeedback: ParentConfiguration.Feedback + "api/Feedback/DeleteFeedback?ID=",
};
