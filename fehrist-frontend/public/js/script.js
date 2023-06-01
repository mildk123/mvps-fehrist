// PERMENANTLY SET LOADING ICON TO FALSE, UNLESS CALLED WITHIN A FUNCTION
$("#LoadingIcon").css("display", "none");

// ONLOAD FUNCTION THAT CALLS VARIOUS ACTIONS BASE ON PATHNAME.
window.addEventListener("load", (event) => {
  if (location.pathname == "/" || location.pathname == "/index.html") {
    GET_Tasks("Added");
    let timerInterval;
    Swal.fire({
      title: "Loading!",
      timer: 500,
      timerProgressBar: true,
      didOpen: () => {
        Swal.showLoading();
      },
      willClose: () => {
        clearInterval(timerInterval);
      },
    });

    // GETS THE CURRENT SELECTED COLOR IN ADD TASK MODAL AND SETS IT TO MODAL BACKGROUND
    var selectElement = document.getElementById("todoColor");
    var colorNameElement = document.getElementById("modalCont");
    selectElement.addEventListener("change", function () {
      var selectedColor = selectElement.value;
      colorNameElement.style.backgroundColor = selectedColor;
    });
  } else if (location.pathname == "/pages/trash.html") {
    GET_Tasks("Deleted");
    let timerInterval;
    Swal.fire({
      title: "Loading!",
      timer: 500,
      timerProgressBar: true,
      didOpen: () => {
        Swal.showLoading();
      },
      willClose: () => {
        clearInterval(timerInterval);
      },
    });
  } else if (location.pathname == "/pages/archive.html") {
    GET_Tasks("Archived");
    let timerInterval;
    Swal.fire({
      title: "Loading!",
      timer: 500,
      timerProgressBar: true,
      didOpen: () => {
        Swal.showLoading();
      },
      willClose: () => {
        clearInterval(timerInterval);
      },
    });
  } else if (location.pathname == "/pages/completed.html") {
    GET_Tasks("Completed");
    let timerInterval;
    Swal.fire({
      title: "Loading!",
      timer: 500,
      timerProgressBar: true,
      didOpen: () => {
        Swal.showLoading();
      },
      willClose: () => {
        clearInterval(timerInterval);
      },
    });
  }
});

var BASE_URL = "https://www.fehrist.somee.com";

// FUNCTION CALLED FROM OTHER FUNCTION THE VERIFY EMAIL FORMAT AS PER THE PROVIDED REGEX
function isValidEmail(email) {
  // Regular expression to validate email format
  var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  // Test the email against the regex
  return emailRegex.test(email);
}

// AUTHORIZATION
// SIGN-UP FUNCTION
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
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "Requried Fields marked with * cannot be empty !!",
    });
    return;
  }
  if (!isValidEmail(email)) {
    // Invalid email format
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "Please enter a valid email address !",
    });
    return;
  }
  if (pass !== cpass) {
    // Passwords do not match
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "Passwords do not match. Please try again!",
    });
    return;
  }

  $("#LoadingIcon").css("display", "block");
  $("#SignupBtn").css("display", "none");

  fetch(`${BASE_URL}/api/user/register`, {
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
      if (res.status == "FAIL")
        Swal.fire({
          icon: "error",
          title: "Error...",
          text: res.msg,
        });
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
            Swal.fire({
              icon: "error",
              title: "Error...",
              text: "Invalid ID/Password. Please login using correct information",
            });
            break;
        }
        window.location.replace("/");
      }
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Unable to reach server at the moment. Please check your internet connection!",
        footer: err.message,
      });
    })
    .finally(() => {
      $("#LoadingIcon").css("display", "none");
      $("#SignupBtn").css("display", "block");
    });
});
// SIGN-IN FUNCTION
$("#LoginBtn").on("click", () => {
  var email = $("#LoginEmailBox").val();
  var pass = $("#LoginPassBox").val();

  if (email.trim() === "" || pass.trim() === "") {
    // email is empty
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "Requried Fields marked with * cannot be empty !",
    });
    return;
  }
  if (!isValidEmail(email)) {
    // Invalid email format
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "Please enter a valid email address.",
    });
    return;
  }

  $("#LoadingIcon").css("display", "block");
  $("#LoginBtn").css("display", "none");

  fetch(`${BASE_URL}/api/user/login`, {
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
      if (res.status == "FAIL")
        Swal.fire({
          icon: "error",
          title: "Error",
          text: res.msg,
        });
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
            Swal.fire({
              icon: "error",
              title: "Error",
              text: "Invalid ID/Password. Please login using correct information.",
            });
            break;
        }
        window.location.replace("/");
      }
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Unable to reach server at the moment. Please check your internet connection!",
        footer: err.message,
      });
    })
    .finally(() => {
      $("#LoadingIcon").css("display", "none");
      $("#LoginBtn").css("display", "block");
    });
});

