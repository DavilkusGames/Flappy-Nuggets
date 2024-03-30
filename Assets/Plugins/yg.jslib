﻿mergeInto(LibraryManager.library, {
  SDKInit: function () {
    if (typeof ysdk === 'undefined') {
        return false;
    }
    else {
        return true;
    }
  },

  PlayerInit: function () {
    if (typeof player === 'undefined') {
        return false;
    }
    else {
        return true;
    }
  },

  AuthCheck: function () {
    if (player.getMode() === 'lite') {
       return false; 
    }
    else {
       return true;
    }
  },

  GameReady: function () {
    ysdk.features.LoadingAPI.ready();
  },

  GetLang: function () {
    var lang = ysdk.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

  ShowFullscreenAd : function() {
    console.log("Show ad request...");
    ysdk.adv.showFullscreenAdv({
        callbacks: {
            onClose: function(wasShown) {
                console.log("Ad shown");
                myGameInstance.SendMessage("_yandexGames", "AdShown");
            },
            onError: function(error) {
                console.log("Ad error:", error);
                myGameInstance.SendMessage("_yandexGames", "AdShown");
            }
        }
    })
  },

  SaveToLb : function (score) {
    lb.setLeaderboardScore('flappynuggetscore', score);
  },

  SaveCloudData : function (data) {
    var dataConverted = UTF8ToString(data);
    var dataObj = JSON.parse(dataConverted);
    player.setData(dataObj);
    myGameInstance.SendMessage("_yandexGames", "DataSaved");
  },

  LoadCloudData : function () {
    player.getData().then(_data => {
        const dataJSON = JSON.stringify(_data);
        myGameInstance.SendMessage("_yandexGames", "DataLoaded", dataJSON);
    });
  },
});