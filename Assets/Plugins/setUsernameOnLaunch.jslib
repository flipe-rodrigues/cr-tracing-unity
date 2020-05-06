mergeInto(LibraryManager.library,
{
  SetUsernameOnLaunch: function ()
  {
    unityInstance.SendMessage("Username Label", "SetUsername", window.username);
  }
});