// FUNCTION CALLED ON LOGOUT TO CLEAN BROWSER FROM COOKIES
deleteAllCookies = () => {
  var cookies = document.cookie.split(";");
  for (var i = 0; i < cookies.length; i++) {
    var cookie = cookies[i];
    var pos = cookie.indexOf("=");
    var name = pos > -1 ? cookie.substr(0, pos) : cookie;
    document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
  }
};

//BASE LOGOUT FUNCTION
$("#BtnLogout").click(function (e) {
  e.preventDefault();
  localStorage.clear();
  deleteAllCookies();
  window.location.replace("/pages/login.html");
});

// HOME
var imageList = []; // THIS LIST MAINTAINES THE IMAGES FETCHED FROM DB TO UNTIL VIEWED ON MODAL
var prevImageList = []; // THIS LIST MAINTAINES THE IMAGES FETCHED FROM DB THAT ARE RETURN TO DB INCASE OF UPDATE TASK
var checklistItems = [];

// EVENT HANDLING TO ADD IMAGES IN NEW TO-DO
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
// EVENT HANDLING TO DONE BUTTON ON MODAL TO SAVE TO-DO TO DB
$("#AddTaskBtn").on("click", () => {
  $("#LoadingIcon").css("display", "block");
  SET_Task();
});
// EVENT HANDLING TO OPEN ADD TASK MODAL
$("#FABBtn").on("click", () => {
  AddTaskModal();
});
// EVENT HANDLING OF SEARCH BUTTON
$("#SearchBtn").on("click", () => {
  if (location.pathname == "/" || location.pathname == "/index.html") {
    _SearchTask("Added");
  } else if (location.pathname == "/pages/trash.html") {
    _SearchTask("Deleted");
  } else if (location.pathname == "/pages/archive.html") {
    _SearchTask("Archived");
  } else if (location.pathname == "/pages/completed.html") {
    _SearchTask("Completed");
  }
});
// EVENT HANDLING OF SEARCH TEXT CHANGE
$("#SearchBox").on("change", () => {
  if (location.pathname == "/" || location.pathname == "/index.html") {
    _SearchTask("Added");
  } else if (location.pathname == "/pages/trash.html") {
    _SearchTask("Deleted");
  } else if (location.pathname == "/pages/archive.html") {
    _SearchTask("Archived");
  } else if (location.pathname == "/pages/completed.html") {
    _SearchTask("Completed");
  }
});

GetCheckList = () => {
  $('.checklistItem input[name="checklistItem"]').each(function () {
    var itemText = $(this).val();
    checklistItems.push(itemText);
  });
};

// OPEN ADD TASK MODAL AFTER CLEARING ANY PREVIOUS STORED VALUES
AddTaskModal = () => {
  $("#taskID").empty();
  $("#todoTitle").val("");
  $("#todoDesc").empty();
  $("#todoColor").val(0);
  $("#todoDueDate").val("");
  $("#imageContainer").empty();
  imageList = [];
  prevImageList = [];
  checklistItems = [];
  $("#addTODOModal").modal("toggle");
};
// METHOD TO REMOVE IMAGE FROM ADDED LIST TO ADD TASK MODEL BY X BUTTON
_RemoveImage = (context, index) => {
  imageList.splice(index, 1);
  prevImageList.splice(index, 1);
  context.parentNode.remove();
};

