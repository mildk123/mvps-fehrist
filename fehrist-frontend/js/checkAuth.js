// checkAuth.js
window.addEventListener("DOMContentLoaded", function () {
  if (isUserAuthenticated()) {
  }else if (
    location.pathname == "/pages/login.html" ||
    location.pathname == "/pages/register.html"
  ) {
    localStorage.clear();
    deleteAllCookies();
  }
  else if (
    location.pathname !== "/pages/login.html" &&
    location.pathname !== "/pages/register.html" &&
    location.pathname !== "/"
  ) {
      window.location.replace("/");
  }else{
    
    localStorage.clear();
    deleteAllCookies();
    window.location.replace("/pages/login.html");
  }
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

function isUserAuthenticated() {
  const tokenString = getCookie("FehristCookie");
  var tokenJson = JSON.parse(tokenString);
  if (tokenJson) {
    return fetch(BASE_URL + "/api/user/verify-token", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + tokenJson.token.data,
      },
      body: JSON.stringify({}),
    })
      .then((res) => {
        return res.json();
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
