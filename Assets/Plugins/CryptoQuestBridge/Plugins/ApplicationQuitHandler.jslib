mergeInto(LibraryManager.library, {
    /**
     * Register OnBeforeUnload event callback
     * @param objectName Unity GameObject's name
     * @param callback Callback to call
     */    
    RegisterOnBeforeUnloadEventCallback: function (objectName, callback) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedObjectName = UTF8ToString(objectName);
        console.log("RegisterOnBeforeUnloadEventCallback: " + parsedObjectName + ", " + parsedCallback);
        window.onbeforeunload = function() {
           console.log("RegisterOnBeforeUnloadEventCallback: send message to Unity ");
           window.unityInstance.SendMessage(parsedObjectName, parsedCallback);
           return "Are you sure you really want to leave the application?";
        };
    },
    /**
     * Register OnBlur event callback
     * @param objectName Unity GameObject's name
     * @param callback Callback to call
     */    
    RegisterOnFocusChangedEventCallback: function (objectName, callback) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedObjectName = UTF8ToString(objectName);
        console.log("RegisterOnFocusChangedEventCallback: " + parsedObjectName + ", " + parsedCallback);
        window.onblur = function() {
           console.log("RegisterOnFocusChangedEventCallback: focus lost ");
           window.unityInstance.SendMessage(parsedObjectName, parsedCallback, 0);
        };
        window.onfocus = function() {
           console.log("RegisterOnFocusChangedEventCallback: focus gain ");
           window.unityInstance.SendMessage(parsedObjectName, parsedCallback, 1);
        };
    },

});