// ADD CHECK LIST ITEM
$("#addChecklistItem").click(function () {
  var checklistItem =
    $(` <div class="form-group checklistItem d-flex align-items-center">
  <input
    type="text"
    class="form-control"
    name="checklistItem"
    placeholder="Checklist Item"
  />
  <button class="btn btn-danger deleteChecklistItem">
    <i class="fa fa-times"></i>
  </button>
</div>`);
  $("#checklistContainer").append(checklistItem);
});

// Delete checklist item
$(document).on("click", ".deleteChecklistItem", function () {
  $(this).closest(".checklistItem").remove();
});

// BASE FUNCTION TO SAVE TASK TO DB
SET_Task = () => {
  var count = 1;
  var taskID = $("#taskID").text() == "" ? "new" : $("#taskID").text();
  var title = $("#todoTitle").val();
  var desc = $("#todoDesc").text();
  var color = $("#todoColor option:selected").val();
  var dueDateTime = $("#todoDueDate").val();
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;
  GetCheckList();

  var formdata = new FormData();
  formdata.append("T_TASKID", taskID);
  formdata.append("T_TITLE", title);
  formdata.append("T_DESC", desc);
  formdata.append("T_STATUS", "Added");
  formdata.append("T_COLOR", color);
  formdata.append("T_DUE_DATE_TIME", dueDateTime);
  formdata.append("T_ADDED_DATE_TIME", new Date().toISOString().slice(0, 16));
  formdata.append("T_PREV_IMAGE", JSON.stringify(prevImageList));
  formdata.append("T_CHECKLIST", JSON.stringify(checklistItems));

  imageList.forEach((image) => {
    formdata.append(`Files[${count}]`, image);
    count++;
  });

  fetch(`${BASE_URL}/api/user/tasks/create`, {
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
        Swal.fire({
          icon: "error",
          title: "Error...",
          text: response.msg,
        });
      } else if (response.status == "PASS") {
        window.location.reload();
      }
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Unable to reach server at the moment. Please check your internet connection!",
        footer: err.message,
      });
    })
    .finally(() => {
      $("#LoadingIcon").css("display", "none");
      imageList = [];
      checklistItems = [];
      prevImageList = [];
    });
};

// GET ALL TASKS FROM DB ACCORDING TO THE CURRENT STATUS
GET_Tasks = (status) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;
  var container = $("#cards-container");
  fetch(`${BASE_URL}/api/user/tasks?state=${status}`, {
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
        container.empty();
        container.append("<h1>Oh no... so empty</h1>");
      } else if (response.status == "PASS") {
        var data = response.response;
        container.empty();
        if (location.pathname == "/" || location.pathname == "/index.html") {
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
              <btn class="todo-action" onclick="ViewTask('${element.taskID}')">
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
        } else if (location.pathname == "/pages/trash.html") {
          $.each(data, function (index, element) {
            try {
              var item = ` <div
              class="card shadow-lg card-todo col-md-3 col-lg-3 col-sm-12 isDeleted"
            >
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
                  <li class="todo-action" onclick="_RestoreTask('${
                    element.taskID
                  }')">
                    <span class="material-symbols-outlined">
                      restore_from_trash
                    </span>
                  </li>
                  <li class="todo-action" onclick="_DeleteTask('${
                    element.taskID
                  }')">
                    <span class="material-symbols-outlined">
                      delete_forever
                    </span>
                  </li>
                </div>
              </div>
            </div>`;
            } catch (error) {
              console.log(error);
            }
            container.append(item);
          });
        } else if (location.pathname == "/pages/completed.html") {
          $.each(data, function (index, element) {
            try {
              var item = `<div
              class="card shadow-lg card-todo col-md-3 col-lg-3 col-sm-12 isCompleted"
            >
              <div class="card-body">
                <div class="image-grid">
                  ${
                    element.imageList != null
                      ? element.imageList
                          .map(function (IMG, index) {
                            return `
                  <div class="image-wrapper">
                    <img src="${BASE_URL}${IMG.imagePath}" alt="Image ${index}" />
                  </div>
                  `;
                          })
                          .join("")
                      : ""
                  }
                </div>
                <div class="card-details-container">
                  <p class="card-text card-heading">${element.title}</p>
                  <p class="card-text card-desc">${element.desc}</p>
                </div>
                <!-- ACTIONS STRIP -->
                <div class="card-todo-action-strip col-12">
                  <li
                    class="todo-action"
                    onclick="_DeleteTask('${element.taskID}')"
                  >
                    <span class="material-symbols-outlined"> delete </span>
                  </li>
                </div>
              </div>
            </div>`;
            } catch (error) {
              console.log(error);
            }
            container.append(item);
          });
        } else if (location.pathname == "/pages/archive.html") {
          $.each(data, function (index, element) {
            try {
              var item = `<div
              class="card shadow-lg card-todo col-md-3 col-lg-3 col-sm-12 isArchived"
            >
              <div class="card-body">
                <div class="image-grid">
                  ${
                    element.imageList != null
                      ? element.imageList
                          .map(function (IMG, index) {
                            return `
                  <div class="image-wrapper">
                    <img src="${BASE_URL}${IMG.imagePath}" alt="Image ${index}" />
                  </div>
                  `;
                          })
                          .join("")
                      : ""
                  }
                </div>
                <div class="card-details-container">
                  <p class="card-text card-heading">${element.title}</p>
                  <p class="card-text card-desc">${element.desc}</p>
                </div>
                <!-- ACTIONS STRIP -->
                <!-- ACTIONS STRIP -->
                <div class="card-todo-action-strip col-12">
                  <li class="todo-action" onclick="_DeleteTask('${
                    element.taskID
                  }')"
                  >
                    <span class="material-symbols-outlined"> delete </span>
                  </li>
                  <li class="todo-action" onclick="_RestoreTask('${
                    element.taskID
                  }')"
                  >
                    <span class="material-symbols-outlined"> unarchive </span>
                  </li>
                </div>
              </div>
            </div>`;
            } catch (error) {
              console.log(error);
            }
            container.append(item);
          });
        }
      }
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Unable to reach server at the moment. Please check your internet connection!",
        footer: err.message,
      });
    });
};

