mergeInto(LibraryManager.library, {    
    CreateUserWithEmailAndPassword: function(email, password, objectName, callback, fallback) {
        try {
            const parsedEmail = UTF8ToString(email);
            const parsedPassword = UTF8ToString(password);
            const parsedObjectName = UTF8ToString(objectName);
            const parsedCallback = UTF8ToString(callback);
            const parsedFallback = UTF8ToString(fallback);        
            firebase.auth().createUserWithEmailAndPassword(parsedEmail, parsedPassword).then((result) => {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(result.user));
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            })
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
    SignInWithEmailAndPassword: function(email, password, objectName, callback, fallback) {
        try {
            const parsedEmail = UTF8ToString(email);
            const parsedPassword = UTF8ToString(password);
            const parsedObjectName = UTF8ToString(objectName);
            const parsedCallback = UTF8ToString(callback);
            const parsedFallback = UTF8ToString(fallback);
            firebase.auth().signInWithEmailAndPassword(parsedEmail, parsedPassword).then((result) => {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(result.user));
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            })
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
    SignInWithGoogle: function(objectName, callback, fallback) {
        try {
            const parsedObjectName = UTF8ToString(objectName);
            const parsedCallback = UTF8ToString(callback);
            const parsedFallback = UTF8ToString(fallback);
            const provider = new firebase.auth.GoogleAuthProvider();
            firebase.auth().signInWithPopup(provider).then((result) => {
                console.log("SignedIn with Google:", result)
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(result.user));
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            })
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
    SignInWithFacebook: function(objectName, callback, fallback) {
        try {
            const parsedObjectName = UTF8ToString(objectName);
            const parsedCallback = UTF8ToString(callback);
            const parsedFallback = UTF8ToString(fallback);
            const provider = new firebase.auth.FacebookAuthProvider();
            firebase.auth().signInWithPopup(provider).then((result) => {
                console.log("SignedIn with Facebook:", result)
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(result.user));
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            })
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },    
    SignOut: function(objectName, callback, fallback) {
        try {
            const parsedObjectName = UTF8ToString(objectName);
            const parsedCallback = UTF8ToString(callback);
            const parsedFallback = UTF8ToString(fallback);
            firebase.auth().signOut().then(() => {                
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "User signed out");
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            })
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
    OnAuthStateChanged: function(objectName, onUserSignedIn, onUserSignedOut) {
        try {
            const parsedObjectName = UTF8ToString(objectName);
            const parsedOnUserSignedIn = UTF8ToString(onUserSignedIn);
            const parsedOnUserSignedOut = UTF8ToString(onUserSignedOut);
            firebase.auth().onAuthStateChanged((user) => {
                if (user) {
                    window.unityInstance.SendMessage(parsedObjectName, parsedOnUserSignedIn, JSON.stringify(user));
                } else {
                    window.unityInstance.SendMessage(parsedObjectName, parsedOnUserSignedOut, "User signed out");
                }
            })
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedOnUserSignedOut, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    }
});