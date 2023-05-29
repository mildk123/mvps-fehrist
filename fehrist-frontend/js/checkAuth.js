// checkAuth.js
window.addEventListener("DOMContentLoaded", function () {
  if (isUserAuthenticated()) {
  } else if (
    location.pathname == "/pages/login.html" ||
    location.pathname == "/pages/register.html"
  ) {
    localStorage.clear();
    deleteAllCookies();
  } else if (
    location.pathname !== "/pages/login.html" &&
    location.pathname !== "/pages/register.html" &&
    location.pathname !== "/"
  ) {
    window.location.replace("/");
  } else {
    localStorage.clear();
    deleteAllCookies();
    window.location.replace("/pages/login.html");
  }
});

if (location.pathname == "/pages/register.html") {
  // Initialize FB Login
  document.addEventListener("DOMContentLoaded", function () {
    var script = document.createElement("script");
    script.src = "https://connect.facebook.net/en_US/sdk.js";
    script.async = true;
    script.defer = true;
    script.onload = function () {
      FB.init({
        appId: "793701339153959",
        version: "v2.7",
      });
    };
    document.head.appendChild(script);
  });

  // FB Login on Click
  $("#FBLoginBtn").on("click", () => {
    FB.login(
      function (response) {
        if (response.status === "connected") {
          // Logged into your webpage and Facebook.
          FB.api("/me", { fields: "name,email" }, function (response) {
            console.log(response);
            $("#RegNameBox").val(response.name);
            $("#RegEmailBox").val(response.email);
            Swal.fire({
              icon: "info",
              title: "Information",
              text: "Passwords must be set manually!",
            });
          });
        }
      },
      { scope: "email" }
    );
  });
  // Google Login on Click
  $("#GPLoginBtn").on("click", () => {
    google.accounts.id.initialize({
      client_id:
        "1050384600148-cg2luaft472hsre0tbgnktfvot6j54ue.apps.googleusercontent.com",
      native_callback: loginGooglePlus,
      callback: loginGooglePlus,
    });
    google.accounts.id.prompt();
  });
  // Google Login on callback
  function loginGooglePlus(response) {
    console.log(response);
    if (response.credential) {
      var credToken = response.credential;
      var decoded = jwt_decode(credToken);
      console.log(decoded);
      $("#RegNameBox").val(decoded.name);
      $("#RegEmailBox").val(decoded.email);
      Swal.fire({
        icon: "info",
        title: "Information",
        text: "Passwords must be set manually!",
      });
    } else {
      console.log("Google Sign-in was not successful.");
    }
  }
}

var BASE_URL = "http://localhost:56067";

deleteAllCookies = () => {
  var cookies = document.cookie.split(";");
  for (var i = 0; i < cookies.length; i++) {
    var cookie = cookies[i];
    var pos = cookie.indexOf("=");
    var name = pos > -1 ? cookie.substr(0, pos) : cookie;
    document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
  }
};

isUserAuthenticated = () => {
  const tokenString = getCookie("FehristCookie");
  var tokenJson = JSON.parse(tokenString);
  if (tokenJson) {
    return fetch(BASE_URL + "/api/token/verify", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + tokenJson.token.data,
      },
      body: JSON.stringify({}),
    })
      .then((res) => {
        if (res.status == 200) return res.json();
        else if (res.status == 400)
        clearAllCookies();
          Swal.fire("Error", "Login session is invalid or expired! clear browser cache and try again  !");
      })
      .then((response) => {
        if (response == "valid") {
          return true;
        } else {
          return false;
        }
      })
      .catch((error) => {
        // An error occurred while verifying the token
        console.error("Error verifying token:", error);
        return false;
      });
  } else {
    // No token found, user is not authenticated
    return false;
  }
}

// Helper function to retrieve a cookie value by name
function getCookie(name) {
  const cookieString = document.cookie;
  const cookies = cookieString.split("; ");
  for (let i = 0; i < cookies.length; i++) {
    const [cookieName, cookieValue] = cookies[i].split("=");
    if (cookieName === name) {
      return cookieValue;
    }
  }
  return null;
}

clearAllCookies = () => {
  var cookies = document.cookie.split(";");
  for (var i = 0; i < cookies.length; i++) {
    var cookie = cookies[i];
    var pos = cookie.indexOf("=");
    var name = pos > -1 ? cookie.substr(0, pos) : cookie;
    document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
  }
};