// GET DATA FROM DB OF THE CLICKED TASK TO ADD TASK MODEL INCASE OF UPDATES OR CHANGES
ViewTask = (taskID) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;
  fetch(`${BASE_URL}/api/user/tasks?taskID=${taskID}`, {
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
        var imageContainer = $("#imageContainer");
        var checklistContainer = $("#checklistContainer");
        imageContainer.empty();
        checklistContainer.empty();
        prevImageList = [];
        checklistItems = [];
        var data = response.response;
        $("#taskID").text(data.taskID);
        $("#todoTitle").val(data.title);
        $("#todoDesc").text(data.desc);
        $("#todoColor").val(data.color);
        $("#todoDueDate").val(data.dueDate);
        $("#addTODOModal").modal("toggle");
        $.each(data.imageList, function (index, element) {
          prevImageList.push(element);
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
        $.each(data.checkList, (index, element) => {
          var checkItem = `<div
          class="form-group checklistItem d-flex align-items-center"
        >
          <input
            type="text"
            disabled
            class="form-control"
            name="checklistItem"
            value="${element.desc}"
          />
          <button onclick="RemoveCheckDB('${element.checkID}')" class="btn btn-danger deleteChecklistItem">
            <i class="fa fa-times"></i>
          </button>
        </div>`;
          checklistContainer.append(checkItem);
        });
      }
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Unable to reach server at the moment. Please check your internet connection!",
        footer: err.message,
      });
    });
};

// FUNCTION TO DELETE CHECKLIST ITEM FROM DB
RemoveCheckDB = (checkID) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;

  fetch(`${BASE_URL}/api/user/tasks/checks/remove?checkID=${checkID}`, {
    method: "GET",
    headers: {
      Authorization: token,
    },
  })
    .then((result) => {
      return result.json();
    })
    .then((response) => {
      if (response.status == "Redirect") {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: response.msg,
        });
        window.location.pathname = "/pages/login.html";
      } else if (response.status == "FAIL") {
        Swal.fire("Error", "Failed to remove checklist item.");
      } else if (response.status == "PASS") {
      }
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Unable to reach server at the moment. Please check your internet connection!",
        footer: err.message,
      });
    });
};

