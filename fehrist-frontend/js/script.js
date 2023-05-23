// ADD IMAGE TO NEW TASK
window.addEventListener("load", (event) => {
  if (location.pathname == "/") {
    GET_Tasks("Added");
  }
});

var BASE_URL = "http://localhost:56067";

function isValidEmail(email) {
  // Regular expression to validate email format
  var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  // Test the email against the regex
  return emailRegex.test(email);
}

// AUTHORIZATION
$("#SignupBtn").on("click", () => {
  var name = $("#RegNameBox").val();
  var email = $("#RegEmailBox").val();
  var pass = $("#RegPassBox").val();
  var cpass = $("#RegRePassBox").val();

  if (
    name.trim() === "" ||
    email.trim() === "" ||
    cpass.trim() === "" ||
    pass.trim() === ""
  ) {
    // Name is empty
    alert("Requried Fields marked with * cannot be empty !");
    return;
  }
  if (!isValidEmail(email)) {
    // Invalid email format
    alert("Please enter a valid email address.");
    return;
  }
  if (pass !== cpass) {
    // Passwords do not match
    alert("Passwords do not match. Please try again.");
    return;
  }

  fetch(`${BASE_URL}/api/user/register-user`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },

    body: JSON.stringify({
      email: email,
      password: pass,
      name: name,
    }),
  })
    .then((result) => {
      console.log(1, result);
      return result.json();
    })
    .then((res) => {
      console.log(2, res);
      if (res.status == "FAIL") alert(res.msg);
      if (res.status == "PASS") {
        switch (res.response.roleName) {
          case "USER":
            deleteAllCookies();
            window.localStorage.setItem("CurrentUser", res.response.name);
            document.cookie = `FehristCookie=${JSON.stringify(
              res.response
            )};path=/`;
            window.location.replace("/");
            break;
          default:
            alert(
              "Invalid ID/Password. Please login using correct information."
            );
            break;
        }
        window.location.replace("/");
      }
    })
    .catch((err) => {
      console.log(3, err);
      alert(
        "Unable to reach server at the moment. Please contact administrator"
      );
    });
});

$("#LoginBtn").on("click", () => {
  var email = $("#LoginEmailBox").val();
  var pass = $("#LoginPassBox").val();

  if (email.trim() === "" || pass.trim() === "") {
    // Name is empty
    alert("Requried Fields marked with * cannot be empty !");
    return;
  }
  if (!isValidEmail(email)) {
    // Invalid email format
    alert("Please enter a valid email address.");
    return;
  }
  fetch(`${BASE_URL}/api/user/login-user`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json", // Adjust content type based on your API
    },
    body: JSON.stringify({
      email: email,
      password: pass,
    }),
  })
    .then((result) => {
      return result.json();
    })
    .then((res) => {
      if (res.status == "FAIL") alert(res.msg);
      if (res.status == "PASS") {
        switch (res.response.roleName) {
          case "USER":
            deleteAllCookies();
            localStorage.setItem("CurrentUser", res.response.name);
            document.cookie = `FehristCookie=${JSON.stringify(
              res.response
            )};path=/`;
            window.location.replace("/");
            break;
          default:
            alert(
              "Invalid ID/Password. Please login using correct information."
            );
            break;
        }
        window.location.replace("/");
      }
    })
    .catch((err) => {
      alert(
        "Unable to reach server at the moment. Please contact administrator"
      );
    });
});

deleteAllCookies = () => {
  var cookies = document.cookie.split(";");
  for (var i = 0; i < cookies.length; i++) {
    var cookie = cookies[i];
    var pos = cookie.indexOf("=");
    var name = pos > -1 ? cookie.substr(0, pos) : cookie;
    document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
  }
};

//Logout FUNCTION Faculty
$("#logOutFacultyBtn").click(function (e) {
  e.preventDefault();
  localStorage.clear();
  deleteAllCookies();
  window.location.replace("/pages/faculty/login-faculty.html");
});

