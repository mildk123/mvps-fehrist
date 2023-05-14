// ADD IMAGE TO NEW TASK
document.getElementById("todoImage").addEventListener("change", function () {
  var imageContainer = $("#imageContainer");
  for (var i = 0; i < this.files.length; i++) {
    var reader = new FileReader();
    reader.onload = function (e) {
      var item = `<div class="NewImageBox">
                    <img src="${e.target.result}" style="max-width: 100%; height: auto; margin-bottom: 20px;">
                    <button class="btn delete-button"  onclick="removeItem(this)">
                      <i class="fa fa-times"></i>
                    </button>
                  </div>`;
      imageContainer.append(item);
    };
    reader.readAsDataURL(this.files[i]);
  }
});

// remove image from card during addition
removeItem = (context) => {
  context.parentNode.remove();
};

// change color of card
document.querySelectorAll(".color-palette div").forEach((element) => {
  // Wire up a click listener on every color
  element.addEventListener("click", (context) => {
    // Unselect every other color
    element.parentElement.parentElement.parentElement.parentElement.parentElement.style.backgroundColor = window.getComputedStyle(element, null).getPropertyValue("background-color");
  });
});