//CARD ACTIONS
// CHANGES RGB TO HEX CODE FOR DB
function rgb2hex(rgb) {
  rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
  function hex(x) {
    return ("0" + parseInt(x).toString(16)).slice(-2);
  }
  return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
}
// TO CHANGE THE BACKGROUND COLOR OF TASK BASED ON THE CURRENT SELECTED COLOR
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

  fetch(`${BASE_URL}/api/user/tasks/update-color`, {
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
        Swal.fire({
          icon: "error",
          title: "Error Occured!",
          text: response.msg,
        });
      } else if (response.status == "PASS") {
      }
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Unable to reach server at the moment. Please check your internet connection!",
        footer: err.message,
      });
    });
};
// SETS THE STATUS OF SELECTED TASK TO COMPLETED SAVE TO DB
_CompleteTask = (taskID) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;

  fetch(`${BASE_URL}/api/user/tasks/update-status`, {
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
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: response.msg,
        });
      } else if (response.status == "PASS") {
        window.location.reload();
      }
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Unable to reach server at the moment. Please check your internet connection!",
        footer: err.message,
      });
    });
};
// SETS THE STATUS OF SELECTED TASK TO ARCHIVED SAVE TO DB
_ArchiveTask = (taskID) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;

  fetch(`${BASE_URL}/api/user/tasks/update-status`, {
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
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: response.msg,
        });
      } else if (response.status == "PASS") {
        window.location.reload();
      }
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Unable to reach server at the moment. Please check your internet connection!",
        footer: err.message,
      });
    });
};

