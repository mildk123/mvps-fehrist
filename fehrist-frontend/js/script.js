document.getElementById('todoImage').addEventListener('change', function() {
    var reader = new FileReader();
    reader.onload = function(e) {
      document.getElementById('selectedImage').setAttribute('src', e.target.result);
      document.getElementById('selectedImage').style.display = 'block';
    };
    reader.readAsDataURL(this.files[0]);
  });