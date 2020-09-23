mergeInto(LibraryManager.library, {

  SendDataJs: function (data) {
    data = Pointer_stringify(data)
    window.sendData(data);
  },

  InitializeConnectionJs: function () {
    window.initializeConnection();
  },

});