// DELETES THE TASK FROM DB AND VIEW
_DeleteTask = (taskID) => {
  // hard delete
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;

  fetch(`${BASE_URL}/api/user/tasks/remove?taskID=${taskID}`, {
    method: "GET",
    headers: {
      Authorization: token,
    },
  })
    .then((result) => {
      return result.json();
    })
    .then((response) => {
      if (response.status == "Redirect") {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: response.msg,
        });
        window.location.pathname = "/pages/login.html";
      } else if (response.status == "FAIL") {
      } else if (response.status == "PASS") {
        window.location.reload();
      }
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Unable to reach server at the moment. Please check your internet connection!",
        footer: err.message,
      });
    });
};
// SETS THE STATUS OF SELECTED TASK TO DELETED, SAVES TO DB
_RemoveTask = (taskID) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;

  fetch(`${BASE_URL}/api/user/tasks/update-status`, {
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
      if (response.status == "Redirect") {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: response.msg,
        });
        window.location.pathname = "/pages/login.html";
      } else if (response.status == "FAIL") {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: response.msg,
        });
      } else if (response.status == "PASS") {
        window.location.reload();
      }
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Unable to reach server at the moment. Please check your internet connection!",
        footer: err.message,
      });
    });
};
// SETS THE STATUS OF SELECTED TASK TO ADDED, SAVES TO DB
_RestoreTask = (taskID) => {
  var userProfile = document.cookie;
  var cookieValue = JSON.parse(userProfile.split("=")[1]);
  var token = "Bearer " + cookieValue.token.data;

  fetch(`${BASE_URL}/api/user/tasks/update-status`, {
    method: "POST",
    headers: {
      Authorization: token,
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      taskID: taskID,
      status: "Added",
    }),
  })
    .then((result) => {
      return result.json();
    })
    .then((response) => {
      if (response.status == "Redirect")
        window.location.pathname = "/pages/login.html";
      else if (response.status == "FAIL") {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: response.msg,
        });
      } else if (response.status == "PASS") {
        window.location.reload();
      }
    })
    .catch((err) => {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Unable to reach server at the moment. Please check your internet connection!",
        footer: err.message,
      });
    });
};
// SEARCHES THE TERM ENTERED IN DB BASED UPON SELECTED STATUS, (COMPLETED,ARCHIVED,ADDED,DELETED)
_SearchTask = (status) => {
  var term = $("#SearchBox").val();
  if (term.length >= 3) {
    let timerInterval;
    Swal.fire({
      timer: 1000,
      timerProgressBar: true,
      didOpen: () => {
        Swal.showLoading();
      },
      willClose: () => {
        clearInterval(timerInterval);
      },
    });
    var userProfile = document.cookie;
    var cookieValue = JSON.parse(userProfile.split("=")[1]);
    var token = "Bearer " + cookieValue.token.data;
    var container = $("#cards-container");

    fetch(`${BASE_URL}/api/user/tasks/?searchTerm=${term}&status=${status}`, {
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
          container.empty();
          container.append("<h1>Oh no... so empty</h1>");
        } else if (response.status == "PASS") {
          var data = response.response;
          container.empty();
          if (location.pathname == "/" || location.pathname == "/index.html") {
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
          } else if (location.pathname == "/pages/trash.html") {
            $.each(data, function (index, element) {
              try {
                var item = ` <div
                class="card shadow-lg card-todo col-md-3 col-lg-3 col-sm-12 isDeleted"
              >
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
                    <li class="todo-action" onclick="_RestoreTask('${
                      element.taskID
                    }')">
                      <span class="material-symbols-outlined">
                        restore_from_trash
                      </span>
                    </li>
                    <li class="todo-action" onclick="_DeleteTask('${
                      element.taskID
                    }')">
                      <span class="material-symbols-outlined">
                        delete_forever
                      </span>
                    </li>
                  </div>
                </div>
              </div>`;
              } catch (error) {
                console.log(error);
              }
              container.append(item);
            });
          } else if (location.pathname == "/pages/completed.html") {
            $.each(data, function (index, element) {
              try {
                var item = `<div
                class="card shadow-lg card-todo col-md-3 col-lg-3 col-sm-12 isCompleted"
              >
                <div class="card-body">
                  <div class="image-grid">
                    ${
                      element.imageList != null
                        ? element.imageList
                            .map(function (IMG, index) {
                              return `
                    <div class="image-wrapper">
                      <img src="${BASE_URL}${IMG.imagePath}" alt="Image ${index}" />
                    </div>
                    `;
                            })
                            .join("")
                        : ""
                    }
                  </div>
                  <div class="card-details-container">
                    <p class="card-text card-heading">${element.title}</p>
                    <p class="card-text card-desc">${element.desc}</p>
                  </div>
                  <!-- ACTIONS STRIP -->
                  <div class="card-todo-action-strip col-12">
                    <li
                      class="todo-action"
                      onclick="_DeleteTask('${element.taskID}')"
                    >
                      <span class="material-symbols-outlined"> delete </span>
                    </li>
                  </div>
                </div>
              </div>`;
              } catch (error) {
                console.log(error);
              }
              container.append(item);
            });
          } else if (location.pathname == "/pages/archive.html") {
            $.each(data, function (index, element) {
              try {
                var item = `<div
                class="card shadow-lg card-todo col-md-3 col-lg-3 col-sm-12 isArchived"
              >
                <div class="card-body">
                  <div class="image-grid">
                    ${
                      element.imageList != null
                        ? element.imageList
                            .map(function (IMG, index) {
                              return `
                    <div class="image-wrapper">
                      <img src="${BASE_URL}${IMG.imagePath}" alt="Image ${index}" />
                    </div>
                    `;
                            })
                            .join("")
                        : ""
                    }
                  </div>
                  <div class="card-details-container">
                    <p class="card-text card-heading">${element.title}</p>
                    <p class="card-text card-desc">${element.desc}</p>
                  </div>
                  <!-- ACTIONS STRIP -->
                  <!-- ACTIONS STRIP -->
                  <div class="card-todo-action-strip col-12">
                    <li class="todo-action" onclick="_DeleteTask('${
                      element.taskID
                    }')"
                    >
                      <span class="material-symbols-outlined"> delete </span>
                    </li>
                    <li class="todo-action" onclick="_RestoreTask('${
                      element.taskID
                    }')"
                    >
                      <span class="material-symbols-outlined"> unarchive </span>
                    </li>
                  </div>
                </div>
              </div>`;
              } catch (error) {
                console.log(error);
              }
              container.append(item);
            });
          }
        }
      })
      .catch((err) => {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Unable to reach server at the moment. Please check your internet connection!",
          footer: err.message,
        });
      });
  } else {
    if (term == "") {
      GET_Tasks(status);
      return;
    }
    Swal.fire("Cannot Search", "Search must be atleast 3 characters long!");
    return;
  }
};
