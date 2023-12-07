
mergeInto(LibraryManager.library, {

  Token: function () {
    console.log(localStorage.getItem("token"));
    console.log(window.localStorage.getItem("token"));
    
    var returnStr = localStorage.getItem("token");
    if(returnStr == null) return null;
    
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    
    return buffer;
  },

  UidFromUrl: function () {
    var url = window.location.href;
    var cleanedUrl = url.replace(/\/+$/, "");
    var parts = cleanedUrl.split('/');
    
    for (let i = parts.length - 1; i >= 0; i--) {
        if (parts[i] !== "") {
           var returnStr = parts[i];
           var bufferSize = lengthBytesUTF8(returnStr) + 1;
           var buffer = _malloc(bufferSize);
           stringToUTF8(returnStr, buffer, bufferSize);
           return buffer;
        }
    }
    
    return null;
  }

});