mergeInto(LibraryManager.library, {

  SendDataJS: function (data) {
    data = Pointer_stringify(data)
    window.sendData(data);
  },

  InitializeConnectionJS: function () {
    window.initializeConnection();
  },

});