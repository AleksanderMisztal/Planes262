mergeInto(LibraryManager.library, {

  SendDataJs: function (data, length) {
    window.sendData(data, length);
  },

  InitializeConnectionJs: function () {
    window.initializeConnection();
  },
});