// HOME
var imageList = [];

$("#todoImage").on("change", function () {
  var imageContainer = $("#imageContainer");
  for (var i = 0; i < this.files.length; i++) {
    imageList.push(this.files[i]);
    var reader = new FileReader();
    reader.onload = function (e) {
      var item = `<div class="NewImageBox">
                    <img src="${
                      e.target.result
                    }" style="max-width: 100%; height: auto; margin-bottom: 20px;">
                    <button class="btn delete-button"  onclick="_RemoveImage(this, ${
                      imageList.length - 1
                    })">
                      <i class="fa fa-times"></i>
                    </button>
                  </div>`;
      imageContainer.append(item);
    };
    reader.readAsDataURL(this.files[i]);
  }
});
$("#AddTaskBtn").on("click", () => {
  SET_Task();
});
$("#FABBtn").on("click", ()=>{
  AddTaskModal();
})

AddTaskModal = () =>{
  $("#taskID").empty();
  $("#todoTitle").val("");
  $("#todoDesc").empty();
  $("#todoColor").val(0);
  $("#todoDueDate").empty();
  $("#imageContainer").empty();
  $("#addTODOModal").modal("toggle");
}
// remove image from card during addition
_RemoveImage = (context, index) => {
  imageList.splice(index, 1);
  context.parentNode.remove();
};

SET_Task = () => {
  debugger
  var count = 1;
  var taskID = $("#taskID").text() == "" ? "new" : $("#taskID").text();
  console.log(taskID)
  var title = $("#todoTitle").val();
  var desc = $("#todoDesc").text();
  var color = $("#todoColor option:selected").val();
  var dueDateTime = $("#todoDueDate").val();
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;

  var formdata = new FormData();
  formdata.append("T_TASKID", taskID);
  formdata.append("T_TITLE", title);
  formdata.append("T_DESC", desc);
  formdata.append("T_STATUS", "Added");
  formdata.append("T_COLOR", color);
  formdata.append("T_DUE_DATE_TIME", dueDateTime);
  formdata.append("T_ADDED_DATE_TIME", (new Date()).toISOString().slice(0, 16));
  imageList.forEach((image) => {
    formdata.append(`Files[${count}]`, image);
    count++;
  });

  fetch(`${BASE_URL}/api/user/add-card`, {
    method: "POST",
    headers: {
      Authorization: token,
    },
    body: formdata,
  })
    .then((result) => {
      return result.json();
    })
    .then((response) => {
      if (response.status == "Redirect")
        window.location.pathname = "/pages/login.html";
      else if (response.status == "FAIL") {
        alert(response.msg);
      } else if (response.status == "PASS") {
        window.location.reload();
      }
    })
    .catch((err) => {
      alert("Unable to connect to server. Please contact administrator.");
    });
};

GET_Tasks = (status) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;
  var container = $("#cards-container");
  fetch(`${BASE_URL}/api/user/get-cards?state=${status}`, {
    method: "GET",
    headers: {
      Authorization: token,
      "Content-Type": "application/json",
    },
  })
    .then((result) => {
      return result.json();
    })
    .then((response) => {
      if (response.status == "Redirect")
        window.location.pathname = "/pages/login.html";
      else if (response.status == "FAIL") {
        console.log(response.msg);
      } else if (response.status == "PASS") {
        var data = response.response;
        container.empty();
        $.each(data, function (index, element) {
          try {
            var item = `<div class="card shadow-lg card-todo col-md-3 col-lg-3 col-sm-12" style="background-color: ${
              element.color
            }">
          <div class="card-body">
            <div class="image-grid">
              ${
                element.imageList != null
                  ? element.imageList
                      .map(function (IMG, index) {
                        return `<div class="image-wrapper"><img src="${BASE_URL}${IMG.imagePath}" alt="Image ${index}" /></div>`;
                      })
                      .join("")
                  : ""
              }
            </div>
            <div class="card-details-container">
              <p class="card-text card-heading">${element.title}</p>
              <p class="card-text card-desc">
                ${element.desc}
              </p>
            </div>
            <!-- ACTIONS STRIP -->
            <div class="card-todo-action-strip col-12">
              <li class="todo-action" onclick="_RemoveTask('${
                element.taskID
              }')">
                <i class="fa fa-trash"></i>
              </li>
              <li class="todo-action" onclick="_ArchiveTask('${
                element.taskID
              }')">
                <i class="fa fa-archive" ></i>
              </li>
              <li class="todo-action dropup">
                <i class="fa fa-paint-brush" data-toggle="dropdown"></i>
                <div
                  class="color-palette dropdown-menu"
                  style="position: absolute; top: 1px; z-index: 9999"
                  tabindex="-1"
                >
                <div onClick="_changeColor(this,'${
                  element.taskID
                }')" class="bg-white"></div>
                  <div onClick="_changeColor(this,'${
                    element.taskID
                  }')" class="bg-red"></div>
                  <div onClick="_changeColor(this,'${
                    element.taskID
                  }')" class="bg-orange"></div>
                  <div onClick="_changeColor(this,'${
                    element.taskID
                  }')" class="bg-yellow"></div>
                  <div onClick="_changeColor(this,'${
                    element.taskID
                  }')" class="bg-green"></div>
                  <div onClick="_changeColor(this,'${
                    element.taskID
                  }')" class="bg-turquoise"></div>
                  <div onClick="_changeColor(this,'${
                    element.taskID
                  }')" class="bg-blue"></div>
                  <div onClick="_changeColor(this,'${
                    element.taskID
                  }')" class="bg-dark-blue"></div>
                  <div onClick="_changeColor(this,'${
                    element.taskID
                  }')" class="bg-purple"></div>
                  <div onClick="_changeColor(this,'${
                    element.taskID
                  }')" class="bg-pink"></div>
                  <div onClick="_changeColor(this,'${
                    element.taskID
                  }')" class="bg-brown"></div>
                  <div onClick="_changeColor(this, '${
                    element.taskID
                  }')" class="bg-grey"></div>
                </div>
              </li>
              <li class="todo-action" onclick="_CompleteTask('${
                element.taskID
              }')">
                <i class="fa fa-check-circle"></i>
              </li>
              <btn class="todo-action" onclick="ViewTask('${
                element.taskID
              }')">
                <i class="fa fa-pencil"></i>
              </btn>
            </div>
          </div>
        </div>`;
          } catch (error) {
            console.log(error);
          }

          container.append(item);
        });
      }
    })
    .catch((err) => {
      alert("Unable to connect to server. Please contact administrator.");
    });
};

ViewTask = (taskID) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;
  fetch(`${BASE_URL}/api/user/get-single-task?taskID=${taskID}`, {
    method: "GET",
    headers: {
      Authorization: token,
      "Content-Type": "application/json",
    },
  })
    .then((result) => {
      return result.json();
    })
    .then((response) => {
      if (response.status == "Redirect")
        window.location.pathname = "/pages/login.html";
      else if (response.status == "FAIL") {
        console.log(response.msg);
      } else if (response.status == "PASS") {
        debugger
        var data = response.response;
        $("#taskID").text(data.taskID);
        $("#todoTitle").val(data.title);
        $("#todoDesc").text(data.desc);
        $("#todoColor").val(data.color);
        $("#todoDueDate").val(data.dueDate);
        var imageContainer = $("#imageContainer");
        $("#addTODOModal").modal("toggle");
        $.each(data.imageList, function (index, element) {
          imageList.push(element);
          var item = `<div class="NewImageBox">
                          <img src="${BASE_URL}${
            element.imagePath
          }" style="max-width: 100%; height: auto; margin-bottom: 20px;">
                          <button class="btn delete-button"  onclick="_RemoveImage(this, ${
                            imageList.length - 1
                          })">
                            <i class="fa fa-times"></i>
                          </button>
                        </div>`;
          imageContainer.append(item);
        });
      }
    })
    .catch((err) => {
      alert("Unable to connect to server. Please contact administrator.");
    });
};

_RemoveTask = (taskID) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;

  fetch(`${BASE_URL}/api/user/update-status`, {
    method: "POST",
    headers: {
      Authorization: token,
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      taskID: taskID,
      status: "Deleted",
    }),
  })
    .then((result) => {
      return result.json();
    })
    .then((response) => {
      if (response.status == "Redirect")
        window.location.pathname = "/pages/login.html";
      else if (response.status == "FAIL") {
        alert(response.msg);
      } else if (response.status == "PASS") {
        alert("Task moved to trash");
        window.location.reload();
      }
    })
    .catch((err) => {
      alert("Unable to connect to server. Please contact administrator.");
    });
};

_ArchiveTask = (taskID) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;

  fetch(`${BASE_URL}/api/user/update-status`, {
    method: "POST",
    headers: {
      Authorization: token,
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      taskID: taskID,
      status: "Archived",
    }),
  })
    .then((result) => {
      return result.json();
    })
    .then((response) => {
      if (response.status == "Redirect")
        window.location.pathname = "/pages/login.html";
      else if (response.status == "FAIL") {
        alert(response.msg);
      } else if (response.status == "PASS") {
        alert("Task moved to archive.");
        window.location.reload();
      }
    })
    .catch((err) => {
      alert("Unable to connect to server. Please contact administrator.");
    });
};

//Most browsers seem to return the RGB value
//Function to Convert RGB to Hex Code
function rgb2hex(rgb) {
  rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
  function hex(x) {
    return ("0" + parseInt(x).toString(16)).slice(-2);
  }
  return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
}

_changeColor = (context, taskID) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;
  var colorSelected;
  colorSelected = window
    .getComputedStyle(context, null)
    .getPropertyValue("background-color");
  colorSelected = rgb2hex(colorSelected);
  context.parentElement.parentElement.parentElement.parentElement.parentElement.style.backgroundColor =
    colorSelected;

  fetch(`${BASE_URL}/api/user/update-color`, {
    method: "POST",
    headers: {
      Authorization: token,
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      taskID: taskID,
      color: colorSelected,
    }),
  })
    .then((result) => {
      return result.json();
    })
    .then((response) => {
      if (response.status == "Redirect")
        window.location.pathname = "/pages/login.html";
      else if (response.status == "FAIL") {
        alert(response.msg);
      } else if (response.status == "PASS") {
      }
    })
    .catch((err) => {
      alert("Unable to connect to server. Please contact administrator.");
    });
};

_CompleteTask = (taskID) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;

  fetch(`${BASE_URL}/api/user/update-status`, {
    method: "POST",
    headers: {
      Authorization: token,
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      taskID: taskID,
      status: "Completed",
    }),
  })
    .then((result) => {
      return result.json();
    })
    .then((response) => {
      if (response.status == "Redirect")
        window.location.pathname = "/pages/login.html";
      else if (response.status == "FAIL") {
        alert(response.msg);
      } else if (response.status == "PASS") {
        alert("Task Completed.");
        window.location.reload();
      }
    })
    .catch((err) => {
      alert("Unable to connect to server. Please contact administrator.");
    });
};

// TRASH
_DeleteTask = (taskID) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;

  fetch(`${BASE_URL}/api/user/delete-card?taskID=${taskID}`, {
    method: "POST",
    headers: {
      Authorization: token,
    },
  })
    .then((result) => {
      return result.json();
    })
    .then((response) => {
      if (response.status == "Redirect")
        window.location.pathname = "/pages/login.html";
      else if (response.status == "FAIL") {
        alert(response.msg);
      } else if (response.status == "PASS") {
        window.location.reload();
      }
    })
    .catch((err) => {
      alert("Unable to connect to server. Please contact administrator.");
    });
};
